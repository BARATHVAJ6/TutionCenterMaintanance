using System.ComponentModel.DataAnnotations;

namespace TutionCenterMaintanance.Shared.DTOs.Homework;

public class HomeworkDto
{
    public int Id { get; set; }
    public int BatchId { get; set; }
    public string BatchName { get; set; } = string.Empty;
    public string GradeName { get; set; } = string.Empty;
    public int SubjectId { get; set; }
    public string SubjectName { get; set; } = string.Empty;
    public int TeacherId { get; set; }
    public string TeacherName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime AssignedDate { get; set; }
    public DateTime DueDate { get; set; }

    public int TotalStudentsCount { get; set; }
    public int SubmittedCount { get; set; }
    public int CheckedCount { get; set; }
    public int LateCount { get; set; }
    public int PendingCount => Math.Max(0, TotalStudentsCount - (SubmittedCount + CheckedCount + LateCount));

    public bool IsOverdue => DueDate.Date < DateTime.Today;
}

public class CreateHomeworkRequest
{
    [Required]
    public int BatchId { get; set; }
    [Required]
    public int SubjectId { get; set; }
    [Required]
    public int TeacherId { get; set; }
    [Required]
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime AssignedDate { get; set; } = DateTime.Today;
    public DateTime DueDate { get; set; } = DateTime.Today.AddDays(2);
}

public class HomeworkSubmissionItemDto
{
    public int? SubmissionId { get; set; }
    public int HomeworkId { get; set; }
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string GradeName { get; set; } = string.Empty;
    public string ParentName { get; set; } = string.Empty;
    public string ParentPhone { get; set; } = string.Empty;
    public string? WhatsAppPhone { get; set; }
    public string Status { get; set; } = "Pending"; // Pending, Submitted, Checked, Late
    public string? Remarks { get; set; }
    public DateTime? SubmittedDate { get; set; }
}

public class HomeworkDetailDto
{
    public HomeworkDto Homework { get; set; } = new();
    public List<HomeworkSubmissionItemDto> Submissions { get; set; } = new();
}

public class BulkUpdateHomeworkSubmissionsRequest
{
    public int HomeworkId { get; set; }
    public List<HomeworkSubmissionItemDto> Submissions { get; set; } = new();
}
