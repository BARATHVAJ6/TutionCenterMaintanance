using System.ComponentModel.DataAnnotations;
using TutionCenterMaintanance.Shared.Enums;

namespace TutionCenterMaintanance.Shared.DTOs.Assessments;

public class AssessmentTypeDto
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    public AssessmentCategory Category { get; set; } = AssessmentCategory.TuitionTest;
    public string? Description { get; set; }
}

public class AssessmentSubjectDto
{
    public int Id { get; set; }
    public int AssessmentId { get; set; }
    public int SubjectId { get; set; }
    public string SubjectName { get; set; } = string.Empty;
    public decimal MaxMarks { get; set; }
}

public class AssessmentDto
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    public int AssessmentTypeId { get; set; }
    public string AssessmentTypeName { get; set; } = string.Empty;
    public AssessmentCategory Category { get; set; }
    public int AcademicYearId { get; set; }
    public string AcademicYearName { get; set; } = string.Empty;
    public int GradeId { get; set; }
    public string GradeName { get; set; } = string.Empty;
    public int BoardId { get; set; }
    public string BoardName { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal TotalMarks { get; set; }
    public List<AssessmentSubjectDto> Subjects { get; set; } = new();
}

public class CreateAssessmentRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;
    public int AssessmentTypeId { get; set; }
    public int AcademicYearId { get; set; }
    public int GradeId { get; set; }
    public int BoardId { get; set; }
    public DateTime Date { get; set; } = DateTime.Today;
    public List<CreateAssessmentSubjectItem> Subjects { get; set; } = new();
}

public class CreateAssessmentSubjectItem
{
    public int SubjectId { get; set; }
    public decimal MaxMarks { get; set; } = 100;
}

public class StudentMarkRowDto
{
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public decimal? MarksObtained { get; set; }
    public string? Remarks { get; set; }
    public bool IsAbsent { get; set; }
}

public class BulkMarksGridDto
{
    public int AssessmentId { get; set; }
    public string AssessmentName { get; set; } = string.Empty;
    public int AssessmentSubjectId { get; set; }
    public string SubjectName { get; set; } = string.Empty;
    public decimal MaxMarks { get; set; }
    public List<StudentMarkRowDto> Rows { get; set; } = new();
}

public class BulkSaveMarksRequest
{
    public int AssessmentSubjectId { get; set; }
    public List<StudentMarkSaveItem> Marks { get; set; } = new();
}

public class StudentMarkSaveItem
{
    public int StudentId { get; set; }
    public decimal? MarksObtained { get; set; }
    public string? Remarks { get; set; }
    public bool IsAbsent { get; set; }
}

public class StudentReportCardDto
{
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string GradeName { get; set; } = string.Empty;
    public string BoardName { get; set; } = string.Empty;
    public List<ReportCardExamSectionDto> Exams { get; set; } = new();
}

public class ReportCardExamSectionDto
{
    public string AssessmentName { get; set; } = string.Empty;
    public string AssessmentType { get; set; } = string.Empty;
    public AssessmentCategory Category { get; set; }
    public DateTime Date { get; set; }
    public List<SubjectMarkDetailDto> Marks { get; set; } = new();
    public decimal TotalObtained { get; set; }
    public decimal TotalMaximum { get; set; }
    public decimal Percentage => TotalMaximum > 0 ? Math.Round((TotalObtained / TotalMaximum) * 100, 2) : 0;
}

public class SubjectMarkDetailDto
{
    public string SubjectName { get; set; } = string.Empty;
    public decimal? MarksObtained { get; set; }
    public decimal MaxMarks { get; set; }
    public bool IsAbsent { get; set; }
    public string? Remarks { get; set; }
}
