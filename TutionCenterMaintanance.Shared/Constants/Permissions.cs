namespace TutionCenterMaintanance.Shared.Constants;

public static class Permissions
{
    // Students
    public const string StudentsView = "Students.View";
    public const string StudentsCreate = "Students.Create";
    public const string StudentsEdit = "Students.Edit";
    public const string StudentsDelete = "Students.Delete";

    // Attendance
    public const string AttendanceView = "Attendance.View";
    public const string AttendanceMark = "Attendance.Mark";

    // Marks & Assessments
    public const string MarksView = "Marks.View";
    public const string MarksEntry = "Marks.Entry";
    public const string MarksEdit = "Marks.Edit";
    public const string AssessmentsManage = "Assessments.Manage";

    // Fees & Payments
    public const string FeesView = "Fees.View";
    public const string FeesManage = "Fees.Manage";
    public const string PaymentsRecord = "Payments.Record";
    public const string PaymentsView = "Payments.View";

    // Academic Configuration
    public const string BoardsManage = "Boards.Manage";
    public const string SubjectsManage = "Subjects.Manage";
    public const string AcademicYearsManage = "AcademicYears.Manage";
    public const string AssessmentTypesManage = "AssessmentTypes.Manage";
    public const string BatchesManage = "Batches.Manage";
    public const string TeachersManage = "Teachers.Manage";
    public const string TimetableManage = "Timetable.Manage";
    public const string HomeworkManage = "Homework.Manage";

    // System Settings & IAM
    public const string SettingsManage = "Settings.Manage";
    public const string UsersView = "Users.View";
    public const string UsersManage = "Users.Manage";
    public const string RolesManage = "Roles.Manage";

    // Advanced & Reporting
    public const string ReportsView = "Reports.View";
    public const string ReportsExport = "Reports.Export";
    public const string MessagingSend = "Messaging.Send";
    public const string AuditView = "Audit.View";
    public const string BackupManage = "Backup.Manage";
    public const string DataImport = "Data.Import";
    public const string DataExport = "Data.Export";

    public static IReadOnlyList<string> GetAll() => new[]
    {
        StudentsView, StudentsCreate, StudentsEdit, StudentsDelete,
        AttendanceView, AttendanceMark,
        MarksView, MarksEntry, MarksEdit, AssessmentsManage,
        FeesView, FeesManage, PaymentsRecord, PaymentsView,
        BoardsManage, SubjectsManage, AcademicYearsManage, AssessmentTypesManage,
        BatchesManage, TeachersManage, TimetableManage, HomeworkManage,
        SettingsManage, UsersView, UsersManage, RolesManage,
        ReportsView, ReportsExport,
        MessagingSend, AuditView, BackupManage,
        DataImport, DataExport
    };
}
