using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TutionCenterMaintanance.Services.Interfaces;
using TutionCenterMaintanance.Shared.Constants;
using TutionCenterMaintanance.Shared.DTOs.Students;
using TutionCenterMaintanance.Shared.Models;

namespace TutionCenterMaintanance.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
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
        [FromQuery] int pageSize = 15,
        CancellationToken ct = default)
    {
        var result = await _studentService.GetStudentsAsync(search, gradeId, boardId, batchId, isActive, page, pageSize, ct);
        return Ok(ApiResponse<PaginatedResult<StudentListItemDto>>.Ok(result));
    }

    [HttpGet("{id}")]
    [Authorize(Policy = Permissions.StudentsView)]
    public async Task<ActionResult<ApiResponse<StudentDto>>> GetStudentById(int id, CancellationToken ct = default)
    {
        var student = await _studentService.GetStudentByIdAsync(id, ct);
        if (student == null)
            return NotFound(ApiResponse<StudentDto>.Fail("Student not found"));

        return Ok(ApiResponse<StudentDto>.Ok(student));
    }

    [HttpPost]
    [Authorize(Policy = Permissions.StudentsCreate)]
    public async Task<ActionResult<ApiResponse<StudentDto>>> CreateStudent(
        [FromBody] CreateStudentRequest request,
        CancellationToken ct = default)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<StudentDto>.Fail("Invalid student information"));

        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var created = await _studentService.CreateStudentAsync(request, currentUserId, ct);

        return CreatedAtAction(nameof(GetStudentById), new { id = created.Id }, ApiResponse<StudentDto>.Ok(created, "Student registered successfully"));
    }

    [HttpPut("{id}")]
    [Authorize(Policy = Permissions.StudentsEdit)]
    public async Task<ActionResult<ApiResponse<bool>>> UpdateStudent(
        int id,
        [FromBody] UpdateStudentRequest request,
        CancellationToken ct = default)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<bool>.Fail("Invalid student information"));

        var updated = await _studentService.UpdateStudentAsync(id, request, ct);
        if (!updated)
            return NotFound(ApiResponse<bool>.Fail("Student not found"));

        return Ok(ApiResponse<bool>.Ok(true, "Student updated successfully"));
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = Permissions.StudentsDelete)]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteStudent(int id, CancellationToken ct = default)
    {
        var deleted = await _studentService.DeleteStudentAsync(id, ct);
        if (!deleted)
            return NotFound(ApiResponse<bool>.Fail("Student not found"));

        return Ok(ApiResponse<bool>.Ok(true, "Student archived successfully"));
    }
}
