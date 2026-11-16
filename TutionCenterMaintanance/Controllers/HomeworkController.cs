using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TutionCenterMaintanance.Data;
using TutionCenterMaintanance.Data.Entities;
using TutionCenterMaintanance.Shared.Constants;
using TutionCenterMaintanance.Shared.DTOs.Homework;
using TutionCenterMaintanance.Shared.Models;

namespace TutionCenterMaintanance.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class HomeworkController : ControllerBase
{
    private readonly AppDbContext _context;

    public HomeworkController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<HomeworkDto>>>> GetHomeworkList([FromQuery] int? batchId = null)
    {
        var query = _context.Homeworks
            .Include(h => h.Batch).ThenInclude(b => b.Grade)
            .Include(h => h.Batch).ThenInclude(b => b.StudentBatches)
            .Include(h => h.Subject)
            .Include(h => h.Teacher)
            .Include(h => h.Submissions)
            .AsQueryable();

        if (batchId.HasValue) query = query.Where(h => h.BatchId == batchId.Value);

        var list = await query
            .OrderByDescending(h => h.AssignedDate)
            .Select(h => new HomeworkDto
            {
                Id = h.Id,
                BatchId = h.BatchId,
                BatchName = h.Batch.Name,
                GradeName = h.Batch.Grade.Name,
                SubjectId = h.SubjectId,
                SubjectName = h.Subject.Name,
                TeacherId = h.TeacherId,
                TeacherName = h.Teacher.Name,
                Title = h.Title,
                Description = h.Description,
                AssignedDate = h.AssignedDate,
                DueDate = h.DueDate,
                TotalStudentsCount = h.Batch.StudentBatches.Count,
                SubmittedCount = h.Submissions.Count(s => s.Status == "Submitted"),
                CheckedCount = h.Submissions.Count(s => s.Status == "Checked"),
                LateCount = h.Submissions.Count(s => s.Status == "Late")
            })
            .ToListAsync();

        return Ok(ApiResponse<List<HomeworkDto>>.Ok(list));
    }

    [HttpPost]
    [Authorize(Policy = Permissions.HomeworkManage)]
    public async Task<ActionResult<ApiResponse<HomeworkDto>>> CreateHomework([FromBody] CreateHomeworkRequest request)
    {
        var batch = await _context.Batches
            .Include(b => b.Grade)
            .Include(b => b.StudentBatches).ThenInclude(sb => sb.Student)
            .FirstOrDefaultAsync(b => b.Id == request.BatchId);

        if (batch == null) return BadRequest(ApiResponse<HomeworkDto>.Fail("Batch not found"));

        var subject = await _context.Subjects.FindAsync(request.SubjectId);
        if (subject == null) return BadRequest(ApiResponse<HomeworkDto>.Fail("Subject not found"));

        var teacher = await _context.Teachers.FindAsync(request.TeacherId);
        if (teacher == null) return BadRequest(ApiResponse<HomeworkDto>.Fail("Teacher not found"));

        var homework = new Homework
        {
            BatchId = request.BatchId,
            SubjectId = request.SubjectId,
            TeacherId = request.TeacherId,
            Title = request.Title,
            Description = request.Description,
            AssignedDate = request.AssignedDate,
            DueDate = request.DueDate
        };

        _context.Homeworks.Add(homework);
        await _context.SaveChangesAsync();

        // Initialize submission rows for enrolled active students
        var activeStudents = batch.StudentBatches.Select(sb => sb.Student).Where(s => s.IsActive).ToList();
        foreach (var s in activeStudents)
        {
            _context.HomeworkSubmissions.Add(new HomeworkSubmission
            {
                HomeworkId = homework.Id,
                StudentId = s.Id,
                Status = "Pending",
                SubmittedDate = DateTime.UtcNow
            });
        }
        await _context.SaveChangesAsync();

        var dto = new HomeworkDto
        {
            Id = homework.Id,
            BatchId = homework.BatchId,
            BatchName = batch.Name,
            GradeName = batch.Grade.Name,
            SubjectId = homework.SubjectId,
            SubjectName = subject.Name,
            TeacherId = homework.TeacherId,
            TeacherName = teacher.Name,
            Title = homework.Title,
            Description = homework.Description,
            AssignedDate = homework.AssignedDate,
            DueDate = homework.DueDate,
            TotalStudentsCount = activeStudents.Count,
            SubmittedCount = 0,
            CheckedCount = 0,
            LateCount = 0
        };

        return Ok(ApiResponse<HomeworkDto>.Ok(dto, "Homework assigned to batch"));
    }

    [HttpGet("{id}/submissions")]
    public async Task<ActionResult<ApiResponse<HomeworkDetailDto>>> GetSubmissions(int id)
    {
        var homework = await _context.Homeworks
            .Include(h => h.Batch).ThenInclude(b => b.Grade)
            .Include(h => h.Subject)
            .Include(h => h.Teacher)
            .FirstOrDefaultAsync(h => h.Id == id);

        if (homework == null) return NotFound(ApiResponse<HomeworkDetailDto>.Fail("Homework not found"));

        // Fetch all active students enrolled in this batch
        var students = await _context.Students
            .Include(s => s.Parent)
            .Include(s => s.Grade)
            .Where(s => s.StudentBatches.Any(sb => sb.BatchId == homework.BatchId) && s.IsActive)
            .ToListAsync();

        var existingSubmissions = await _context.HomeworkSubmissions
            .Where(s => s.HomeworkId == id)
            .ToListAsync();

        var submissionItems = new List<HomeworkSubmissionItemDto>();

        foreach (var s in students)
        {
            var sub = existingSubmissions.FirstOrDefault(x => x.StudentId == s.Id);
            submissionItems.Add(new HomeworkSubmissionItemDto
            {
                SubmissionId = sub?.Id,
                HomeworkId = id,
                StudentId = s.Id,
                StudentName = $"{s.FirstName} {s.LastName}".Trim(),
                GradeName = s.Grade.Name,
                ParentName = s.Parent != null ? s.Parent.FatherName : "Parent",
                ParentPhone = s.Parent != null ? s.Parent.FatherPhone : "",
                WhatsAppPhone = s.Parent != null && !string.IsNullOrEmpty(s.Parent.FatherWhatsApp) ? s.Parent.FatherWhatsApp : s.Parent?.FatherPhone,
                Status = sub?.Status ?? "Pending",
                Remarks = sub?.Remarks,
                SubmittedDate = sub?.SubmittedDate
            });
        }

        var result = new HomeworkDetailDto
        {
            Homework = new HomeworkDto
            {
                Id = homework.Id,
                BatchId = homework.BatchId,
                BatchName = homework.Batch.Name,
                GradeName = homework.Batch.Grade.Name,
                SubjectId = homework.SubjectId,
                SubjectName = homework.Subject.Name,
                TeacherId = homework.TeacherId,
                TeacherName = homework.Teacher.Name,
                Title = homework.Title,
                Description = homework.Description,
                AssignedDate = homework.AssignedDate,
                DueDate = homework.DueDate,
                TotalStudentsCount = students.Count,
                SubmittedCount = submissionItems.Count(s => s.Status == "Submitted"),
                CheckedCount = submissionItems.Count(s => s.Status == "Checked"),
                LateCount = submissionItems.Count(s => s.Status == "Late")
            },
            Submissions = submissionItems
        };

        return Ok(ApiResponse<HomeworkDetailDto>.Ok(result));
    }

    [HttpPost("{id}/submissions")]
    [Authorize(Policy = Permissions.HomeworkManage)]
    public async Task<ActionResult<ApiResponse<bool>>> BulkUpdateSubmissions(int id, [FromBody] BulkUpdateHomeworkSubmissionsRequest request)
    {
        var existing = await _context.HomeworkSubmissions.Where(s => s.HomeworkId == id).ToListAsync();

        foreach (var item in request.Submissions)
        {
            var sub = existing.FirstOrDefault(s => s.StudentId == item.StudentId);
            if (sub != null)
            {
                sub.Status = item.Status;
                sub.Remarks = item.Remarks;
                sub.ModifiedAt = DateTime.UtcNow;
            }
            else
            {
                _context.HomeworkSubmissions.Add(new HomeworkSubmission
                {
                    HomeworkId = id,
                    StudentId = item.StudentId,
                    Status = item.Status,
                    Remarks = item.Remarks,
                    SubmittedDate = DateTime.UtcNow
                });
            }
        }

        await _context.SaveChangesAsync();
        return Ok(ApiResponse<bool>.Ok(true, "Homework submissions updated"));
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = Permissions.HomeworkManage)]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteHomework(int id)
    {
        var homework = await _context.Homeworks.FindAsync(id);
        if (homework == null) return NotFound(ApiResponse<bool>.Fail("Homework not found"));

        homework.IsDeleted = true;
        homework.ModifiedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return Ok(ApiResponse<bool>.Ok(true, "Homework deleted"));
    }
}
