namespace TutionCenterMaintanance.Shared.DTOs.Notifications;

public class NotificationDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = "Info"; // Info, Warning, Success, Error
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
    public string RelativeTime
    {
        get
        {
            var span = DateTime.UtcNow - CreatedAt;
            if (span.TotalMinutes < 1) return "Just now";
            if (span.TotalMinutes < 60) return $"{(int)span.TotalMinutes}m ago";
            if (span.TotalHours < 24) return $"{(int)span.TotalHours}h ago";
            return CreatedAt.ToString("MMM dd");
        }
    }
}

public class NotificationCountDto
{
    public int UnreadCount { get; set; }
}
