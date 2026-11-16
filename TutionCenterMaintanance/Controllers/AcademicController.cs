using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TutionCenterMaintanance.Data;
using TutionCenterMaintanance.Data.Entities;
using TutionCenterMaintanance.Shared.Constants;
using TutionCenterMaintanance.Shared.DTOs.Academic;
using TutionCenterMaintanance.Shared.Models;

namespace TutionCenterMaintanance.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AcademicController : ControllerBase
{
    private readonly AppDbContext _context;

    public AcademicController(AppDbContext context)
    {
        _context = context;
    }

    // --- Boards ---
    [HttpGet("boards")]
    public async Task<ActionResult<ApiResponse<List<BoardDto>>>> GetBoards()
    {
        var boards = await _context.Boards
            .Select(b => new BoardDto
            {
                Id = b.Id,
                Name = b.Name,
                Code = b.Code,
                IsActive = b.IsActive
            })
            .ToListAsync();

        return Ok(ApiResponse<List<BoardDto>>.Ok(boards));
    }

    [HttpPost("boards")]
    [Authorize(Policy = Permissions.BoardsManage)]
    public async Task<ActionResult<ApiResponse<BoardDto>>> CreateBoard([FromBody] BoardDto dto)
    {
        var board = new Board
        {
            Name = dto.Name,
            Code = dto.Code,
            IsActive = dto.IsActive
        };
        _context.Boards.Add(board);
        await _context.SaveChangesAsync();
        dto.Id = board.Id;
        return Ok(ApiResponse<BoardDto>.Ok(dto, "Board created successfully"));
    }

    [HttpPut("boards/{id}")]
    [Authorize(Policy = Permissions.BoardsManage)]
    public async Task<ActionResult<ApiResponse<bool>>> UpdateBoard(int id, [FromBody] BoardDto dto)
    {
        var board = await _context.Boards.FindAsync(id);
        if (board == null) return NotFound();

        board.Name = dto.Name;
        board.Code = dto.Code;
        board.IsActive = dto.IsActive;
        board.ModifiedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return Ok(ApiResponse<bool>.Ok(true, "Board updated successfully"));
    }

    [HttpDelete("boards/{id}")]
    [Authorize(Policy = Permissions.BoardsManage)]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteBoard(int id)
    {
        var board = await _context.Boards.FindAsync(id);
        if (board == null) return NotFound();

        board.IsDeleted = true;
        board.ModifiedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return Ok(ApiResponse<bool>.Ok(true, "Board deleted successfully"));
    }

    // --- Grades ---
    [HttpGet("grades")]
    public async Task<ActionResult<ApiResponse<List<GradeDto>>>> GetGrades()
    {
        var grades = await _context.Grades
            .OrderBy(g => g.OrderIndex)
            .Select(g => new GradeDto
            {
                Id = g.Id,
                Name = g.Name,
                OrderIndex = g.OrderIndex
            })
            .ToListAsync();

        return Ok(ApiResponse<List<GradeDto>>.Ok(grades));
    }

    // --- Subjects ---
    [HttpGet("subjects")]
    public async Task<ActionResult<ApiResponse<List<SubjectDto>>>> GetSubjects()
    {
        var subjects = await _context.Subjects
            .Select(s => new SubjectDto
            {
                Id = s.Id,
                Name = s.Name,
                Code = s.Code,
                IsActive = s.IsActive
            })
            .ToListAsync();

        return Ok(ApiResponse<List<SubjectDto>>.Ok(subjects));
    }

    [HttpPost("subjects")]
    [Authorize(Policy = Permissions.SubjectsManage)]
    public async Task<ActionResult<ApiResponse<SubjectDto>>> CreateSubject([FromBody] SubjectDto dto)
    {
        var subject = new Subject
        {
            Name = dto.Name,
            Code = dto.Code,
            IsActive = dto.IsActive
        };
        _context.Subjects.Add(subject);
        await _context.SaveChangesAsync();
        dto.Id = subject.Id;
        return Ok(ApiResponse<SubjectDto>.Ok(dto, "Subject created successfully"));
    }

    [HttpPut("subjects/{id}")]
    [Authorize(Policy = Permissions.SubjectsManage)]
    public async Task<ActionResult<ApiResponse<bool>>> UpdateSubject(int id, [FromBody] SubjectDto dto)
    {
        var subject = await _context.Subjects.FindAsync(id);
        if (subject == null) return NotFound();

        subject.Name = dto.Name;
        subject.Code = dto.Code;
        subject.IsActive = dto.IsActive;
        subject.ModifiedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return Ok(ApiResponse<bool>.Ok(true, "Subject updated successfully"));
    }

    [HttpDelete("subjects/{id}")]
    [Authorize(Policy = Permissions.SubjectsManage)]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteSubject(int id)
    {
        var subject = await _context.Subjects.FindAsync(id);
        if (subject == null) return NotFound();

        subject.IsDeleted = true;
        subject.ModifiedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return Ok(ApiResponse<bool>.Ok(true, "Subject deleted successfully"));
    }

    // --- Board-Grade-Subject Mappings ---
    [HttpGet("mappings")]
    public async Task<ActionResult<ApiResponse<List<BoardGradeSubjectDto>>>> GetMappings(
        [FromQuery] int? boardId, [FromQuery] int? gradeId)
    {
        var query = _context.BoardGradeSubjects
            .Include(m => m.Board)
            .Include(m => m.Grade)
            .Include(m => m.Subject)
            .AsQueryable();

        if (boardId.HasValue) query = query.Where(m => m.BoardId == boardId.Value);
        if (gradeId.HasValue) query = query.Where(m => m.GradeId == gradeId.Value);

        var list = await query.Select(m => new BoardGradeSubjectDto
        {
            Id = m.Id,
            BoardId = m.BoardId,
            BoardName = m.Board.Name,
            GradeId = m.GradeId,
            GradeName = m.Grade.Name,
            SubjectId = m.SubjectId,
            SubjectName = m.Subject.Name
        }).ToListAsync();

        return Ok(ApiResponse<List<BoardGradeSubjectDto>>.Ok(list));
    }

    [HttpPost("mappings")]
    [Authorize(Policy = Permissions.SubjectsManage)]
    public async Task<ActionResult<ApiResponse<bool>>> SaveMappings([FromBody] ManageBoardGradeSubjectsRequest request)
    {
        var existing = await _context.BoardGradeSubjects
            .Where(m => m.BoardId == request.BoardId && m.GradeId == request.GradeId)
            .ToListAsync();

        _context.BoardGradeSubjects.RemoveRange(existing);

        foreach (var subId in request.SubjectIds)
        {
            _context.BoardGradeSubjects.Add(new BoardGradeSubject
            {
                BoardId = request.BoardId,
                GradeId = request.GradeId,
                SubjectId = subId
            });
        }

        await _context.SaveChangesAsync();
        return Ok(ApiResponse<bool>.Ok(true, "Subjects mapped to Board and Grade successfully"));
    }

    // --- Academic Years ---
    [HttpGet("years")]
    public async Task<ActionResult<ApiResponse<List<AcademicYearDto>>>> GetAcademicYears()
    {
        var years = await _context.AcademicYears
            .OrderByDescending(y => y.StartDate)
            .Select(y => new AcademicYearDto
            {
                Id = y.Id,
                Name = y.Name,
                StartDate = y.StartDate,
                EndDate = y.EndDate,
                IsCurrent = y.IsCurrent
            })
            .ToListAsync();

        return Ok(ApiResponse<List<AcademicYearDto>>.Ok(years));
    }

    [HttpPost("years")]
    [Authorize(Policy = Permissions.AcademicYearsManage)]
    public async Task<ActionResult<ApiResponse<AcademicYearDto>>> CreateAcademicYear([FromBody] AcademicYearDto dto)
    {
        if (dto.IsCurrent)
        {
            var currents = await _context.AcademicYears.Where(y => y.IsCurrent).ToListAsync();
            foreach (var c in currents) c.IsCurrent = false;
        }

        var year = new AcademicYear
        {
            Name = dto.Name,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            IsCurrent = dto.IsCurrent
        };

        _context.AcademicYears.Add(year);
        await _context.SaveChangesAsync();
        dto.Id = year.Id;
        return Ok(ApiResponse<AcademicYearDto>.Ok(dto, "Academic Year created successfully"));
    }

    [HttpPost("years/{id}/set-current")]
    [Authorize(Policy = Permissions.AcademicYearsManage)]
    public async Task<ActionResult<ApiResponse<bool>>> SetCurrentAcademicYear(int id)
    {
        var years = await _context.AcademicYears.ToListAsync();
        foreach (var y in years)
        {
            y.IsCurrent = (y.Id == id);
        }
        await _context.SaveChangesAsync();
        return Ok(ApiResponse<bool>.Ok(true, "Current academic year updated"));
    }

    // --- Batches ---
    [HttpGet("batches")]
    public async Task<ActionResult<ApiResponse<List<BatchDto>>>> GetBatches(
        [FromQuery] int? gradeId, [FromQuery] int? boardId)
    {
        var query = _context.Batches
            .Include(b => b.Grade)
            .Include(b => b.Board)
            .Include(b => b.AcademicYear)
            .Include(b => b.StudentBatches)
            .AsQueryable();

        if (gradeId.HasValue) query = query.Where(b => b.GradeId == gradeId.Value);
        if (boardId.HasValue) query = query.Where(b => b.BoardId == boardId.Value);

        var batches = await query.Select(b => new BatchDto
        {
            Id = b.Id,
            Name = b.Name,
            GradeId = b.GradeId,
            GradeName = b.Grade.Name,
            BoardId = b.BoardId,
            BoardName = b.Board.Name,
            AcademicYearId = b.AcademicYearId,
            AcademicYearName = b.AcademicYear.Name,
            Schedule = b.Schedule,
            IsActive = b.IsActive,
            StudentCount = b.StudentBatches.Count,
            StudentIds = b.StudentBatches.Select(sb => sb.StudentId).ToList()
        }).ToListAsync();

        return Ok(ApiResponse<List<BatchDto>>.Ok(batches));
    }

    [HttpPost("batches")]
    [Authorize(Policy = Permissions.BatchesManage)]
    public async Task<ActionResult<ApiResponse<BatchDto>>> CreateBatch([FromBody] CreateBatchRequest request)
    {
        var batch = new Batch
        {
            Name = request.Name,
            GradeId = request.GradeId,
            BoardId = request.BoardId,
            AcademicYearId = request.AcademicYearId,
            Schedule = request.Schedule,
            IsActive = request.IsActive
        };

        _context.Batches.Add(batch);
        await _context.SaveChangesAsync();

        foreach (var sId in request.StudentIds)
        {
            _context.StudentBatches.Add(new StudentBatch
            {
                BatchId = batch.Id,
                StudentId = sId,
                EnrolledDate = DateTime.UtcNow
            });
        }
        await _context.SaveChangesAsync();

        var dto = new BatchDto
        {
            Id = batch.Id,
            Name = batch.Name,
            GradeId = batch.GradeId,
            BoardId = batch.BoardId,
            AcademicYearId = batch.AcademicYearId,
            Schedule = batch.Schedule,
            IsActive = batch.IsActive,
            StudentCount = request.StudentIds.Count,
            StudentIds = request.StudentIds
        };

        return Ok(ApiResponse<BatchDto>.Ok(dto, "Batch created successfully"));
    }

    [HttpPut("batches/{id}")]
    [Authorize(Policy = Permissions.BatchesManage)]
    public async Task<ActionResult<ApiResponse<bool>>> UpdateBatch(int id, [FromBody] CreateBatchRequest request)
    {
        var batch = await _context.Batches.Include(b => b.StudentBatches).FirstOrDefaultAsync(b => b.Id == id);
        if (batch == null) return NotFound();

        batch.Name = request.Name;
        batch.GradeId = request.GradeId;
        batch.BoardId = request.BoardId;
        batch.AcademicYearId = request.AcademicYearId;
        batch.Schedule = request.Schedule;
        batch.IsActive = request.IsActive;
        batch.ModifiedAt = DateTime.UtcNow;

        _context.StudentBatches.RemoveRange(batch.StudentBatches);
        foreach (var sId in request.StudentIds)
        {
            _context.StudentBatches.Add(new StudentBatch
            {
                BatchId = batch.Id,
                StudentId = sId,
                EnrolledDate = DateTime.UtcNow
            });
        }

        await _context.SaveChangesAsync();
        return Ok(ApiResponse<bool>.Ok(true, "Batch updated successfully"));
    }

    [HttpDelete("batches/{id}")]
    [Authorize(Policy = Permissions.BatchesManage)]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteBatch(int id)
    {
        var batch = await _context.Batches.FindAsync(id);
        if (batch == null) return NotFound();

        batch.IsDeleted = true;
        batch.ModifiedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return Ok(ApiResponse<bool>.Ok(true, "Batch deleted successfully"));
    }

    // --- Teachers ---
    [HttpGet("teachers")]
    public async Task<ActionResult<ApiResponse<List<TeacherDto>>>> GetTeachers()
    {
        var teachers = await _context.Teachers
            .Select(t => new TeacherDto
            {
                Id = t.Id,
                Name = t.Name,
                Phone = t.Phone,
                Email = t.Email,
                Specialization = t.Specialization,
                IsActive = t.IsActive
            })
            .ToListAsync();

        return Ok(ApiResponse<List<TeacherDto>>.Ok(teachers));
    }

    [HttpPost("teachers")]
    [Authorize(Policy = Permissions.TeachersManage)]
    public async Task<ActionResult<ApiResponse<TeacherDto>>> CreateTeacher([FromBody] TeacherDto dto)
    {
        var teacher = new Teacher
        {
            Name = dto.Name,
            Phone = dto.Phone,
            Email = dto.Email,
            Specialization = dto.Specialization,
            IsActive = dto.IsActive
        };

        _context.Teachers.Add(teacher);
        await _context.SaveChangesAsync();
        dto.Id = teacher.Id;
        return Ok(ApiResponse<TeacherDto>.Ok(dto, "Teacher registered successfully"));
    }

    [HttpPut("teachers/{id}")]
    [Authorize(Policy = Permissions.TeachersManage)]
    public async Task<ActionResult<ApiResponse<bool>>> UpdateTeacher(int id, [FromBody] TeacherDto dto)
    {
        var teacher = await _context.Teachers.FindAsync(id);
        if (teacher == null) return NotFound();

        teacher.Name = dto.Name;
        teacher.Phone = dto.Phone;
        teacher.Email = dto.Email;
        teacher.Specialization = dto.Specialization;
        teacher.IsActive = dto.IsActive;
        teacher.ModifiedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(ApiResponse<bool>.Ok(true, "Teacher updated successfully"));
    }

    [HttpDelete("teachers/{id}")]
    [Authorize(Policy = Permissions.TeachersManage)]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteTeacher(int id)
    {
        var teacher = await _context.Teachers.FindAsync(id);
        if (teacher == null) return NotFound();

        teacher.IsDeleted = true;
        teacher.ModifiedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return Ok(ApiResponse<bool>.Ok(true, "Teacher deleted successfully"));
    }
}
