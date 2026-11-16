using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TutionCenterMaintanance.Data;
using TutionCenterMaintanance.Data.Entities;
using TutionCenterMaintanance.Shared.Constants;
using TutionCenterMaintanance.Shared.DTOs.Timetable;
using TutionCenterMaintanance.Shared.Models;

namespace TutionCenterMaintanance.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TimetableController : ControllerBase
{
    private readonly AppDbContext _context;

    public TimetableController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<TimetableSlotDto>>>> GetTimetable(
        [FromQuery] int? batchId = null,
        [FromQuery] int? teacherId = null,
        [FromQuery] DayOfWeek? dayOfWeek = null)
    {
        var query = _context.Timetables
            .Include(t => t.Batch).ThenInclude(b => b.Grade)
            .Include(t => t.Subject)
            .Include(t => t.Teacher)
            .AsQueryable();

        if (batchId.HasValue) query = query.Where(t => t.BatchId == batchId.Value);
        if (teacherId.HasValue) query = query.Where(t => t.TeacherId == teacherId.Value);
        if (dayOfWeek.HasValue) query = query.Where(t => t.DayOfWeek == dayOfWeek.Value);

        var list = await query
            .OrderBy(t => t.DayOfWeek)
            .ThenBy(t => t.StartTime)
            .Select(t => new TimetableSlotDto
            {
                Id = t.Id,
                BatchId = t.BatchId,
                BatchName = t.Batch.Name,
                GradeName = t.Batch.Grade.Name,
                SubjectId = t.SubjectId,
                SubjectName = t.Subject.Name,
                TeacherId = t.TeacherId,
                TeacherName = t.Teacher.Name,
                DayOfWeek = t.DayOfWeek,
                StartTime = t.StartTime,
                EndTime = t.EndTime
            })
            .ToListAsync();

        return Ok(ApiResponse<List<TimetableSlotDto>>.Ok(list));
    }

    [HttpPost]
    [Authorize(Policy = Permissions.TimetableManage)]
    public async Task<ActionResult<ApiResponse<TimetableSlotDto>>> CreateSlot([FromBody] CreateOrUpdateTimetableRequest request)
    {
        var batch = await _context.Batches.Include(b => b.Grade).FirstOrDefaultAsync(b => b.Id == request.BatchId);
        if (batch == null) return BadRequest(ApiResponse<TimetableSlotDto>.Fail("Batch not found"));

        var subject = await _context.Subjects.FindAsync(request.SubjectId);
        if (subject == null) return BadRequest(ApiResponse<TimetableSlotDto>.Fail("Subject not found"));

        var teacher = await _context.Teachers.FindAsync(request.TeacherId);
        if (teacher == null) return BadRequest(ApiResponse<TimetableSlotDto>.Fail("Teacher not found"));

        var slot = new Timetable
        {
            BatchId = request.BatchId,
            SubjectId = request.SubjectId,
            TeacherId = request.TeacherId,
            DayOfWeek = request.DayOfWeek,
            StartTime = request.StartTime,
            EndTime = request.EndTime
        };

        _context.Timetables.Add(slot);
        await _context.SaveChangesAsync();

        var dto = new TimetableSlotDto
        {
            Id = slot.Id,
            BatchId = slot.BatchId,
            BatchName = batch.Name,
            GradeName = batch.Grade.Name,
            SubjectId = slot.SubjectId,
            SubjectName = subject.Name,
            TeacherId = slot.TeacherId,
            TeacherName = teacher.Name,
            DayOfWeek = slot.DayOfWeek,
            StartTime = slot.StartTime,
            EndTime = slot.EndTime
        };

        return Ok(ApiResponse<TimetableSlotDto>.Ok(dto, "Timetable slot added successfully"));
    }

    [HttpPut("{id}")]
    [Authorize(Policy = Permissions.TimetableManage)]
    public async Task<ActionResult<ApiResponse<bool>>> UpdateSlot(int id, [FromBody] CreateOrUpdateTimetableRequest request)
    {
        var slot = await _context.Timetables.FindAsync(id);
        if (slot == null) return NotFound(ApiResponse<bool>.Fail("Slot not found"));

        slot.BatchId = request.BatchId;
        slot.SubjectId = request.SubjectId;
        slot.TeacherId = request.TeacherId;
        slot.DayOfWeek = request.DayOfWeek;
        slot.StartTime = request.StartTime;
        slot.EndTime = request.EndTime;
        slot.ModifiedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(ApiResponse<bool>.Ok(true, "Timetable slot updated successfully"));
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = Permissions.TimetableManage)]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteSlot(int id)
    {
        var slot = await _context.Timetables.FindAsync(id);
        if (slot == null) return NotFound(ApiResponse<bool>.Fail("Slot not found"));

        slot.IsDeleted = true;
        slot.ModifiedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return Ok(ApiResponse<bool>.Ok(true, "Timetable slot deleted"));
    }
}
