namespace TutionCenterMaintanance.Shared.DTOs.Reports;

public class StudentProgressTrendDto
{
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string GradeName { get; set; } = string.Empty;
    public List<AssessmentScoreTrendItemDto> TrendPoints { get; set; } = new();
    public decimal OverallAveragePercentage { get; set; }
}

public class AssessmentScoreTrendItemDto
{
    public int AssessmentId { get; set; }
    public string AssessmentName { get; set; } = string.Empty;
    public string AssessmentTypeName { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal MarksObtained { get; set; }
    public decimal MaxMarks { get; set; }
    public decimal Percentage => MaxMarks > 0 ? Math.Round((MarksObtained / MaxMarks) * 100, 1) : 0;
    public decimal ClassAveragePercentage { get; set; }
}

public class SubjectPerformanceComparisonDto
{
    public int SubjectId { get; set; }
    public string SubjectName { get; set; } = string.Empty;
    public int TotalAssessmentsEvaluated { get; set; }
    public int TotalStudentsEvaluated { get; set; }
    public decimal AverageScorePercentage { get; set; }
    public decimal HighestScorePercentage { get; set; }
    public decimal LowestScorePercentage { get; set; }
    public decimal PassPercentage { get; set; }
}

public class BatchPerformanceComparisonDto
{
    public int BatchId { get; set; }
    public string BatchName { get; set; } = string.Empty;
    public string GradeName { get; set; } = string.Empty;
    public string BoardName { get; set; } = string.Empty;
    public int StudentCount { get; set; }
    public decimal AverageAcademicScore { get; set; }
    public decimal AverageAttendancePercentage { get; set; }
    public int TopPerformerStudentId { get; set; }
    public string TopPerformerName { get; set; } = string.Empty;
    public decimal TopPerformerScore { get; set; }
}

public class AttendancePerformanceCorrelationDto
{
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string GradeName { get; set; } = string.Empty;
    public string BatchName { get; set; } = string.Empty;
    public decimal AttendancePercentage { get; set; }
    public decimal AcademicScorePercentage { get; set; }
    public string CategoryStatus { get; set; } = string.Empty; // "High Att. / High Score", "Low Att. Alert", "Need Support", "Consistent"
    public string Recommendation { get; set; } = string.Empty;
}
