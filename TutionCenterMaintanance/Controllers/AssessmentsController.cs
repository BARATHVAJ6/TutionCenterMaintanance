using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TutionCenterMaintanance.Data;
using TutionCenterMaintanance.Data.Entities;
using TutionCenterMaintanance.Shared.Constants;
using TutionCenterMaintanance.Shared.DTOs.Assessments;
using TutionCenterMaintanance.Shared.Models;

namespace TutionCenterMaintanance.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AssessmentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public AssessmentsController(AppDbContext context)
    {
        _context = context;
    }

    // --- Assessment Types ---
    [HttpGet("types")]
    public async Task<ActionResult<ApiResponse<List<AssessmentTypeDto>>>> GetAssessmentTypes()
    {
        var types = await _context.AssessmentTypes
            .Select(t => new AssessmentTypeDto
            {
                Id = t.Id,
                Name = t.Name,
                Category = t.Category,
                Description = t.Description
            })
            .ToListAsync();

        return Ok(ApiResponse<List<AssessmentTypeDto>>.Ok(types));
    }

    [HttpPost("types")]
    [Authorize(Policy = Permissions.AssessmentTypesManage)]
    public async Task<ActionResult<ApiResponse<AssessmentTypeDto>>> CreateAssessmentType([FromBody] AssessmentTypeDto dto)
    {
        var type = new AssessmentType
        {
            Name = dto.Name,
            Category = dto.Category,
            Description = dto.Description
        };

        _context.AssessmentTypes.Add(type);
        await _context.SaveChangesAsync();
        dto.Id = type.Id;
        return Ok(ApiResponse<AssessmentTypeDto>.Ok(dto, "Assessment type created successfully"));
    }

    // --- Assessments ---
    [HttpGet]
    [Authorize(Policy = Permissions.MarksView)]
    public async Task<ActionResult<ApiResponse<List<AssessmentDto>>>> GetAssessments(
        [FromQuery] int? gradeId,
        [FromQuery] int? boardId,
        [FromQuery] int? academicYearId)
    {
        var query = _context.Assessments
            .Include(a => a.AssessmentType)
            .Include(a => a.AcademicYear)
            .Include(a => a.Grade)
            .Include(a => a.Board)
            .Include(a => a.AssessmentSubjects)
                .ThenInclude(asub => asub.Subject)
            .AsQueryable();

        if (gradeId.HasValue) query = query.Where(a => a.GradeId == gradeId.Value);
        if (boardId.HasValue) query = query.Where(a => a.BoardId == boardId.Value);
        if (academicYearId.HasValue) query = query.Where(a => a.AcademicYearId == academicYearId.Value);

        var list = await query
            .OrderByDescending(a => a.Date)
            .Select(a => new AssessmentDto
            {
                Id = a.Id,
                Name = a.Name,
                AssessmentTypeId = a.AssessmentTypeId,
                AssessmentTypeName = a.AssessmentType.Name,
                Category = a.AssessmentType.Category,
                AcademicYearId = a.AcademicYearId,
                AcademicYearName = a.AcademicYear.Name,
                GradeId = a.GradeId,
                GradeName = a.Grade.Name,
                BoardId = a.BoardId,
                BoardName = a.Board.Name,
                Date = a.Date,
                TotalMarks = a.AssessmentSubjects.Sum(asub => asub.MaxMarks),
                Subjects = a.AssessmentSubjects.Select(asub => new AssessmentSubjectDto
                {
                    Id = asub.Id,
                    AssessmentId = asub.AssessmentId,
                    SubjectId = asub.SubjectId,
                    SubjectName = asub.Subject.Name,
                    MaxMarks = asub.MaxMarks
                }).ToList()
            })
            .ToListAsync();

        return Ok(ApiResponse<List<AssessmentDto>>.Ok(list));
    }

    [HttpPost]
    [Authorize(Policy = Permissions.AssessmentsManage)]
    public async Task<ActionResult<ApiResponse<AssessmentDto>>> CreateAssessment([FromBody] CreateAssessmentRequest request)
    {
        var assessment = new Assessment
        {
            Name = request.Name,
            AssessmentTypeId = request.AssessmentTypeId,
            AcademicYearId = request.AcademicYearId,
            GradeId = request.GradeId,
            BoardId = request.BoardId,
            Date = request.Date
        };

        _context.Assessments.Add(assessment);
        await _context.SaveChangesAsync();

        foreach (var sub in request.Subjects)
        {
            _context.AssessmentSubjects.Add(new AssessmentSubject
            {
                AssessmentId = assessment.Id,
                SubjectId = sub.SubjectId,
                MaxMarks = sub.MaxMarks
            });
        }

        await _context.SaveChangesAsync();

        return Ok(ApiResponse<AssessmentDto>.Ok(new AssessmentDto
        {
            Id = assessment.Id,
            Name = assessment.Name,
            AssessmentTypeId = assessment.AssessmentTypeId,
            AcademicYearId = assessment.AcademicYearId,
            GradeId = assessment.GradeId,
            BoardId = assessment.BoardId,
            Date = assessment.Date
        }, "Assessment created successfully"));
    }
}
