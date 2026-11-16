using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TutionCenterMaintanance.Data;
using TutionCenterMaintanance.Data.Entities;
using TutionCenterMaintanance.Shared.Constants;
using TutionCenterMaintanance.Shared.DTOs.Communication;
using TutionCenterMaintanance.Shared.Enums;
using TutionCenterMaintanance.Shared.Models;

namespace TutionCenterMaintanance.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WhatsAppController : ControllerBase
{
    private readonly AppDbContext _context;

    public WhatsAppController(AppDbContext context)
    {
        _context = context;
    }

    // --- Templates ---
    [HttpGet("templates")]
    public async Task<ActionResult<ApiResponse<List<WhatsAppTemplateDto>>>> GetTemplates()
    {
        var templates = await _context.WhatsAppTemplates
            .OrderBy(t => t.Category)
            .ThenBy(t => t.Name)
            .Select(t => new WhatsAppTemplateDto
            {
                Id = t.Id,
                Name = t.Name,
                Category = t.Category,
                BodyTemplate = t.BodyTemplate,
                IsSystem = t.IsSystem
            })
            .ToListAsync();

        return Ok(ApiResponse<List<WhatsAppTemplateDto>>.Ok(templates));
    }

    [HttpPost("templates")]
    [Authorize(Policy = Permissions.MessagingSend)]
    public async Task<ActionResult<ApiResponse<WhatsAppTemplateDto>>> CreateTemplate([FromBody] CreateOrUpdateTemplateRequest request)
    {
        var template = new WhatsAppTemplate
        {
            Name = request.Name,
            Category = request.Category,
            BodyTemplate = request.BodyTemplate,
            IsSystem = false
        };

        _context.WhatsAppTemplates.Add(template);
        await _context.SaveChangesAsync();

        var dto = new WhatsAppTemplateDto
        {
            Id = template.Id,
            Name = template.Name,
            Category = template.Category,
            BodyTemplate = template.BodyTemplate,
            IsSystem = false
        };

        return Ok(ApiResponse<WhatsAppTemplateDto>.Ok(dto, "Template created successfully"));
    }

    [HttpPut("templates/{id}")]
    [Authorize(Policy = Permissions.MessagingSend)]
    public async Task<ActionResult<ApiResponse<bool>>> UpdateTemplate(int id, [FromBody] CreateOrUpdateTemplateRequest request)
    {
        var template = await _context.WhatsAppTemplates.FindAsync(id);
        if (template == null) return NotFound();

        template.Name = request.Name;
        template.Category = request.Category;
        template.BodyTemplate = request.BodyTemplate;
        template.ModifiedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(ApiResponse<bool>.Ok(true, "Template updated successfully"));
    }

    [HttpDelete("templates/{id}")]
    [Authorize(Policy = Permissions.MessagingSend)]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteTemplate(int id)
    {
        var template = await _context.WhatsAppTemplates.FindAsync(id);
        if (template == null) return NotFound();

        if (template.IsSystem)
        {
            return Ok(ApiResponse<bool>.Fail("Default system templates cannot be deleted."));
        }

        template.IsDeleted = true;
        template.ModifiedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return Ok(ApiResponse<bool>.Ok(true, "Template deleted successfully"));
    }

    // --- Message Logging ---
    [HttpGet("logs")]
    [Authorize(Policy = Permissions.MessagingSend)]
    public async Task<ActionResult<ApiResponse<List<WhatsAppMessageLogDto>>>> GetMessageLogs([FromQuery] int? studentId = null)
    {
        var query = _context.WhatsAppMessageLogs
            .Include(l => l.Student)
            .Include(l => l.SentByUser)
            .AsQueryable();

        if (studentId.HasValue)
        {
            query = query.Where(l => l.StudentId == studentId.Value);
        }

        var logs = await query
            .OrderByDescending(l => l.SentAt)
            .Take(100)
            .Select(l => new WhatsAppMessageLogDto
            {
                Id = l.Id,
                StudentId = l.StudentId,
                StudentName = l.Student != null ? $"{l.Student.FirstName} {l.Student.LastName}".Trim() : null,
                RecipientName = l.RecipientName,
                RecipientPhone = l.RecipientPhone,
                MessageCategory = l.MessageCategory,
                MessageBody = l.MessageBody,
                SentByName = l.SentByUser != null ? l.SentByUser.FullName : "Staff",
                SentAt = l.SentAt
            })
            .ToListAsync();

        return Ok(ApiResponse<List<WhatsAppMessageLogDto>>.Ok(logs));
    }

    [HttpPost("logs")]
    [Authorize(Policy = Permissions.MessagingSend)]
    public async Task<ActionResult<ApiResponse<bool>>> LogDispatch([FromBody] LogWhatsAppDispatchDto request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var log = new WhatsAppMessageLog
        {
            StudentId = request.StudentId,
            RecipientName = request.RecipientName,
            RecipientPhone = request.RecipientPhone,
            MessageCategory = request.MessageCategory,
            MessageBody = request.MessageBody,
            SentByUserId = userId,
            SentAt = DateTime.UtcNow
        };

        _context.WhatsAppMessageLogs.Add(log);
        await _context.SaveChangesAsync();

        return Ok(ApiResponse<bool>.Ok(true, "Dispatch recorded"));
    }

    // --- Targeted WhatsApp feeds ---
    [HttpGet("targets/overdue")]
    [Authorize(Policy = Permissions.MessagingSend)]
    public async Task<ActionResult<ApiResponse<List<BulkWhatsAppRecipientDto>>>> GetOverdueTargets([FromQuery] int? gradeId = null)
    {
        var center = await _context.CenterSettings.FirstOrDefaultAsync() ?? new CenterSettings();
        var template = await _context.WhatsAppTemplates
            .FirstOrDefaultAsync(t => t.Category == "FeeReminder")
            ?? new WhatsAppTemplate
            {
                BodyTemplate = "Dear {ParentName},\n\nFee of ₹{DueAmount} for {StudentName} ({Grade}) is pending at {CenterName}. Kindly pay at earliest.\n\nThank you!"
            };

        var studentsQuery = _context.Students
            .Include(s => s.Grade)
            .Include(s => s.Board)
            .Include(s => s.Parent)
            .Include(s => s.StudentFeePlans)
                .ThenInclude(sfp => sfp.Installments)
            .Include(s => s.Payments)
            .Where(s => s.IsActive);

        if (gradeId.HasValue)
        {
            studentsQuery = studentsQuery.Where(s => s.GradeId == gradeId.Value);
        }

        var students = await studentsQuery.ToListAsync();
        var targets = new List<BulkWhatsAppRecipientDto>();

        foreach (var s in students)
        {
            var totalFee = s.StudentFeePlans.Sum(sfp => sfp.FinalAmount);
            var paid = s.Payments.Sum(p => p.Amount);
            var due = totalFee - paid;

            if (due <= 0) continue;

            var phone = !string.IsNullOrEmpty(s.Parent.FatherWhatsApp) ? s.Parent.FatherWhatsApp : s.Parent.FatherPhone;
            var cleanPhone = CleanPhoneNumber(phone);

            var message = template.BodyTemplate
                .Replace("{ParentName}", s.Parent.FatherName)
                .Replace("{StudentName}", $"{s.FirstName} {s.LastName}".Trim())
                .Replace("{Grade}", s.Grade.Name)
                .Replace("{Board}", s.Board.Name)
                .Replace("{DueAmount}", due.ToString("N0"))
                .Replace("{CenterName}", center.CenterName)
                .Replace("{CenterPhone}", center.Phone ?? "");

            targets.Add(new BulkWhatsAppRecipientDto
            {
                StudentId = s.Id,
                StudentName = $"{s.FirstName} {s.LastName}".Trim(),
                GradeName = s.Grade.Name,
                ParentName = s.Parent.FatherName,
                RawPhone = phone,
                CleanPhone = cleanPhone,
                Category = "FeeReminder",
                Details = $"₹{due:N0} Overdue",
                RenderedMessage = message
            });
        }

        return Ok(ApiResponse<List<BulkWhatsAppRecipientDto>>.Ok(targets));
    }

    [HttpGet("targets/absent")]
    [Authorize(Policy = Permissions.MessagingSend)]
    public async Task<ActionResult<ApiResponse<List<BulkWhatsAppRecipientDto>>>> GetAbsentTargets([FromQuery] DateTime? date = null)
    {
        var targetDate = (date ?? DateTime.Today).Date;
        var center = await _context.CenterSettings.FirstOrDefaultAsync() ?? new CenterSettings();
        var template = await _context.WhatsAppTemplates
            .FirstOrDefaultAsync(t => t.Category == "Attendance")
            ?? new WhatsAppTemplate
            {
                BodyTemplate = "Dear {ParentName},\n\n{StudentName} ({Grade}) was absent today ({AttendanceDate}) for tuition classes.\n\nRegards,\n{CenterName}"
            };

        var absentees = await _context.Attendances
            .Include(a => a.Student)
                .ThenInclude(s => s.Grade)
            .Include(a => a.Student)
                .ThenInclude(s => s.Parent)
            .Where(a => a.Date.Date == targetDate && a.Status == AttendanceStatus.Absent)
            .ToListAsync();

        var targets = new List<BulkWhatsAppRecipientDto>();
        foreach (var a in absentees)
        {
            var s = a.Student;
            var phone = !string.IsNullOrEmpty(s.Parent.FatherWhatsApp) ? s.Parent.FatherWhatsApp : s.Parent.FatherPhone;
            var cleanPhone = CleanPhoneNumber(phone);

            var message = template.BodyTemplate
                .Replace("{ParentName}", s.Parent.FatherName)
                .Replace("{StudentName}", $"{s.FirstName} {s.LastName}".Trim())
                .Replace("{Grade}", s.Grade.Name)
                .Replace("{AttendanceDate}", targetDate.ToString("dd MMM yyyy"))
                .Replace("{CenterName}", center.CenterName)
                .Replace("{CenterPhone}", center.Phone ?? "");

            targets.Add(new BulkWhatsAppRecipientDto
            {
                StudentId = s.Id,
                StudentName = $"{s.FirstName} {s.LastName}".Trim(),
                GradeName = s.Grade.Name,
                ParentName = s.Parent.FatherName,
                RawPhone = phone,
                CleanPhone = cleanPhone,
                Category = "Attendance",
                Details = targetDate.ToString("dd MMM yyyy"),
                RenderedMessage = message
            });
        }

        return Ok(ApiResponse<List<BulkWhatsAppRecipientDto>>.Ok(targets));
    }

    [HttpGet("targets/assessment/{assessmentId}")]
    [Authorize(Policy = Permissions.MessagingSend)]
    public async Task<ActionResult<ApiResponse<List<BulkWhatsAppRecipientDto>>>> GetAssessmentTargets(int assessmentId)
    {
        var center = await _context.CenterSettings.FirstOrDefaultAsync() ?? new CenterSettings();
        var template = await _context.WhatsAppTemplates
            .FirstOrDefaultAsync(t => t.Category == "ExamResult")
            ?? new WhatsAppTemplate
            {
                BodyTemplate = "Dear {ParentName},\n\nResult for {StudentName} in {AssessmentName}:\nScore: {MarksObtained}/{TotalMarks} ({Percentage}%).\n\n{CenterName}"
            };

        var assessment = await _context.Assessments
            .Include(a => a.Grade)
            .Include(a => a.AssessmentSubjects)
                .ThenInclude(asub => asub.Subject)
            .Include(a => a.AssessmentSubjects)
                .ThenInclude(asub => asub.Marks)
                    .ThenInclude(m => m.Student)
                        .ThenInclude(s => s.Parent)
            .FirstOrDefaultAsync(a => a.Id == assessmentId);

        if (assessment == null) return NotFound();

        var totalMax = assessment.AssessmentSubjects.Sum(s => s.MaxMarks);

        // Group marks by student
        var studentMarks = new Dictionary<int, (Student Student, decimal TotalObtained, bool IsAbsent, string Remarks)>();

        foreach (var asub in assessment.AssessmentSubjects)
        {
            foreach (var m in asub.Marks)
            {
                if (!studentMarks.ContainsKey(m.StudentId))
                {
                    studentMarks[m.StudentId] = (m.Student, 0, m.IsAbsent, m.Remarks ?? "");
                }

                var current = studentMarks[m.StudentId];
                var newTotal = current.TotalObtained + (m.MarksObtained ?? 0);
                studentMarks[m.StudentId] = (current.Student, newTotal, current.IsAbsent || m.IsAbsent, current.Remarks);
            }
        }

        var targets = new List<BulkWhatsAppRecipientDto>();
        foreach (var kvp in studentMarks)
        {
            var s = kvp.Value.Student;
            var phone = !string.IsNullOrEmpty(s.Parent.FatherWhatsApp) ? s.Parent.FatherWhatsApp : s.Parent.FatherPhone;
            var cleanPhone = CleanPhoneNumber(phone);
            var pct = totalMax > 0 ? Math.Round((kvp.Value.TotalObtained / totalMax) * 100, 1) : 0;

            var message = template.BodyTemplate
                .Replace("{ParentName}", s.Parent.FatherName)
                .Replace("{StudentName}", $"{s.FirstName} {s.LastName}".Trim())
                .Replace("{Grade}", assessment.Grade.Name)
                .Replace("{AssessmentName}", assessment.Name)
                .Replace("{MarksObtained}", kvp.Value.IsAbsent ? "ABSENT" : kvp.Value.TotalObtained.ToString("N1"))
                .Replace("{TotalMarks}", totalMax.ToString("N0"))
                .Replace("{Percentage}", kvp.Value.IsAbsent ? "0" : pct.ToString("N1"))
                .Replace("{Remarks}", string.IsNullOrEmpty(kvp.Value.Remarks) ? "Keep working hard" : kvp.Value.Remarks)
                .Replace("{CenterName}", center.CenterName)
                .Replace("{CenterPhone}", center.Phone ?? "");

            targets.Add(new BulkWhatsAppRecipientDto
            {
                StudentId = s.Id,
                StudentName = $"{s.FirstName} {s.LastName}".Trim(),
                GradeName = assessment.Grade.Name,
                ParentName = s.Parent.FatherName,
                RawPhone = phone,
                CleanPhone = cleanPhone,
                Category = "ExamResult",
                Details = $"{kvp.Value.TotalObtained:N1} / {totalMax:N0} ({pct:N1}%)",
                RenderedMessage = message
            });
        }

        return Ok(ApiResponse<List<BulkWhatsAppRecipientDto>>.Ok(targets));
    }

    private static string CleanPhoneNumber(string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone)) return string.Empty;
        var digits = new string(phone.Where(char.IsDigit).ToArray());
        // Indian number format check: if 10 digits without 91, add 91
        if (digits.Length == 10)
        {
            digits = "91" + digits;
        }
        return digits;
    }
}
