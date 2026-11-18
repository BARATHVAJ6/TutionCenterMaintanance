using Microsoft.EntityFrameworkCore;
using TutionCenterMaintanance.Data;
using TutionCenterMaintanance.Data.Entities;
using TutionCenterMaintanance.Services.Interfaces;
using TutionCenterMaintanance.Shared.DTOs.Students;
using TutionCenterMaintanance.Shared.Models;

namespace TutionCenterMaintanance.Services;

public class StudentService : IStudentService
{
    private readonly AppDbContext _context;

    public StudentService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedResult<StudentListItemDto>> GetStudentsAsync(
        string? search = null,
        int? gradeId = null,
        int? boardId = null,
        int? batchId = null,
        bool? isActive = true,
        int page = 1,
        int pageSize = 15,
        CancellationToken ct = default)
    {
        pageSize = Math.Clamp(pageSize, 1, 100);
        page = Math.Max(1, page);

        var query = _context.Students
            .AsNoTracking()
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
            var s = search.Trim();
            var pattern = $"%{s}%";
            query = query.Where(st =>
                EF.Functions.Like(st.FirstName, pattern) ||
                EF.Functions.Like(st.LastName, pattern) ||
                EF.Functions.Like(st.SchoolName, pattern) ||
                (st.Parent != null && (
                    EF.Functions.Like(st.Parent.FatherName, pattern) ||
                    EF.Functions.Like(st.Parent.FatherPhone, pattern) ||
                    (st.Parent.MotherName != null && EF.Functions.Like(st.Parent.MotherName, pattern)) ||
                    (st.Parent.MotherPhone != null && EF.Functions.Like(st.Parent.MotherPhone, pattern))
                ))
            );
        }

        var totalCount = await query.CountAsync(ct);

        var items = await query
            .OrderBy(s => s.FirstName)
            .ThenBy(s => s.LastName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(s => new StudentListItemDto
            {
                Id = s.Id,
                FullName = (s.FirstName + " " + s.LastName).Trim(),
                GradeName = s.Grade != null ? s.Grade.Name : string.Empty,
                BoardName = s.Board != null ? s.Board.Name : string.Empty,
                SchoolName = s.SchoolName,
                FatherName = s.Parent != null ? s.Parent.FatherName : string.Empty,
                PrimaryPhone = s.Parent != null
                    ? (!string.IsNullOrWhiteSpace(s.Parent.FatherPhone) ? s.Parent.FatherPhone : s.Parent.MotherPhone ?? string.Empty)
                    : string.Empty,
                WhatsAppPhone = s.Parent != null
                    ? (!string.IsNullOrWhiteSpace(s.Parent.FatherWhatsApp) ? s.Parent.FatherWhatsApp : s.Parent.MotherWhatsApp)
                    : null,
                IsActive = s.IsActive,
                PhotoUrl = s.PhotoUrl,
                MonthlyFeeAmount = s.MonthlyFeeAmount,
                BatchNames = s.StudentBatches.Select(sb => sb.Batch.Name).ToList()
            })
            .ToListAsync(ct);

        return new PaginatedResult<StudentListItemDto>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = page,
            PageSize = pageSize
        };
    }

    public async Task<StudentDto?> GetStudentByIdAsync(int id, CancellationToken ct = default)
    {
        var student = await _context.Students
            .AsNoTracking()
            .Include(s => s.Grade)
            .Include(s => s.Board)
            .Include(s => s.Parent)
            .Include(s => s.StudentBatches)
                .ThenInclude(sb => sb.Batch)
            .FirstOrDefaultAsync(s => s.Id == id, ct);

        if (student == null) return null;

        return new StudentDto
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName,
            DateOfBirth = student.DateOfBirth,
            Gender = student.Gender,
            PhotoUrl = student.PhotoUrl,
            BoardId = student.BoardId,
            BoardName = student.Board?.Name ?? string.Empty,
            GradeId = student.GradeId,
            GradeName = student.Grade?.Name ?? string.Empty,
            SchoolName = student.SchoolName,
            Address = student.Address,
            AdmissionDate = student.AdmissionDate,
            BloodGroup = student.BloodGroup,
            MedicalNotes = student.MedicalNotes,
            PreviousRecords = student.PreviousRecords,
            MonthlyFeeAmount = student.MonthlyFeeAmount,
            IsActive = student.IsActive,
            Parent = student.Parent != null ? new ParentInfoDto
            {
                FatherName = student.Parent.FatherName,
                FatherPhone = student.Parent.FatherPhone,
                FatherWhatsApp = student.Parent.FatherWhatsApp,
                MotherName = student.Parent.MotherName,
                MotherPhone = student.Parent.MotherPhone,
                MotherWhatsApp = student.Parent.MotherWhatsApp,
                EmergencyContactName = student.Parent.EmergencyContactName,
                EmergencyContactPhone = student.Parent.EmergencyContactPhone
            } : new ParentInfoDto(),
            BatchIds = student.StudentBatches.Select(sb => sb.BatchId).ToList(),
            BatchNames = student.StudentBatches.Select(sb => sb.Batch.Name).ToList()
        };
    }

    public async Task<StudentDto> CreateStudentAsync(CreateStudentRequest request, string? currentUserId, CancellationToken ct = default)
    {
        var student = new Student
        {
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            DateOfBirth = request.DateOfBirth,
            Gender = request.Gender,
            PhotoUrl = request.PhotoBase64,
            BoardId = request.BoardId,
            GradeId = request.GradeId,
            SchoolName = request.SchoolName.Trim(),
            Address = request.Address?.Trim(),
            AdmissionDate = request.AdmissionDate != default ? request.AdmissionDate : DateTime.Today,
            BloodGroup = request.BloodGroup?.Trim(),
            MedicalNotes = request.MedicalNotes?.Trim(),
            PreviousRecords = request.PreviousRecords?.Trim(),
            MonthlyFeeAmount = request.MonthlyFeeAmount,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        if (request.Parent != null)
        {
            student.Parent = new ParentInfo
            {
                FatherName = request.Parent.FatherName?.Trim() ?? string.Empty,
                FatherPhone = request.Parent.FatherPhone?.Trim() ?? string.Empty,
                FatherWhatsApp = request.Parent.FatherWhatsApp?.Trim(),
                MotherName = request.Parent.MotherName?.Trim(),
                MotherPhone = request.Parent.MotherPhone?.Trim(),
                MotherWhatsApp = request.Parent.MotherWhatsApp?.Trim(),
                EmergencyContactName = request.Parent.EmergencyContactName?.Trim(),
                EmergencyContactPhone = request.Parent.EmergencyContactPhone?.Trim(),
                CreatedAt = DateTime.UtcNow
            };
        }

        if (request.BatchIds.Any())
        {
            student.StudentBatches = request.BatchIds.Select(bId => new StudentBatch
            {
                BatchId = bId,
                EnrolledDate = DateTime.UtcNow
            }).ToList();
        }

        _context.Students.Add(student);
        await _context.SaveChangesAsync(ct);

        return (await GetStudentByIdAsync(student.Id, ct))!;
    }

    public async Task<bool> UpdateStudentAsync(int id, UpdateStudentRequest request, CancellationToken ct = default)
    {
        var student = await _context.Students
            .Include(s => s.Parent)
            .Include(s => s.StudentBatches)
            .FirstOrDefaultAsync(s => s.Id == id, ct);

        if (student == null) return false;

        student.FirstName = request.FirstName.Trim();
        student.LastName = request.LastName.Trim();
        student.DateOfBirth = request.DateOfBirth;
        student.Gender = request.Gender;
        if (!string.IsNullOrWhiteSpace(request.PhotoBase64))
        {
            student.PhotoUrl = request.PhotoBase64;
        }
        student.BoardId = request.BoardId;
        student.GradeId = request.GradeId;
        student.SchoolName = request.SchoolName.Trim();
        student.Address = request.Address?.Trim();
        student.AdmissionDate = request.AdmissionDate;
        student.BloodGroup = request.BloodGroup?.Trim();
        student.MedicalNotes = request.MedicalNotes?.Trim();
        student.PreviousRecords = request.PreviousRecords?.Trim();
        student.MonthlyFeeAmount = request.MonthlyFeeAmount;
        student.IsActive = request.IsActive;
        student.ModifiedAt = DateTime.UtcNow;

        if (request.Parent != null)
        {
            if (student.Parent == null)
            {
                student.Parent = new ParentInfo
                {
                    StudentId = student.Id,
                    CreatedAt = DateTime.UtcNow
                };
            }

            student.Parent.FatherName = request.Parent.FatherName?.Trim() ?? string.Empty;
            student.Parent.FatherPhone = request.Parent.FatherPhone?.Trim() ?? string.Empty;
            student.Parent.FatherWhatsApp = request.Parent.FatherWhatsApp?.Trim();
            student.Parent.MotherName = request.Parent.MotherName?.Trim();
            student.Parent.MotherPhone = request.Parent.MotherPhone?.Trim();
            student.Parent.MotherWhatsApp = request.Parent.MotherWhatsApp?.Trim();
            student.Parent.EmergencyContactName = request.Parent.EmergencyContactName?.Trim();
            student.Parent.EmergencyContactPhone = request.Parent.EmergencyContactPhone?.Trim();
            student.Parent.ModifiedAt = DateTime.UtcNow;
        }

        // Sync batches
        _context.StudentBatches.RemoveRange(student.StudentBatches);
        if (request.BatchIds.Any())
        {
            student.StudentBatches = request.BatchIds.Select(bId => new StudentBatch
            {
                StudentId = student.Id,
                BatchId = bId,
                EnrolledDate = DateTime.UtcNow
            }).ToList();
        }

        await _context.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteStudentAsync(int id, CancellationToken ct = default)
    {
        var student = await _context.Students.FindAsync(new object[] { id }, ct);
        if (student == null) return false;

        student.IsDeleted = true;
        student.IsActive = false;
        student.ModifiedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(ct);
        return true;
    }
}
