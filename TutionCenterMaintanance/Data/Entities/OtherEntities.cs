namespace TutionCenterMaintanance.Data.Entities;

public class Timetable : BaseEntity
{
    public int BatchId { get; set; }
    public Batch Batch { get; set; } = null!;

    public int SubjectId { get; set; }
    public Subject Subject { get; set; } = null!;

    public int TeacherId { get; set; }
    public Teacher Teacher { get; set; } = null!;

    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
}

public class Homework : BaseEntity
{
    public int BatchId { get; set; }
    public Batch Batch { get; set; } = null!;

    public int SubjectId { get; set; }
    public Subject Subject { get; set; } = null!;

    public int TeacherId { get; set; }
    public Teacher Teacher { get; set; } = null!;

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime AssignedDate { get; set; } = DateTime.UtcNow;
    public DateTime DueDate { get; set; }

    public ICollection<HomeworkSubmission> Submissions { get; set; } = new List<HomeworkSubmission>();
}

public class HomeworkSubmission : BaseEntity
{
    public int HomeworkId { get; set; }
    public Homework Homework { get; set; } = null!;

    public int StudentId { get; set; }
    public Student Student { get; set; } = null!;

    public string Status { get; set; } = "Submitted"; // Submitted, Checked, Late
    public string? Remarks { get; set; }
    public DateTime SubmittedDate { get; set; } = DateTime.UtcNow;
}

public class Notification : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = "Info"; // Info, Warning, Success, Error
    public string? UserId { get; set; }
    public bool IsRead { get; set; } = false;
}

public class WhatsAppTemplate : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = "General"; // FeeReminder, PaymentConfirmation, ExamResult, Attendance, Announcement, General
    public string BodyTemplate { get; set; } = string.Empty;
    public bool IsSystem { get; set; } = false;
}

public class WhatsAppMessageLog : BaseEntity
{
    public int? StudentId { get; set; }
    public Student? Student { get; set; }
    public string RecipientName { get; set; } = string.Empty;
    public string RecipientPhone { get; set; } = string.Empty;
    public string MessageCategory { get; set; } = "General";
    public string MessageBody { get; set; } = string.Empty;
    public string? SentByUserId { get; set; }
    public AppUser? SentByUser { get; set; }
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
}

public class AuditLog
{
    public int Id { get; set; }
    public string EntityName { get; set; } = string.Empty;
    public string EntityId { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty; // Create, Update, Delete
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public string? UserId { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

public class CenterSettings : BaseEntity
{
    public string CenterName { get; set; } = "Sai Tuition Center";
    public string? Tagline { get; set; } = "Excellence in Learning from UKG to Grade 12";
    public string? LogoBase64 { get; set; }
    public string? Address { get; set; } = "Main Road, Center City";
    public string? Phone { get; set; } = "+91 98765 43210";
    public string? Email { get; set; } = "contact@brightminds.com";
    public string? Website { get; set; }
    public string? CurrencySymbol { get; set; } = "₹";
}
