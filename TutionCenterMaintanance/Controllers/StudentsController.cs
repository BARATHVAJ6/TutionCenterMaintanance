using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TutionCenterMaintanance.Data;
using TutionCenterMaintanance.Data.Entities;
using TutionCenterMaintanance.Shared.Constants;
using TutionCenterMaintanance.Shared.DTOs.Students;
using TutionCenterMaintanance.Shared.Models;

namespace TutionCenterMaintanance.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StudentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public StudentsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Authorize(Policy = Permissions.StudentsView)]
    public async Task<ActionResult<ApiResponse<PaginatedResult<StudentListItemDto>>>> GetStudents(
        [FromQuery] string? search = null,
        [FromQuery] int? gradeId = null,
        [FromQuery] int? boardId = null,
        [FromQuery] int? batchId = null,
        [FromQuery] bool? isActive = true,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 15)
    {
        var query = _context.Students
            .Include(s => s.Grade)
            .Include(s => s.Board)
            .Include(s => s.Parent)
            .Include(s => s.StudentBatches)
                .ThenInclude(sb => sb.Batch)
            .AsQueryable();

        if (isActive.HasValue)
        {
            query = query.Where(s => s.IsActive == isActive.Value);
        }

        if (gradeId.HasValue)
        {
            query = query.Where(s => s.GradeId == gradeId.Value);
        }

        if (boardId.HasValue)
        {
            query = query.Where(s => s.BoardId == boardId.Value);
        }

        if (batchId.HasValue)
        {
            query = query.Where(s => s.StudentBatches.Any(sb => sb.BatchId == batchId.Value));
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim().ToLower();
            query = query.Where(st =>
                st.FirstName.ToLower().Contains(s) ||
                st.LastName.ToLower().Contains(s) ||
                st.SchoolName.ToLower().Contains(s) ||
                st.Parent.FatherName.ToLower().Contains(s) ||
                st.Parent.FatherPhone.Contains(s) ||
                (st.Parent.MotherPhone != null && st.Parent.MotherPhone.Contains(s))
            );
        }

        var total = await query.CountAsync();

        var items = await query
            .OrderBy(s => s.FirstName)
            .ThenBy(s => s.LastName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(s => new StudentListItemDto
            {
                Id = s.Id,
                FullName = $"{s.FirstName} {s.LastName}".Trim(),
                GradeName = s.Grade.Name,
                BoardName = s.Board.Name,
                SchoolName = s.SchoolName,
                FatherName = s.Parent.FatherName,
                PrimaryPhone = s.Parent.FatherPhone,
                WhatsAppPhone = !string.IsNullOrEmpty(s.Parent.FatherWhatsApp) ? s.Parent.FatherWhatsApp : s.Parent.FatherPhone,
                IsActive = s.IsActive,
                PhotoUrl = s.PhotoUrl,
                BatchNames = s.StudentBatches.Select(sb => sb.Batch.Name).ToList()
            })
            .ToListAsync();

        var result = new PaginatedResult<StudentListItemDto>
        {
            Items = items,
            TotalCount = total,
            PageNumber = page,
            PageSize = pageSize
        };

        return Ok(ApiResponse<PaginatedResult<StudentListItemDto>>.Ok(result));
    }

    [HttpGet("{id}")]
    [Authorize(Policy = Permissions.StudentsView)]
    public async Task<ActionResult<ApiResponse<StudentDto>>> GetStudentById(int id)
    {
        var s = await _context.Students
            .Include(st => st.Grade)
            .Include(st => st.Board)
            .Include(st => st.Parent)
            .Include(st => st.StudentBatches)
                .ThenInclude(sb => sb.Batch)
            .FirstOrDefaultAsync(st => st.Id == id);

        if (s == null) return NotFound();

        var dto = new StudentDto
        {
            Id = s.Id,
            FirstName = s.FirstName,
            LastName = s.LastName,
            DateOfBirth = s.DateOfBirth,
            Gender = s.Gender,
            PhotoUrl = s.PhotoUrl,
            BoardId = s.BoardId,
            BoardName = s.Board.Name,
            GradeId = s.GradeId,
            GradeName = s.Grade.Name,
            SchoolName = s.SchoolName,
            Address = s.Address,
            AdmissionDate = s.AdmissionDate,
            BloodGroup = s.BloodGroup,
            MedicalNotes = s.MedicalNotes,
            PreviousRecords = s.PreviousRecords,
            IsActive = s.IsActive,
            BatchIds = s.StudentBatches.Select(sb => sb.BatchId).ToList(),
            BatchNames = s.StudentBatches.Select(sb => sb.Batch.Name).ToList(),
            Parent = new ParentInfoDto
            {
                FatherName = s.Parent.FatherName,
                FatherPhone = s.Parent.FatherPhone,
                FatherWhatsApp = s.Parent.FatherWhatsApp,
                MotherName = s.Parent.MotherName,
                MotherPhone = s.Parent.MotherPhone,
                MotherWhatsApp = s.Parent.MotherWhatsApp,
                EmergencyContactName = s.Parent.EmergencyContactName,
                EmergencyContactPhone = s.Parent.EmergencyContactPhone
            }
        };

        return Ok(ApiResponse<StudentDto>.Ok(dto));
    }

    [HttpPost]
    [Authorize(Policy = Permissions.StudentsCreate)]
    public async Task<ActionResult<ApiResponse<StudentDto>>> CreateStudent([FromBody] CreateStudentRequest request)
    {
        var student = new Student
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            DateOfBirth = request.DateOfBirth,
            Gender = request.Gender,
            BoardId = request.BoardId,
            GradeId = request.GradeId,
            SchoolName = request.SchoolName,
            Address = request.Address,
            AdmissionDate = request.AdmissionDate,
            BloodGroup = request.BloodGroup,
            MedicalNotes = request.MedicalNotes,
            PreviousRecords = request.PreviousRecords,
            PhotoUrl = request.PhotoBase64,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            Parent = new ParentInfo
            {
                FatherName = request.Parent.FatherName,
                FatherPhone = request.Parent.FatherPhone,
                FatherWhatsApp = request.Parent.FatherWhatsApp,
                MotherName = request.Parent.MotherName,
                MotherPhone = request.Parent.MotherPhone,
                MotherWhatsApp = request.Parent.MotherWhatsApp,
                EmergencyContactName = request.Parent.EmergencyContactName,
                EmergencyContactPhone = request.Parent.EmergencyContactPhone
            }
        };

        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        if (request.BatchIds.Any())
        {
            foreach (var bId in request.BatchIds)
            {
                _context.StudentBatches.Add(new StudentBatch
                {
                    StudentId = student.Id,
                    BatchId = bId,
                    EnrolledDate = DateTime.UtcNow
                });
            }
            await _context.SaveChangesAsync();
        }

        return await GetStudentById(student.Id);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = Permissions.StudentsEdit)]
    public async Task<ActionResult<ApiResponse<bool>>> UpdateStudent(int id, [FromBody] UpdateStudentRequest request)
    {
        var student = await _context.Students
            .Include(s => s.Parent)
            .Include(s => s.StudentBatches)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (student == null) return NotFound();

        student.FirstName = request.FirstName;
        student.LastName = request.LastName;
        student.DateOfBirth = request.DateOfBirth;
        student.Gender = request.Gender;
        student.BoardId = request.BoardId;
        student.GradeId = request.GradeId;
        student.SchoolName = request.SchoolName;
        student.Address = request.Address;
        student.AdmissionDate = request.AdmissionDate;
        student.BloodGroup = request.BloodGroup;
        student.MedicalNotes = request.MedicalNotes;
        student.PreviousRecords = request.PreviousRecords;
        student.IsActive = request.IsActive;
        student.ModifiedAt = DateTime.UtcNow;

        if (!string.IsNullOrEmpty(request.PhotoBase64))
        {
            student.PhotoUrl = request.PhotoBase64;
        }

        // Update Parent info
        student.Parent.FatherName = request.Parent.FatherName;
        student.Parent.FatherPhone = request.Parent.FatherPhone;
        student.Parent.FatherWhatsApp = request.Parent.FatherWhatsApp;
        student.Parent.MotherName = request.Parent.MotherName;
        student.Parent.MotherPhone = request.Parent.MotherPhone;
        student.Parent.MotherWhatsApp = request.Parent.MotherWhatsApp;
        student.Parent.EmergencyContactName = request.Parent.EmergencyContactName;
        student.Parent.EmergencyContactPhone = request.Parent.EmergencyContactPhone;
        student.Parent.ModifiedAt = DateTime.UtcNow;

        // Update Batches
        _context.StudentBatches.RemoveRange(student.StudentBatches);
        foreach (var bId in request.BatchIds)
        {
            _context.StudentBatches.Add(new StudentBatch
            {
                StudentId = student.Id,
                BatchId = bId,
                EnrolledDate = DateTime.UtcNow
            });
        }

        await _context.SaveChangesAsync();
        return Ok(ApiResponse<bool>.Ok(true, "Student record updated successfully"));
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = Permissions.StudentsDelete)]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteStudent(int id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null) return NotFound();

        student.IsDeleted = true;
        student.IsActive = false;
        student.ModifiedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return Ok(ApiResponse<bool>.Ok(true, "Student record deactivated (soft deleted)"));
    }
}
