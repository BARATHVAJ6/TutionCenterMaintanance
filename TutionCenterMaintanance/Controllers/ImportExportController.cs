using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using TutionCenterMaintanance.Data;
using TutionCenterMaintanance.Data.Entities;
using TutionCenterMaintanance.Shared.Constants;
using TutionCenterMaintanance.Shared.DTOs.ImportExport;
using TutionCenterMaintanance.Shared.Enums;
using TutionCenterMaintanance.Shared.Models;

namespace TutionCenterMaintanance.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ImportExportController : ControllerBase
{
    private readonly AppDbContext _context;

    public ImportExportController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("students/export")]
    [Authorize(Policy = Permissions.DataExport)]
    public async Task<IActionResult> ExportStudentsCsv()
    {
        var students = await _context.Students
            .Include(s => s.Grade)
            .Include(s => s.Board)
            .Include(s => s.Parent)
            .Where(s => s.IsActive)
            .OrderBy(s => s.GradeId).ThenBy(s => s.FirstName)
            .ToListAsync();

        var sb = new StringBuilder();
        sb.AppendLine("ID,FirstName,LastName,Gender,DOB,Grade,Board,SchoolName,FatherName,FatherPhone,FatherWhatsApp,MotherName,MotherPhone,Address");

        foreach (var s in students)
        {
            sb.AppendLine($"\"{s.Id}\",\"{EscapeCsv(s.FirstName)}\",\"{EscapeCsv(s.LastName)}\",\"{s.Gender}\",\"{s.DateOfBirth:yyyy-MM-dd}\",\"{EscapeCsv(s.Grade.Name)}\",\"{EscapeCsv(s.Board.Name)}\",\"{EscapeCsv(s.SchoolName)}\",\"{EscapeCsv(s.Parent.FatherName)}\",\"{EscapeCsv(s.Parent.FatherPhone)}\",\"{EscapeCsv(s.Parent.FatherWhatsApp)}\",\"{EscapeCsv(s.Parent.MotherName)}\",\"{EscapeCsv(s.Parent.MotherPhone)}\",\"{EscapeCsv(s.Address)}\"");
        }

        var bytes = Encoding.UTF8.GetBytes(sb.ToString());
        return File(bytes, "text/csv", $"Students_Export_{DateTime.UtcNow:yyyyMMdd_HHmm}.csv");
    }

    [HttpGet("fees/export")]
    [Authorize(Policy = Permissions.DataExport)]
    public async Task<IActionResult> ExportFeesCsv()
    {
        var payments = await _context.Payments
            .Include(p => p.Student).ThenInclude(s => s.Grade)
            .Include(p => p.Student).ThenInclude(s => s.Board)
            .Include(p => p.ReceivedByUser)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();

        var sb = new StringBuilder();
        sb.AppendLine("ReceiptNo,PaymentDate,StudentID,StudentName,Grade,Board,Amount,PaymentMode,TransactionRef,ReceivedBy,Remarks");

        foreach (var p in payments)
        {
            var studentName = $"{p.Student.FirstName} {p.Student.LastName}".Trim();
            sb.AppendLine($"\"{p.ReceiptNumber}\",\"{p.PaymentDate:yyyy-MM-dd}\",\"{p.StudentId}\",\"{EscapeCsv(studentName)}\",\"{EscapeCsv(p.Student.Grade.Name)}\",\"{EscapeCsv(p.Student.Board.Name)}\",\"{p.Amount}\",\"{p.PaymentMode}\",\"{EscapeCsv(p.TransactionRef ?? "")}\",\"{EscapeCsv(p.ReceivedByUser?.FullName ?? "Staff")}\",\"{EscapeCsv(p.Remarks ?? "")}\"");
        }

        var bytes = Encoding.UTF8.GetBytes(sb.ToString());
        return File(bytes, "text/csv", $"Fee_Collections_Export_{DateTime.UtcNow:yyyyMMdd_HHmm}.csv");
    }

    [HttpGet("students/template")]
    public IActionResult GetStudentsImportTemplate()
    {
        var template = "FirstName,LastName,Gender,DateOfBirth,GradeName,BoardName,SchoolName,FatherName,FatherPhone,FatherWhatsApp,Address\n" +
                       "Rohan,Verma,Male,2010-05-15,Grade 10,CBSE,St. Xavier High School,Mr. Ramesh Verma,+91 98765 11111,+91 98765 11111,Civil Lines Street\n" +
                       "Pooja,Patel,Female,2011-08-22,Grade 9,ICSE,Modern Public School,Mr. Anand Patel,+91 98765 22222,+91 98765 22222,Park View Apartments";

        return File(Encoding.UTF8.GetBytes(template), "text/csv", "Students_Import_Template.csv");
    }

    [HttpPost("students/import")]
    [Authorize(Policy = Permissions.DataImport)]
    public async Task<ActionResult<ApiResponse<ImportSummaryDto>>> ImportStudents([FromBody] ImportStudentsRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.CsvContent))
            return BadRequest(ApiResponse<ImportSummaryDto>.Fail("CSV content is empty"));

        var lines = request.CsvContent.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length <= 1)
            return BadRequest(ApiResponse<ImportSummaryDto>.Fail("CSV must have a header row and at least one data row."));

        var summary = new ImportSummaryDto();
        var grades = await _context.Grades.ToListAsync();
        var boards = await _context.Boards.ToListAsync();

        // Line 0 is header
        for (int i = 1; i < lines.Length; i++)
        {
            var line = lines[i].Trim();
            if (string.IsNullOrWhiteSpace(line)) continue;

            summary.TotalRows++;

            try
            {
                var cols = ParseCsvLine(line);
                if (cols.Count < 9)
                {
                    summary.FailureCount++;
                    summary.Errors.Add($"Row {i}: Incomplete columns (expected at least 9).");
                    continue;
                }

                var firstName = cols[0].Trim();
                var lastName = cols[1].Trim();
                var genderStr = cols[2].Trim();
                var dobStr = cols[3].Trim();
                var gradeName = cols[4].Trim();
                var boardName = cols[5].Trim();
                var schoolName = cols.Count > 6 ? cols[6].Trim() : "";
                var fatherName = cols.Count > 7 ? cols[7].Trim() : "";
                var fatherPhone = cols.Count > 8 ? cols[8].Trim() : "";
                var fatherWhatsApp = cols.Count > 9 ? cols[9].Trim() : fatherPhone;
                var address = cols.Count > 10 ? cols[10].Trim() : "";

                if (string.IsNullOrEmpty(firstName))
                {
                    summary.FailureCount++;
                    summary.Errors.Add($"Row {i}: First name is required.");
                    continue;
                }

                var grade = grades.FirstOrDefault(g => g.Name.Equals(gradeName, StringComparison.OrdinalIgnoreCase));
                if (grade == null)
                {
                    summary.FailureCount++;
                    summary.Errors.Add($"Row {i}: Grade '{gradeName}' not found in database.");
                    continue;
                }

                var board = boards.FirstOrDefault(b => b.Name.Equals(boardName, StringComparison.OrdinalIgnoreCase)) ?? boards.FirstOrDefault();
                if (board == null)
                {
                    summary.FailureCount++;
                    summary.Errors.Add($"Row {i}: Board '{boardName}' not found in database.");
                    continue;
                }

                if (!DateTime.TryParse(dobStr, out var dob))
                {
                    dob = new DateTime(2010, 1, 1);
                }

                var gender = Enum.TryParse<Gender>(genderStr, true, out var gVal) ? gVal : Gender.Male;

                // Create Student with Parent navigation
                var student = new Student
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Gender = gender,
                    DateOfBirth = dob,
                    GradeId = grade.Id,
                    BoardId = board.Id,
                    SchoolName = schoolName,
                    Address = address,
                    AdmissionDate = DateTime.Today,
                    IsActive = true,
                    Parent = new ParentInfo
                    {
                        FatherName = !string.IsNullOrEmpty(fatherName) ? fatherName : $"{firstName}'s Parent",
                        FatherPhone = !string.IsNullOrEmpty(fatherPhone) ? fatherPhone : "+91 00000 00000",
                        FatherWhatsApp = fatherWhatsApp
                    }
                };
                _context.Students.Add(student);
                await _context.SaveChangesAsync();

                summary.SuccessCount++;
                summary.SuccessMessages.Add($"Enrolled {firstName} {lastName} ({grade.Name} - {board.Name})");
            }
            catch (Exception ex)
            {
                summary.FailureCount++;
                summary.Errors.Add($"Row {i}: Error processing: {ex.Message}");
            }
        }

        return Ok(ApiResponse<ImportSummaryDto>.Ok(summary, $"Import processed: {summary.SuccessCount} imported, {summary.FailureCount} failed."));
    }

    private static string EscapeCsv(string? val) => (val ?? "").Replace("\"", "\"\"");

    private static List<string> ParseCsvLine(string line)
    {
        var result = new List<string>();
        bool inQuotes = false;
        var current = new StringBuilder();

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (c == '\"')
            {
                if (inQuotes && i + 1 < line.Length && line[i + 1] == '\"')
                {
                    current.Append('\"');
                    i++;
                }
                else
                {
                    inQuotes = !inQuotes;
                }
            }
            else if (c == ',' && !inQuotes)
            {
                result.Add(current.ToString());
                current.Clear();
            }
            else
            {
                current.Append(c);
            }
        }
        result.Add(current.ToString());
        return result;
    }
}
