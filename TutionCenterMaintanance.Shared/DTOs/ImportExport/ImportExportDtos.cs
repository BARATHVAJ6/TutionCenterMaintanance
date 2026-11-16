namespace TutionCenterMaintanance.Shared.DTOs.ImportExport;

public class StudentCsvRow
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Gender { get; set; } = "Male";
    public string DateOfBirth { get; set; } = "2010-01-01";
    public string GradeName { get; set; } = "Grade 10";
    public string BoardName { get; set; } = "CBSE";
    public string SchoolName { get; set; } = string.Empty;
    public string FatherName { get; set; } = string.Empty;
    public string FatherPhone { get; set; } = string.Empty;
    public string FatherWhatsApp { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}

public class ImportSummaryDto
{
    public int TotalRows { get; set; }
    public int SuccessCount { get; set; }
    public int FailureCount { get; set; }
    public List<string> Errors { get; set; } = new();
    public List<string> SuccessMessages { get; set; } = new();
}

public class ImportStudentsRequest
{
    public string CsvContent { get; set; } = string.Empty;
}
