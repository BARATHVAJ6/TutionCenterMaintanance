using TutionCenterMaintanance.Shared.Enums;

namespace TutionCenterMaintanance.Data.Entities;

public class Student : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string? PhotoUrl { get; set; }
    public int BoardId { get; set; }
    public Board Board { get; set; } = null!;
    public int GradeId { get; set; }
    public Grade Grade { get; set; } = null!;
    public string SchoolName { get; set; } = string.Empty;
    public string? Address { get; set; }
    public DateTime AdmissionDate { get; set; }
    public string? BloodGroup { get; set; }
    public string? MedicalNotes { get; set; }
    public string? PreviousRecords { get; set; }
    public decimal MonthlyFeeAmount { get; set; } = 0;
    public bool IsActive { get; set; } = true;

    public ParentInfo Parent { get; set; } = null!;
    public ICollection<StudentBatch> StudentBatches { get; set; } = new List<StudentBatch>();
    public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    public ICollection<Mark> Marks { get; set; } = new List<Mark>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}

public class ParentInfo : BaseEntity
{
    public int StudentId { get; set; }
    public Student Student { get; set; } = null!;

    public string FatherName { get; set; } = string.Empty;
    public string FatherPhone { get; set; } = string.Empty;
    public string? FatherWhatsApp { get; set; }
    public string? MotherName { get; set; }
    public string? MotherPhone { get; set; }
    public string? MotherWhatsApp { get; set; }
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
}
