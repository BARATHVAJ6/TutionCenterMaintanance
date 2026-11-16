using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TutionCenterMaintanance.Data;
using TutionCenterMaintanance.Data.Entities;
using TutionCenterMaintanance.Shared.Constants;
using TutionCenterMaintanance.Shared.DTOs.Assessments;
using TutionCenterMaintanance.Shared.Models;

namespace TutionCenterMaintanance.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MarksController : ControllerBase
{
    private readonly AppDbContext _context;

    public MarksController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("grid/{assessmentSubjectId}")]
    [Authorize(Policy = Permissions.MarksView)]
    public async Task<ActionResult<ApiResponse<BulkMarksGridDto>>> GetMarksGrid(int assessmentSubjectId, [FromQuery] int? batchId)
    {
        var asub = await _context.AssessmentSubjects
            .Include(a => a.Assessment)
            .Include(a => a.Subject)
            .FirstOrDefaultAsync(a => a.Id == assessmentSubjectId);

        if (asub == null) return NotFound();

        // Get students for this Assessment's Grade & Board
        var studentsQuery = _context.Students
            .Where(s => s.GradeId == asub.Assessment.GradeId && s.BoardId == asub.Assessment.BoardId && s.IsActive);

        if (batchId.HasValue)
        {
            studentsQuery = studentsQuery.Where(s => s.StudentBatches.Any(sb => sb.BatchId == batchId.Value));
        }

        var students = await studentsQuery
            .OrderBy(s => s.FirstName)
            .ThenBy(s => s.LastName)
            .ToListAsync();

        var existingMarks = await _context.Marks
            .Where(m => m.AssessmentSubjectId == assessmentSubjectId)
            .ToDictionaryAsync(m => m.StudentId);

        var rows = new List<StudentMarkRowDto>();
        foreach (var st in students)
        {
            existingMarks.TryGetValue(st.Id, out var m);
            rows.Add(new StudentMarkRowDto
            {
                StudentId = st.Id,
                StudentName = $"{st.FirstName} {st.LastName}".Trim(),
                MarksObtained = m?.MarksObtained,
                IsAbsent = m?.IsAbsent ?? false,
                Remarks = m?.Remarks
            });
        }

        var result = new BulkMarksGridDto
        {
            AssessmentId = asub.AssessmentId,
            AssessmentName = asub.Assessment.Name,
            AssessmentSubjectId = asub.Id,
            SubjectName = asub.Subject.Name,
            MaxMarks = asub.MaxMarks,
            Rows = rows
        };

        return Ok(ApiResponse<BulkMarksGridDto>.Ok(result));
    }

    [HttpPost("bulk-save")]
    [Authorize(Policy = Permissions.MarksEntry)]
    public async Task<ActionResult<ApiResponse<bool>>> SaveBulkMarks([FromBody] BulkSaveMarksRequest request)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var existingMarks = await _context.Marks
            .Where(m => m.AssessmentSubjectId == request.AssessmentSubjectId)
            .ToListAsync();

        var markMap = existingMarks.ToDictionary(m => m.StudentId);

        foreach (var item in request.Marks)
        {
            if (markMap.TryGetValue(item.StudentId, out var existing))
            {
                existing.MarksObtained = item.IsAbsent ? null : item.MarksObtained;
                existing.IsAbsent = item.IsAbsent;
                existing.Remarks = item.Remarks;
                existing.EnteredByUserId = currentUserId;
                existing.EnteredAt = DateTime.UtcNow;
                existing.ModifiedAt = DateTime.UtcNow;
            }
            else
            {
                _context.Marks.Add(new Mark
                {
                    AssessmentSubjectId = request.AssessmentSubjectId,
                    StudentId = item.StudentId,
                    MarksObtained = item.IsAbsent ? null : item.MarksObtained,
                    IsAbsent = item.IsAbsent,
                    Remarks = item.Remarks,
                    EnteredByUserId = currentUserId,
                    EnteredAt = DateTime.UtcNow
                });
            }
        }

        await _context.SaveChangesAsync();
        return Ok(ApiResponse<bool>.Ok(true, "Marks recorded successfully"));
    }

    [HttpGet("report-card/{studentId}")]
    [Authorize(Policy = Permissions.ReportsView)]
    public async Task<ActionResult<ApiResponse<StudentReportCardDto>>> GetStudentReportCard(
        int studentId, [FromQuery] int? academicYearId)
    {
        var student = await _context.Students
            .Include(s => s.Grade)
            .Include(s => s.Board)
            .FirstOrDefaultAsync(s => s.Id == studentId);

        if (student == null) return NotFound();

        var assessmentsQuery = _context.Assessments
            .Include(a => a.AssessmentType)
            .Include(a => a.AssessmentSubjects)
                .ThenInclude(asub => asub.Subject)
            .Include(a => a.AssessmentSubjects)
                .ThenInclude(asub => asub.Marks.Where(m => m.StudentId == studentId))
            .Where(a => a.GradeId == student.GradeId && a.BoardId == student.BoardId);

        if (academicYearId.HasValue)
        {
            assessmentsQuery = assessmentsQuery.Where(a => a.AcademicYearId == academicYearId.Value);
        }

        var assessments = await assessmentsQuery.OrderBy(a => a.Date).ToListAsync();

        var examSections = new List<ReportCardExamSectionDto>();

        foreach (var a in assessments)
        {
            var subjectDetails = new List<SubjectMarkDetailDto>();
            decimal totalObt = 0;
            decimal totalMax = 0;

            foreach (var asub in a.AssessmentSubjects)
            {
                var mark = asub.Marks.FirstOrDefault();
                subjectDetails.Add(new SubjectMarkDetailDto
                {
                    SubjectName = asub.Subject.Name,
                    MarksObtained = mark?.MarksObtained,
                    MaxMarks = asub.MaxMarks,
                    IsAbsent = mark?.IsAbsent ?? false,
                    Remarks = mark?.Remarks
                });

                totalMax += asub.MaxMarks;
                if (mark?.MarksObtained.HasValue == true)
                {
                    totalObt += mark.MarksObtained.Value;
                }
            }

            examSections.Add(new ReportCardExamSectionDto
            {
                AssessmentName = a.Name,
                AssessmentType = a.AssessmentType.Name,
                Category = a.AssessmentType.Category,
                Date = a.Date,
                Marks = subjectDetails,
                TotalObtained = totalObt,
                TotalMaximum = totalMax
            });
        }

        var reportCard = new StudentReportCardDto
        {
            StudentId = student.Id,
            StudentName = $"{student.FirstName} {student.LastName}".Trim(),
            GradeName = student.Grade.Name,
            BoardName = student.Board.Name,
            Exams = examSections
        };

        return Ok(ApiResponse<StudentReportCardDto>.Ok(reportCard));
    }
}
