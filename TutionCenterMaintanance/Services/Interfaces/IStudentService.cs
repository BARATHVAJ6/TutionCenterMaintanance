using TutionCenterMaintanance.Shared.DTOs.Students;
using TutionCenterMaintanance.Shared.Models;

namespace TutionCenterMaintanance.Services.Interfaces;

public interface IStudentService
{
    Task<PaginatedResult<StudentListItemDto>> GetStudentsAsync(
        string? search = null,
        int? gradeId = null,
        int? boardId = null,
        int? batchId = null,
        bool? isActive = true,
        int page = 1,
        int pageSize = 15,
        CancellationToken ct = default);

    Task<StudentDto?> GetStudentByIdAsync(int id, CancellationToken ct = default);

    Task<StudentDto> CreateStudentAsync(CreateStudentRequest request, string? currentUserId, CancellationToken ct = default);

    Task<bool> UpdateStudentAsync(int id, UpdateStudentRequest request, CancellationToken ct = default);

    Task<bool> DeleteStudentAsync(int id, CancellationToken ct = default);
}
