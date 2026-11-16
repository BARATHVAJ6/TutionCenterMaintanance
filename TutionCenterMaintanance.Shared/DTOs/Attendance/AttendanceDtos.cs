using System.ComponentModel.DataAnnotations;
using TutionCenterMaintanance.Shared.Enums;

namespace TutionCenterMaintanance.Shared.DTOs.Attendance;

public class DailyAttendanceItemDto
{
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string GradeName { get; set; } = string.Empty;
    public AttendanceStatus Status { get; set; } = AttendanceStatus.Present;
    public string? Remarks { get; set; }
}

public class BatchAttendanceSheetDto
{
    public int? BatchId { get; set; }
    public string? BatchName { get; set; }
    public int? GradeId { get; set; }
    public DateTime Date { get; set; }
    public List<DailyAttendanceItemDto> Students { get; set; } = new();
}

public class SaveDailyAttendanceRequest
{
    public int? BatchId { get; set; }
    public int? GradeId { get; set; }
    [Required]
    public DateTime Date { get; set; }
    public List<DailyAttendanceItemDto> AttendanceList { get; set; } = new();
}

public class AttendanceSummaryDto
{
    public int TotalStudents { get; set; }
    public int PresentCount { get; set; }
    public int AbsentCount { get; set; }
    public decimal AttendancePercentage => TotalStudents > 0 
        ? Math.Round(((decimal)PresentCount / TotalStudents) * 100, 1) 
        : 0;
}
