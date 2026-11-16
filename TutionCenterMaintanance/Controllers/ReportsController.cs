using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TutionCenterMaintanance.Data;
using TutionCenterMaintanance.Shared.Constants;
using TutionCenterMaintanance.Shared.DTOs.Reports;
using TutionCenterMaintanance.Shared.Enums;
using TutionCenterMaintanance.Shared.Models;

namespace TutionCenterMaintanance.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = Permissions.ReportsView)]
public class ReportsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ReportsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("student-trend/{studentId}")]
    public async Task<ActionResult<ApiResponse<StudentProgressTrendDto>>> GetStudentTrend(int studentId)
    {
        var student = await _context.Students
            .Include(s => s.Grade)
            .FirstOrDefaultAsync(s => s.Id == studentId);

        if (student == null) return NotFound(ApiResponse<StudentProgressTrendDto>.Fail("Student not found"));

        var marks = await _context.Marks
            .Include(m => m.AssessmentSubject).ThenInclude(asub => asub.Assessment).ThenInclude(a => a.AssessmentType)
            .Where(m => m.StudentId == studentId && m.AssessmentSubject.Assessment != null)
            .OrderBy(m => m.AssessmentSubject.Assessment.Date)
            .ToListAsync();

        var trendPoints = new List<AssessmentScoreTrendItemDto>();

        // Group by assessment
        var assessmentGroups = marks.GroupBy(m => m.AssessmentSubject.Assessment);

        foreach (var group in assessmentGroups)
        {
            var assessment = group.Key;
            var totalMax = group.Sum(m => m.AssessmentSubject.MaxMarks);
            var totalObtained = group.Sum(m => m.MarksObtained ?? 0);

            // Calculate class average for this assessment
            var allClassMarks = await _context.Marks
                .Where(m => m.AssessmentSubject.AssessmentId == assessment.Id)
                .ToListAsync();

            decimal classAvg = 0;
            if (allClassMarks.Any())
            {
                var studentTotals = allClassMarks.GroupBy(m => m.StudentId)
                    .Select(g => (g.Sum(m => m.MarksObtained ?? 0) / (totalMax > 0 ? totalMax : 100)) * 100);
                classAvg = Math.Round(studentTotals.Average(), 1);
            }

            trendPoints.Add(new AssessmentScoreTrendItemDto
            {
                AssessmentId = assessment.Id,
                AssessmentName = assessment.Name,
                AssessmentTypeName = assessment.AssessmentType.Name,
                Date = assessment.Date,
                MarksObtained = totalObtained,
                MaxMarks = totalMax,
                ClassAveragePercentage = classAvg
            });
        }

        var overallAvg = trendPoints.Any() ? Math.Round(trendPoints.Average(t => t.Percentage), 1) : 0;

        return Ok(ApiResponse<StudentProgressTrendDto>.Ok(new StudentProgressTrendDto
        {
            StudentId = student.Id,
            StudentName = $"{student.FirstName} {student.LastName}".Trim(),
            GradeName = student.Grade.Name,
            TrendPoints = trendPoints,
            OverallAveragePercentage = overallAvg
        }));
    }

    [HttpGet("subject-comparison")]
    public async Task<ActionResult<ApiResponse<List<SubjectPerformanceComparisonDto>>>> GetSubjectComparison()
    {
        var subjects = await _context.Subjects
            .Include(s => s.AssessmentSubjects).ThenInclude(asub => asub.Marks)
            .ToListAsync();

        var list = new List<SubjectPerformanceComparisonDto>();

        foreach (var sub in subjects)
        {
            var allMarks = sub.AssessmentSubjects
                .SelectMany(asub => asub.Marks.Where(m => m.MarksObtained.HasValue && !m.IsAbsent).Select(m => new
                {
                    Percentage = asub.MaxMarks > 0 ? ((m.MarksObtained!.Value / asub.MaxMarks) * 100) : 0
                }))
                .ToList();

            if (!allMarks.Any())
            {
                list.Add(new SubjectPerformanceComparisonDto
                {
                    SubjectId = sub.Id,
                    SubjectName = sub.Name,
                    TotalAssessmentsEvaluated = 0,
                    TotalStudentsEvaluated = 0,
                    AverageScorePercentage = 0,
                    HighestScorePercentage = 0,
                    LowestScorePercentage = 0,
                    PassPercentage = 0
                });
                continue;
            }

            var avg = Math.Round(allMarks.Average(m => m.Percentage), 1);
            var high = Math.Round(allMarks.Max(m => m.Percentage), 1);
            var low = Math.Round(allMarks.Min(m => m.Percentage), 1);
            var passCount = allMarks.Count(m => m.Percentage >= 40);
            var passRate = Math.Round(((decimal)passCount / allMarks.Count) * 100, 1);

            list.Add(new SubjectPerformanceComparisonDto
            {
                SubjectId = sub.Id,
                SubjectName = sub.Name,
                TotalAssessmentsEvaluated = sub.AssessmentSubjects.Count,
                TotalStudentsEvaluated = allMarks.Count,
                AverageScorePercentage = avg,
                HighestScorePercentage = high,
                LowestScorePercentage = low,
                PassPercentage = passRate
            });
        }

        return Ok(ApiResponse<List<SubjectPerformanceComparisonDto>>.Ok(list.OrderByDescending(s => s.AverageScorePercentage).ToList()));
    }

    [HttpGet("batch-comparison")]
    public async Task<ActionResult<ApiResponse<List<BatchPerformanceComparisonDto>>>> GetBatchComparison()
    {
        var batches = await _context.Batches
            .Include(b => b.Grade)
            .Include(b => b.Board)
            .Include(b => b.StudentBatches).ThenInclude(sb => sb.Student)
            .ToListAsync();

        var list = new List<BatchPerformanceComparisonDto>();

        foreach (var b in batches)
        {
            var activeStudents = b.StudentBatches.Select(sb => sb.Student).Where(s => s.IsActive).ToList();
            var studentIds = activeStudents.Select(s => s.Id).ToList();

            // Academic average
            var marks = await _context.Marks
                .Include(m => m.AssessmentSubject)
                .Where(m => studentIds.Contains(m.StudentId) && m.MarksObtained.HasValue && !m.IsAbsent)
                .ToListAsync();

            decimal academicAvg = 0;
            if (marks.Any())
            {
                academicAvg = Math.Round(marks.Average(m => (m.MarksObtained!.Value / (m.AssessmentSubject.MaxMarks > 0 ? m.AssessmentSubject.MaxMarks : 100)) * 100), 1);
            }

            // Attendance average
            decimal attAvg = 0;
            var atts = await _context.Attendances.Where(a => a.BatchId == b.Id).ToListAsync();
            if (atts.Any())
            {
                var present = atts.Count(a => a.Status == AttendanceStatus.Present);
                attAvg = Math.Round(((decimal)present / atts.Count) * 100, 1);
            }

            // Top Performer
            string topName = "-";
            int topId = 0;
            decimal topScore = 0;

            if (marks.Any())
            {
                var studentAvgs = marks.GroupBy(m => m.StudentId)
                    .Select(g => new
                    {
                        StudentId = g.Key,
                        Avg = g.Average(m => (m.MarksObtained!.Value / (m.AssessmentSubject.MaxMarks > 0 ? m.AssessmentSubject.MaxMarks : 100)) * 100)
                    })
                    .OrderByDescending(x => x.Avg)
                    .FirstOrDefault();

                if (studentAvgs != null)
                {
                    topId = studentAvgs.StudentId;
                    topScore = Math.Round(studentAvgs.Avg, 1);
                    var topStudent = activeStudents.FirstOrDefault(s => s.Id == topId);
                    if (topStudent != null) topName = $"{topStudent.FirstName} {topStudent.LastName}".Trim();
                }
            }

            list.Add(new BatchPerformanceComparisonDto
            {
                BatchId = b.Id,
                BatchName = b.Name,
                GradeName = b.Grade.Name,
                BoardName = b.Board.Name,
                StudentCount = activeStudents.Count,
                AverageAcademicScore = academicAvg,
                AverageAttendancePercentage = attAvg,
                TopPerformerStudentId = topId,
                TopPerformerName = topName,
                TopPerformerScore = topScore
            });
        }

        return Ok(ApiResponse<List<BatchPerformanceComparisonDto>>.Ok(list));
    }

    [HttpGet("attendance-correlation")]
    public async Task<ActionResult<ApiResponse<List<AttendancePerformanceCorrelationDto>>>> GetAttendanceCorrelation()
    {
        var students = await _context.Students
            .Include(s => s.Grade)
            .Include(s => s.StudentBatches).ThenInclude(sb => sb.Batch)
            .Include(s => s.Attendances)
            .Where(s => s.IsActive)
            .ToListAsync();

        var list = new List<AttendancePerformanceCorrelationDto>();

        foreach (var s in students)
        {
            // Attendance %
            var totalAtt = s.Attendances.Count;
            var presentAtt = s.Attendances.Count(a => a.Status == AttendanceStatus.Present);
            var attPct = totalAtt > 0 ? Math.Round(((decimal)presentAtt / totalAtt) * 100, 1) : 100;

            // Academic %
            var marks = await _context.Marks
                .Include(m => m.AssessmentSubject)
                .Where(m => m.StudentId == s.Id && m.MarksObtained.HasValue && !m.IsAbsent)
                .ToListAsync();

            decimal academicPct = 0;
            if (marks.Any())
            {
                academicPct = Math.Round(marks.Average(m => (m.MarksObtained!.Value / (m.AssessmentSubject.MaxMarks > 0 ? m.AssessmentSubject.MaxMarks : 100)) * 100), 1);
            }

            string category;
            string recommendation;

            if (attPct >= 85 && academicPct >= 75)
            {
                category = "Star Performer";
                recommendation = "Excellent discipline & high scores. Encourage for competitive/scholarship exams.";
            }
            else if (attPct < 75 && academicPct < 50)
            {
                category = "Critical Academic & Attendance Risk";
                recommendation = "Immediate parent meeting required. High absenteeism directly degrading exam scores.";
            }
            else if (attPct >= 85 && academicPct < 50)
            {
                category = "Needs Conceptual Tutoring";
                recommendation = "Regular in attendance but struggles with test comprehension. Needs remedial doubt-clearing sessions.";
            }
            else if (attPct < 75 && academicPct >= 70)
            {
                category = "Irregular High Potential";
                recommendation = "Capable student with inconsistent attendance. Push for 100% attendance to reach top ranks.";
            }
            else
            {
                category = "Consistent Steady Progress";
                recommendation = "Maintaining average pace. Monitor monthly progress.";
            }

            var batchName = s.StudentBatches.FirstOrDefault()?.Batch.Name ?? "Unassigned";

            list.Add(new AttendancePerformanceCorrelationDto
            {
                StudentId = s.Id,
                StudentName = $"{s.FirstName} {s.LastName}".Trim(),
                GradeName = s.Grade.Name,
                BatchName = batchName,
                AttendancePercentage = attPct,
                AcademicScorePercentage = academicPct,
                CategoryStatus = category,
                Recommendation = recommendation
            });
        }

        return Ok(ApiResponse<List<AttendancePerformanceCorrelationDto>>.Ok(list.OrderBy(x => x.AttendancePercentage).ToList()));
    }
}
