using System.ComponentModel.DataAnnotations;

namespace TutionCenterMaintanance.Shared.DTOs.Settings;

public class CenterSettingsDto
{
    public int Id { get; set; }
    [Required]
    public string CenterName { get; set; } = "Bright Minds Tuition Center";
    public string? Tagline { get; set; } = "Excellence in Learning from UKG to Grade 12";
    public string? LogoBase64 { get; set; }
    public string? Address { get; set; } = "Main Road, Center City";
    public string? Phone { get; set; } = "+91 98765 43210";
    public string? Email { get; set; } = "contact@brightminds.com";
    public string? Website { get; set; }
    public string? CurrencySymbol { get; set; } = "₹";
}
