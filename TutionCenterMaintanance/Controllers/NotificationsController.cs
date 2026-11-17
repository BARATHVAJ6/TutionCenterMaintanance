using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TutionCenterMaintanance.Data;
using TutionCenterMaintanance.Data.Entities;
using TutionCenterMaintanance.Shared.Constants;
using TutionCenterMaintanance.Shared.DTOs.Notifications;
using TutionCenterMaintanance.Shared.Enums;
using TutionCenterMaintanance.Shared.Models;

namespace TutionCenterMaintanance.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly AppDbContext _context;

    public NotificationsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<NotificationDto>>>> GetNotifications([FromQuery] bool? unreadOnly = null)
    {
        var query = _context.Notifications.AsQueryable();

        if (unreadOnly == true)
        {
            query = query.Where(n => !n.IsRead);
        }

        var list = await query
            .OrderByDescending(n => n.CreatedAt)
            .Take(50)
            .Select(n => new NotificationDto
            {
                Id = n.Id,
                Title = n.Title,
                Message = n.Message,
                Type = n.Type,
                IsRead = n.IsRead,
                CreatedAt = n.CreatedAt
            })
            .ToListAsync();

        return Ok(ApiResponse<List<NotificationDto>>.Ok(list));
    }

    [HttpGet("unread-count")]
    public async Task<ActionResult<ApiResponse<NotificationCountDto>>> GetUnreadCount()
    {
        var count = await _context.Notifications.CountAsync(n => !n.IsRead);
        return Ok(ApiResponse<NotificationCountDto>.Ok(new NotificationCountDto { UnreadCount = count }));
    }

    [HttpPut("{id}/read")]
    public async Task<ActionResult<ApiResponse<bool>>> MarkAsRead(int id)
    {
        var n = await _context.Notifications.FindAsync(id);
        if (n == null) return NotFound();

        n.IsRead = true;
        n.ModifiedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return Ok(ApiResponse<bool>.Ok(true, "Notification marked as read"));
    }

    [HttpPost("read-all")]
    public async Task<ActionResult<ApiResponse<bool>>> MarkAllAsRead()
    {
        var unread = await _context.Notifications.Where(n => !n.IsRead).ToListAsync();
        foreach (var n in unread)
        {
            n.IsRead = true;
            n.ModifiedAt = DateTime.UtcNow;
        }
        await _context.SaveChangesAsync();

        return Ok(ApiResponse<bool>.Ok(true, "All notifications marked as read"));
    }

    [HttpPost("generate-system-alerts")]
    public async Task<ActionResult<ApiResponse<int>>> GenerateSystemAlerts()
    {
        int alertsCreated = 0;
        var today = DateTime.Today;

        // 1. Check for Pending Fees this month
        var startMonth = new DateTime(today.Year, today.Month, 1);
        var endMonth = startMonth.AddMonths(1).AddTicks(-1);
        var activeStudents = await _context.Students
            .Include(s => s.Payments.Where(p => p.PaymentDate >= startMonth && p.PaymentDate <= endMonth))
            .Where(s => s.IsActive && s.MonthlyFeeAmount > 0)
            .ToListAsync();

        var pendingCount = activeStudents.Count(s => s.Payments.Sum(p => p.Amount) < s.MonthlyFeeAmount);

        if (pendingCount > 0)
        {
            var alreadyNotifiedToday = await _context.Notifications
                .AnyAsync(n => n.Type == "Warning" && n.Title.Contains("Fee") && n.CreatedAt.Date == today);

            if (!alreadyNotifiedToday)
            {
                _context.Notifications.Add(new Notification
                {
                    Title = "Monthly Fee Collection Alert",
                    Message = $"{pendingCount} student(s) have pending fees for this month.",
                    Type = "Warning",
                    CreatedAt = DateTime.UtcNow
                });
                alertsCreated++;
            }
        }

        // 2. Check for Upcoming Assessments (next 48 hours)
        var upcomingTests = await _context.Assessments
            .Include(a => a.Grade)
            .Where(a => a.Date.Date >= today && a.Date.Date <= today.AddDays(2))
            .ToListAsync();

        foreach (var test in upcomingTests)
        {
            var testAlertExists = await _context.Notifications
                .AnyAsync(n => n.Title.Contains(test.Name) && n.CreatedAt.Date == today);

            if (!testAlertExists)
            {
                _context.Notifications.Add(new Notification
                {
                    Title = $"Upcoming Test: {test.Name}",
                    Message = $"Scheduled for {test.Grade.Name} on {test.Date:dd MMM yyyy}.",
                    Type = "Info",
                    CreatedAt = DateTime.UtcNow
                });
                alertsCreated++;
            }
        }

        // 3. Check Today's Attendance Absentees
        var absenteesCount = await _context.Attendances
            .CountAsync(a => a.Date.Date == today && a.Status == AttendanceStatus.Absent);

        if (absenteesCount > 0)
        {
            var attAlertExists = await _context.Notifications
                .AnyAsync(n => n.Title.Contains("Daily Attendance") && n.CreatedAt.Date == today);

            if (!attAlertExists)
            {
                _context.Notifications.Add(new Notification
                {
                    Title = "Daily Attendance Absentees",
                    Message = $"{absenteesCount} student(s) marked absent today. Consider sending absence notices to parents.",
                    Type = "Warning",
                    CreatedAt = DateTime.UtcNow
                });
                alertsCreated++;
            }
        }

        if (alertsCreated > 0)
        {
            await _context.SaveChangesAsync();
        }

        return Ok(ApiResponse<int>.Ok(alertsCreated, $"{alertsCreated} alert(s) generated"));
    }
}
