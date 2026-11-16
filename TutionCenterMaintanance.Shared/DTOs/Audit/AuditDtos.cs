namespace TutionCenterMaintanance.Shared.DTOs.Audit;

public class AuditLogDto
{
    public int Id { get; set; }
    public string EntityName { get; set; } = string.Empty;
    public string EntityId { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty; // Create, Update, Delete
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public string? UserId { get; set; }
    public string? UserName { get; set; }
    public DateTime Timestamp { get; set; }
}

public class SystemBackupBundleDto
{
    public string BackupVersion { get; set; } = "1.0";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string CenterName { get; set; } = string.Empty;
    public int TotalStudentsCount { get; set; }
    public int TotalPaymentsCount { get; set; }
    public int TotalMarksCount { get; set; }
    public string SerializedData { get; set; } = string.Empty;
}
