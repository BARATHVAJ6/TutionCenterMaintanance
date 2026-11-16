namespace TutionCenterMaintanance.Data.Entities;

public class Board : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<BoardGradeSubject> BoardGradeSubjects { get; set; } = new List<BoardGradeSubject>();
    public ICollection<Student> Students { get; set; } = new List<Student>();
    public ICollection<Batch> Batches { get; set; } = new List<Batch>();
}

public class Grade : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public int OrderIndex { get; set; }

    public ICollection<BoardGradeSubject> BoardGradeSubjects { get; set; } = new List<BoardGradeSubject>();
    public ICollection<Student> Students { get; set; } = new List<Student>();
    public ICollection<Batch> Batches { get; set; } = new List<Batch>();
    public ICollection<FeePlan> FeePlans { get; set; } = new List<FeePlan>();
}

public class Subject : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<BoardGradeSubject> BoardGradeSubjects { get; set; } = new List<BoardGradeSubject>();
    public ICollection<AssessmentSubject> AssessmentSubjects { get; set; } = new List<AssessmentSubject>();
}

public class BoardGradeSubject : BaseEntity
{
    public int BoardId { get; set; }
    public Board Board { get; set; } = null!;

    public int GradeId { get; set; }
    public Grade Grade { get; set; } = null!;

    public int SubjectId { get; set; }
    public Subject Subject { get; set; } = null!;
}

public class AcademicYear : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsCurrent { get; set; }

    public ICollection<Batch> Batches { get; set; } = new List<Batch>();
    public ICollection<Assessment> Assessments { get; set; } = new List<Assessment>();
}

public class Batch : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public int GradeId { get; set; }
    public Grade Grade { get; set; } = null!;
    public int BoardId { get; set; }
    public Board Board { get; set; } = null!;
    public int AcademicYearId { get; set; }
    public AcademicYear AcademicYear { get; set; } = null!;
    public string? Schedule { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<StudentBatch> StudentBatches { get; set; } = new List<StudentBatch>();
    public ICollection<TeacherBatchSubject> TeacherBatchSubjects { get; set; } = new List<TeacherBatchSubject>();
    public ICollection<Timetable> Timetables { get; set; } = new List<Timetable>();
}

public class StudentBatch
{
    public int StudentId { get; set; }
    public Student Student { get; set; } = null!;

    public int BatchId { get; set; }
    public Batch Batch { get; set; } = null!;

    public DateTime EnrolledDate { get; set; } = DateTime.UtcNow;
}

public class Teacher : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Specialization { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<TeacherBatchSubject> TeacherBatchSubjects { get; set; } = new List<TeacherBatchSubject>();
}

public class TeacherBatchSubject : BaseEntity
{
    public int TeacherId { get; set; }
    public Teacher Teacher { get; set; } = null!;
    public int BatchId { get; set; }
    public Batch Batch { get; set; } = null!;
    public int SubjectId { get; set; }
    public Subject Subject { get; set; } = null!;
}
