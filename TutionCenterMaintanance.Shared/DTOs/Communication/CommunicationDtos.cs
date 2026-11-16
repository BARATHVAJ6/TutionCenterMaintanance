using System.ComponentModel.DataAnnotations;

namespace TutionCenterMaintanance.Shared.DTOs.Communication;

public class WhatsAppTemplateDto
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = "General"; // FeeReminder, PaymentConfirmation, ExamResult, Attendance, Announcement, General
    [Required]
    public string BodyTemplate { get; set; } = string.Empty;
    public bool IsSystem { get; set; }
}

public class CreateOrUpdateTemplateRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = "General";
    [Required]
    public string BodyTemplate { get; set; } = string.Empty;
}

public class WhatsAppMessageLogDto
{
    public int Id { get; set; }
    public int? StudentId { get; set; }
    public string? StudentName { get; set; }
    public string RecipientName { get; set; } = string.Empty;
    public string RecipientPhone { get; set; } = string.Empty;
    public string MessageCategory { get; set; } = "General";
    public string MessageBody { get; set; } = string.Empty;
    public string SentByName { get; set; } = string.Empty;
    public DateTime SentAt { get; set; }
}

public class LogWhatsAppDispatchDto
{
    public int? StudentId { get; set; }
    public string RecipientName { get; set; } = string.Empty;
    public string RecipientPhone { get; set; } = string.Empty;
    public string MessageCategory { get; set; } = "General";
    public string MessageBody { get; set; } = string.Empty;
}

public class BulkWhatsAppRecipientDto
{
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string GradeName { get; set; } = string.Empty;
    public string ParentName { get; set; } = string.Empty;
    public string RawPhone { get; set; } = string.Empty;
    public string CleanPhone { get; set; } = string.Empty;
    public string Category { get; set; } = "General";
    public string Details { get; set; } = string.Empty;
    public string RenderedMessage { get; set; } = string.Empty;
    public string WhatsAppUrl => $"https://wa.me/{CleanPhone}?text={Uri.EscapeDataString(RenderedMessage)}";
    public bool HasValidPhone => !string.IsNullOrWhiteSpace(CleanPhone) && CleanPhone.Length >= 10;
    public bool IsSent { get; set; }
}
