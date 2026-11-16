using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TutionCenterMaintanance.Data;
using TutionCenterMaintanance.Data.Entities;
using TutionCenterMaintanance.Shared.Constants;
using TutionCenterMaintanance.Shared.DTOs.Attendance;
using TutionCenterMaintanance.Shared.Enums;
using TutionCenterMaintanance.Shared.Models;

namespace TutionCenterMaintanance.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AttendanceController : ControllerBase
{
    private readonly AppDbContext _context;

    public AttendanceController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("sheet")]
    [Authorize(Policy = Permissions.AttendanceView)]
    public async Task<ActionResult<ApiResponse<BatchAttendanceSheetDto>>> GetAttendanceSheet(
        [FromQuery] DateTime date,
        [FromQuery] int? batchId = null,
        [FromQuery] int? gradeId = null)
    {
        var targetDate = date.Date;

        var studentsQuery = _context.Students
            .Include(s => s.Grade)
            .Where(s => s.IsActive);

        if (batchId.HasValue)
        {
            studentsQuery = studentsQuery.Where(s => s.StudentBatches.Any(sb => sb.BatchId == batchId.Value));
        }
        else if (gradeId.HasValue)
        {
            studentsQuery = studentsQuery.Where(s => s.GradeId == gradeId.Value);
        }

        var students = await studentsQuery
            .OrderBy(s => s.FirstName)
            .ThenBy(s => s.LastName)
            .ToListAsync();

        var studentIds = students.Select(s => s.Id).ToList();

        var existingAttendances = await _context.Attendances
            .Where(a => studentIds.Contains(a.StudentId) && a.Date.Date == targetDate)
            .ToDictionaryAsync(a => a.StudentId);

        var sheet = new BatchAttendanceSheetDto
        {
            BatchId = batchId,
            GradeId = gradeId,
            Date = targetDate,
            Students = students.Select(st =>
            {
                existingAttendances.TryGetValue(st.Id, out var att);
                return new DailyAttendanceItemDto
                {
                    StudentId = st.Id,
                    StudentName = $"{st.FirstName} {st.LastName}".Trim(),
                    GradeName = st.Grade.Name,
                    Status = att?.Status ?? AttendanceStatus.Present,
                    Remarks = att?.Remarks
                };
            }).ToList()
        };

        return Ok(ApiResponse<BatchAttendanceSheetDto>.Ok(sheet));
    }

    [HttpPost("save")]
    [Authorize(Policy = Permissions.AttendanceMark)]
    public async Task<ActionResult<ApiResponse<bool>>> SaveAttendance([FromBody] SaveDailyAttendanceRequest request)
    {
        var targetDate = request.Date.Date;
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var studentIds = request.AttendanceList.Select(a => a.StudentId).ToList();

        var existing = await _context.Attendances
            .Where(a => studentIds.Contains(a.StudentId) && a.Date.Date == targetDate)
            .ToListAsync();

        var existingMap = existing.ToDictionary(a => a.StudentId);

        foreach (var item in request.AttendanceList)
        {
            if (existingMap.TryGetValue(item.StudentId, out var att))
            {
                att.Status = item.Status;
                att.Remarks = item.Remarks;
                att.BatchId = request.BatchId;
                att.MarkedByUserId = currentUserId;
                att.ModifiedAt = DateTime.UtcNow;
            }
            else
            {
                _context.Attendances.Add(new Attendance
                {
                    StudentId = item.StudentId,
                    BatchId = request.BatchId,
                    Date = targetDate,
                    Status = item.Status,
                    Remarks = item.Remarks,
                    MarkedByUserId = currentUserId
                });
            }
        }

        await _context.SaveChangesAsync();
        return Ok(ApiResponse<bool>.Ok(true, "Attendance recorded successfully"));
    }

    [HttpGet("summary")]
    [Authorize(Policy = Permissions.AttendanceView)]
    public async Task<ActionResult<ApiResponse<AttendanceSummaryDto>>> GetAttendanceSummary([FromQuery] DateTime? date)
    {
        var targetDate = (date ?? DateTime.Today).Date;

        var totalActiveStudents = await _context.Students.CountAsync(s => s.IsActive);
        var attendances = await _context.Attendances
            .Where(a => a.Date.Date == targetDate)
            .ToListAsync();

        var presentCount = attendances.Count(a => a.Status == AttendanceStatus.Present);
        var absentCount = attendances.Count(a => a.Status == AttendanceStatus.Absent);

        var summary = new AttendanceSummaryDto
        {
            TotalStudents = totalActiveStudents,
            PresentCount = presentCount,
            AbsentCount = absentCount
        };

        return Ok(ApiResponse<AttendanceSummaryDto>.Ok(summary));
    }
}
