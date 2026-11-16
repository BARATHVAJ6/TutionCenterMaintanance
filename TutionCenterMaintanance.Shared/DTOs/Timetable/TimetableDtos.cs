using System.ComponentModel.DataAnnotations;

namespace TutionCenterMaintanance.Shared.DTOs.Timetable;

public class TimetableSlotDto
{
    public int Id { get; set; }
    public int BatchId { get; set; }
    public string BatchName { get; set; } = string.Empty;
    public string GradeName { get; set; } = string.Empty;
    public int SubjectId { get; set; }
    public string SubjectName { get; set; } = string.Empty;
    public int TeacherId { get; set; }
    public string TeacherName { get; set; } = string.Empty;
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }

    public string TimeFormatted => $"{DateTime.Today.Add(StartTime):hh:mm tt} - {DateTime.Today.Add(EndTime):hh:mm tt}";
    public string DayName => DayOfWeek.ToString();
}

public class CreateOrUpdateTimetableRequest
{
    public int? Id { get; set; }
    [Required]
    public int BatchId { get; set; }
    [Required]
    public int SubjectId { get; set; }
    [Required]
    public int TeacherId { get; set; }
    [Required]
    public DayOfWeek DayOfWeek { get; set; }
    [Required]
    public TimeSpan StartTime { get; set; }
    [Required]
    public TimeSpan EndTime { get; set; }
}

public class BatchWeeklyTimetableDto
{
    public int BatchId { get; set; }
    public string BatchName { get; set; } = string.Empty;
    public string GradeName { get; set; } = string.Empty;
    public List<TimetableSlotDto> Slots { get; set; } = new();
}
