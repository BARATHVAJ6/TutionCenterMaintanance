using System.ComponentModel.DataAnnotations;

namespace TutionCenterMaintanance.Shared.DTOs.Academic;

public class BoardDto
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public bool IsActive { get; set; } = true;
}

public class GradeDto
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    public int OrderIndex { get; set; }
}

public class SubjectDto
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public bool IsActive { get; set; } = true;
}

public class BoardGradeSubjectDto
{
    public int Id { get; set; }
    public int BoardId { get; set; }
    public string BoardName { get; set; } = string.Empty;
    public int GradeId { get; set; }
    public string GradeName { get; set; } = string.Empty;
    public int SubjectId { get; set; }
    public string SubjectName { get; set; } = string.Empty;
}

public class ManageBoardGradeSubjectsRequest
{
    public int BoardId { get; set; }
    public int GradeId { get; set; }
    public List<int> SubjectIds { get; set; } = new();
}

public class AcademicYearDto
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsCurrent { get; set; }
}

public class BatchDto
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    public int GradeId { get; set; }
    public string GradeName { get; set; } = string.Empty;
    public int BoardId { get; set; }
    public string BoardName { get; set; } = string.Empty;
    public int AcademicYearId { get; set; }
    public string AcademicYearName { get; set; } = string.Empty;
    public string? Schedule { get; set; }
    public bool IsActive { get; set; } = true;
    public int StudentCount { get; set; }
    public List<int> StudentIds { get; set; } = new();
}

public class CreateBatchRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;
    public int GradeId { get; set; }
    public int BoardId { get; set; }
    public int AcademicYearId { get; set; }
    public string? Schedule { get; set; }
    public bool IsActive { get; set; } = true;
    public List<int> StudentIds { get; set; } = new();
}

public class TeacherDto
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Specialization { get; set; }
    public bool IsActive { get; set; } = true;
    public List<int> SubjectIds { get; set; } = new();
    public List<string> SubjectNames { get; set; } = new();
}
