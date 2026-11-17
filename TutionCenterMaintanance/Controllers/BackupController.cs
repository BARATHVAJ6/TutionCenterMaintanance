using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;
using TutionCenterMaintanance.Data;
using TutionCenterMaintanance.Shared.Constants;
using TutionCenterMaintanance.Shared.DTOs.Audit;
using TutionCenterMaintanance.Shared.Models;

namespace TutionCenterMaintanance.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = Permissions.BackupManage)]
public class BackupController : ControllerBase
{
    private readonly AppDbContext _context;

    public BackupController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("summary")]
    public async Task<ActionResult<ApiResponse<SystemBackupBundleDto>>> GetBackupSummary()
    {
        var center = await _context.CenterSettings.FirstOrDefaultAsync();
        var studentCount = await _context.Students.CountAsync();
        var paymentCount = await _context.Payments.CountAsync();
        var marksCount = await _context.Marks.CountAsync();

        var bundle = new SystemBackupBundleDto
        {
            CenterName = center?.CenterName ?? "Sai Tuition Center",
            TotalStudentsCount = studentCount,
            TotalPaymentsCount = paymentCount,
            TotalMarksCount = marksCount,
            CreatedAt = DateTime.UtcNow
        };

        return Ok(ApiResponse<SystemBackupBundleDto>.Ok(bundle));
    }

    [HttpGet("download")]
    public async Task<IActionResult> DownloadJsonBackup()
    {
        var center = await _context.CenterSettings.FirstOrDefaultAsync();
        var students = await _context.Students.Include(s => s.Parent).ToListAsync();
        var batches = await _context.Batches.ToListAsync();
        var boards = await _context.Boards.ToListAsync();
        var grades = await _context.Grades.ToListAsync();
        var subjects = await _context.Subjects.ToListAsync();
        var expenses = await _context.Expenses.ToListAsync();
        var payments = await _context.Payments.ToListAsync();
        var stationeryItems = await _context.StationeryItems.ToListAsync();
        var stationeryPurchases = await _context.StationeryPurchases.ToListAsync();
        var stationerySales = await _context.StationerySales.ToListAsync();
        var assessments = await _context.Assessments.Include(a => a.AssessmentSubjects).ThenInclude(asub => asub.Marks).ToListAsync();
        var timetables = await _context.Timetables.ToListAsync();
        var homeworks = await _context.Homeworks.Include(h => h.Submissions).ToListAsync();
        var attendances = await _context.Attendances.ToListAsync();

        var backupData = new
        {
            Version = "1.0",
            ExportedAt = DateTime.UtcNow,
            CenterSettings = center,
            Boards = boards,
            Grades = grades,
            Subjects = subjects,
            Batches = batches,
            Students = students,
            Payments = payments,
            Expenses = expenses,
            StationeryItems = stationeryItems,
            StationeryPurchases = stationeryPurchases,
            StationerySales = stationerySales,
            Assessments = assessments,
            Timetables = timetables,
            Homeworks = homeworks,
            Attendances = attendances
        };

        var json = JsonSerializer.Serialize(backupData, new JsonSerializerOptions
        {
            WriteIndented = true,
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
        });

        var bytes = Encoding.UTF8.GetBytes(json);
        var filename = $"TuitionCenter_Backup_{DateTime.UtcNow:yyyyMMdd_HHmm}.json";

        return File(bytes, "application/json", filename);
    }
}
