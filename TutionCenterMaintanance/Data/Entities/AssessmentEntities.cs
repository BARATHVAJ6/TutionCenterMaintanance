using TutionCenterMaintanance.Shared.Enums;

namespace TutionCenterMaintanance.Data.Entities;

public class AssessmentType : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public AssessmentCategory Category { get; set; } = AssessmentCategory.TuitionTest;
    public string? Description { get; set; }

    public ICollection<Assessment> Assessments { get; set; } = new List<Assessment>();
}

public class Assessment : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public int AssessmentTypeId { get; set; }
    public AssessmentType AssessmentType { get; set; } = null!;

    public int AcademicYearId { get; set; }
    public AcademicYear AcademicYear { get; set; } = null!;

    public int GradeId { get; set; }
    public Grade Grade { get; set; } = null!;

    public int BoardId { get; set; }
    public Board Board { get; set; } = null!;

    public DateTime Date { get; set; }

    public ICollection<AssessmentSubject> AssessmentSubjects { get; set; } = new List<AssessmentSubject>();
}

public class AssessmentSubject : BaseEntity
{
    public int AssessmentId { get; set; }
    public Assessment Assessment { get; set; } = null!;

    public int SubjectId { get; set; }
    public Subject Subject { get; set; } = null!;

    public decimal MaxMarks { get; set; } = 100;

    public ICollection<Mark> Marks { get; set; } = new List<Mark>();
}

public class Mark : BaseEntity
{
    public int StudentId { get; set; }
    public Student Student { get; set; } = null!;

    public int AssessmentSubjectId { get; set; }
    public AssessmentSubject AssessmentSubject { get; set; } = null!;

    public decimal? MarksObtained { get; set; }
    public bool IsAbsent { get; set; } = false;
    public string? Remarks { get; set; }

    public string? EnteredByUserId { get; set; }
    public DateTime EnteredAt { get; set; } = DateTime.UtcNow;
}

public class Attendance : BaseEntity
{
    public int StudentId { get; set; }
    public Student Student { get; set; } = null!;

    public int? BatchId { get; set; }
    public Batch? Batch { get; set; }

    public DateTime Date { get; set; }
    public AttendanceStatus Status { get; set; } = AttendanceStatus.Present;
    public string? Remarks { get; set; }

    public string? MarkedByUserId { get; set; }
}
