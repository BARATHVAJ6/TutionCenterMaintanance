namespace TutionCenterMaintanance.Shared.DTOs.Dashboard;

public class DashboardStatsDto
{
    public int TotalActiveStudents { get; set; }
    public int TotalTeachers { get; set; }
    public int TotalBatches { get; set; }
    public decimal TodayAttendancePercentage { get; set; }
    public int TodayPresentCount { get; set; }
    public int TodayAbsentCount { get; set; }
    public decimal TotalFeesCollectedMonth { get; set; }
    public decimal TotalFeesPending { get; set; }
    public int OverdueStudentsCount { get; set; }
    public List<RecentActivityDto> RecentActivities { get; set; } = new();
    public List<UpcomingAssessmentDto> UpcomingAssessments { get; set; } = new();
}

public class RecentActivityDto
{
    public string Description { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string Icon { get; set; } = "Info";
    public string User { get; set; } = string.Empty;
}

public class UpcomingAssessmentDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string GradeName { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int SubjectCount { get; set; }
}
