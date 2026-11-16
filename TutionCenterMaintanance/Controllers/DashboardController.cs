using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TutionCenterMaintanance.Data;
using TutionCenterMaintanance.Data.Entities;
using TutionCenterMaintanance.Shared.Constants;
using TutionCenterMaintanance.Shared.DTOs.Dashboard;
using TutionCenterMaintanance.Shared.DTOs.Settings;
using TutionCenterMaintanance.Shared.Enums;
using TutionCenterMaintanance.Shared.Models;

namespace TutionCenterMaintanance.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SettingsController : ControllerBase
{
    private readonly AppDbContext _context;

    public SettingsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<CenterSettingsDto>>> GetSettings()
    {
        var settings = await _context.CenterSettings.FirstOrDefaultAsync();
        if (settings == null)
        {
            settings = new CenterSettings();
            _context.CenterSettings.Add(settings);
            await _context.SaveChangesAsync();
        }

        var dto = new CenterSettingsDto
        {
            Id = settings.Id,
            CenterName = settings.CenterName,
            Tagline = settings.Tagline,
            LogoBase64 = settings.LogoBase64,
            Address = settings.Address,
            Phone = settings.Phone,
            Email = settings.Email,
            Website = settings.Website,
            CurrencySymbol = settings.CurrencySymbol
        };

        return Ok(ApiResponse<CenterSettingsDto>.Ok(dto));
    }

    [HttpPut]
    [Authorize(Policy = Permissions.SettingsManage)]
    public async Task<ActionResult<ApiResponse<bool>>> UpdateSettings([FromBody] CenterSettingsDto dto)
    {
        var settings = await _context.CenterSettings.FirstOrDefaultAsync();
        if (settings == null)
        {
            settings = new CenterSettings();
            _context.CenterSettings.Add(settings);
        }

        settings.CenterName = dto.CenterName;
        settings.Tagline = dto.Tagline;
        settings.LogoBase64 = dto.LogoBase64;
        settings.Address = dto.Address;
        settings.Phone = dto.Phone;
        settings.Email = dto.Email;
        settings.Website = dto.Website;
        settings.CurrencySymbol = dto.CurrencySymbol;
        settings.ModifiedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(ApiResponse<bool>.Ok(true, "Settings saved successfully"));
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly AppDbContext _context;

    public DashboardController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("stats")]
    public async Task<ActionResult<ApiResponse<DashboardStatsDto>>> GetStats()
    {
        var totalStudents = await _context.Students.CountAsync(s => s.IsActive);
        var totalTeachers = await _context.Teachers.CountAsync(t => t.IsActive);
        var totalBatches = await _context.Batches.CountAsync(b => b.IsActive);

        var today = DateTime.Today;
        var todayAttendances = await _context.Attendances
            .Where(a => a.Date.Date == today)
            .ToListAsync();

        var presentCount = todayAttendances.Count(a => a.Status == AttendanceStatus.Present);
        var absentCount = todayAttendances.Count(a => a.Status == AttendanceStatus.Absent);
        decimal attPct = totalStudents > 0 && todayAttendances.Any()
            ? Math.Round(((decimal)presentCount / totalStudents) * 100, 1)
            : 0;

        var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
        var monthPayments = await _context.Payments
            .Where(p => p.PaymentDate >= firstDayOfMonth)
            .SumAsync(p => (decimal?)p.Amount) ?? 0;

        var upcomingAssessments = await _context.Assessments
            .Include(a => a.Grade)
            .Include(a => a.AssessmentSubjects)
            .Where(a => a.Date >= today)
            .OrderBy(a => a.Date)
            .Take(5)
            .Select(a => new UpcomingAssessmentDto
            {
                Id = a.Id,
                Name = a.Name,
                GradeName = a.Grade.Name,
                Date = a.Date,
                SubjectCount = a.AssessmentSubjects.Count
            })
            .ToListAsync();

        var stats = new DashboardStatsDto
        {
            TotalActiveStudents = totalStudents,
            TotalTeachers = totalTeachers,
            TotalBatches = totalBatches,
            TodayAttendancePercentage = attPct,
            TodayPresentCount = presentCount,
            TodayAbsentCount = absentCount,
            TotalFeesCollectedMonth = monthPayments,
            UpcomingAssessments = upcomingAssessments
        };

        return Ok(ApiResponse<DashboardStatsDto>.Ok(stats));
    }
}
