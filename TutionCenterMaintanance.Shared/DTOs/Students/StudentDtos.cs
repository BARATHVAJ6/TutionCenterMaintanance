using System.ComponentModel.DataAnnotations;
using TutionCenterMaintanance.Shared.Enums;

namespace TutionCenterMaintanance.Shared.DTOs.Students;

public class ParentInfoDto
{
    public string? FatherName { get; set; } = string.Empty;
    public string? FatherPhone { get; set; } = string.Empty;
    public string? FatherWhatsApp { get; set; }
    public string? MotherName { get; set; }
    public string? MotherPhone { get; set; }
    public string? MotherWhatsApp { get; set; }
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
}

public class StudentDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}".Trim();
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string? PhotoUrl { get; set; }
    public int BoardId { get; set; }
    public string BoardName { get; set; } = string.Empty;
    public int GradeId { get; set; }
    public string GradeName { get; set; } = string.Empty;
    public string SchoolName { get; set; } = string.Empty;
    public string? Address { get; set; }
    public DateTime AdmissionDate { get; set; }
    public string? BloodGroup { get; set; }
    public string? MedicalNotes { get; set; }
    public string? PreviousRecords { get; set; }
    public decimal MonthlyFeeAmount { get; set; }
    public bool IsActive { get; set; }
    public ParentInfoDto Parent { get; set; } = new();
    public List<int> BatchIds { get; set; } = new();
    public List<string> BatchNames { get; set; } = new();
}

public class StudentListItemDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string GradeName { get; set; } = string.Empty;
    public string BoardName { get; set; } = string.Empty;
    public string SchoolName { get; set; } = string.Empty;
    public string FatherName { get; set; } = string.Empty;
    public string PrimaryPhone { get; set; } = string.Empty;
    public string? WhatsAppPhone { get; set; }
    public bool IsActive { get; set; }
    public string? PhotoUrl { get; set; }
    public decimal MonthlyFeeAmount { get; set; }
    public List<string> BatchNames { get; set; } = new();
}

public class CreateStudentRequest
{
    [Required]
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; } = DateTime.Today.AddYears(-10);

    public Gender Gender { get; set; } = Gender.Male;

    [Required]
    public int BoardId { get; set; }

    [Required]
    public int GradeId { get; set; }

    [Required]
    public string SchoolName { get; set; } = string.Empty;

    public string? Address { get; set; }

    public DateTime AdmissionDate { get; set; } = DateTime.Today;

    public string? BloodGroup { get; set; }
    public string? MedicalNotes { get; set; }
    public string? PreviousRecords { get; set; }
    public decimal MonthlyFeeAmount { get; set; } = 0;
    public string? PhotoBase64 { get; set; }

    [Required]
    public ParentInfoDto Parent { get; set; } = new();

    public List<int> BatchIds { get; set; } = new();
}

public class UpdateStudentRequest : CreateStudentRequest
{
    public int Id { get; set; }
    public bool IsActive { get; set; } = true;
}
