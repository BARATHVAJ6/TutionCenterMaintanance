using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using TutionCenterMaintanance.Client.Auth;
using TutionCenterMaintanance.Shared.DTOs.Academic;
using TutionCenterMaintanance.Shared.DTOs.Assessments;
using TutionCenterMaintanance.Shared.DTOs.Attendance;
using TutionCenterMaintanance.Shared.DTOs.Auth;
using TutionCenterMaintanance.Shared.DTOs.Communication;
using TutionCenterMaintanance.Shared.DTOs.Dashboard;
using TutionCenterMaintanance.Shared.DTOs.Fees;
using TutionCenterMaintanance.Shared.DTOs.Expenses;
using TutionCenterMaintanance.Shared.DTOs.Stationery;
using TutionCenterMaintanance.Shared.DTOs.Financials;
using TutionCenterMaintanance.Shared.DTOs.Notifications;
using TutionCenterMaintanance.Shared.DTOs.Settings;
using TutionCenterMaintanance.Shared.DTOs.Students;
using TutionCenterMaintanance.Shared.DTOs.Timetable;
using TutionCenterMaintanance.Shared.DTOs.Homework;
using TutionCenterMaintanance.Shared.DTOs.Reports;
using TutionCenterMaintanance.Shared.DTOs.ImportExport;
using TutionCenterMaintanance.Shared.DTOs.Audit;
using TutionCenterMaintanance.Shared.Enums;
using TutionCenterMaintanance.Shared.Models;

namespace TutionCenterMaintanance.Client.Services;

public interface IApiService
{
    // Auth
    Task<ApiResponse<LoginResponse>?> LoginAsync(LoginRequest request);
    Task<ApiResponse<bool>?> ChangePasswordAsync(ChangePasswordRequest request);
    Task<ApiResponse<List<UserDto>>?> GetUsersAsync();
    Task<ApiResponse<UserDto>?> CreateUserAsync(CreateUserRequest request);
    Task<ApiResponse<bool>?> UpdateUserAsync(string id, UpdateUserRequest request);
    Task<ApiResponse<List<RoleDto>>?> GetRolesAsync();
    Task<ApiResponse<RoleDto>?> CreateRoleAsync(CreateOrUpdateRoleRequest request);
    Task<ApiResponse<bool>?> UpdateRoleAsync(string id, CreateOrUpdateRoleRequest request);
    Task<ApiResponse<IReadOnlyList<string>>?> GetAllPermissionsAsync();

    // Academic
    Task<ApiResponse<List<BoardDto>>?> GetBoardsAsync();
    Task<ApiResponse<BoardDto>?> CreateBoardAsync(BoardDto dto);
    Task<ApiResponse<bool>?> UpdateBoardAsync(int id, BoardDto dto);
    Task<ApiResponse<bool>?> DeleteBoardAsync(int id);
    Task<ApiResponse<List<GradeDto>>?> GetGradesAsync();
    Task<ApiResponse<List<SubjectDto>>?> GetSubjectsAsync();
    Task<ApiResponse<SubjectDto>?> CreateSubjectAsync(SubjectDto dto);
    Task<ApiResponse<bool>?> UpdateSubjectAsync(int id, SubjectDto dto);
    Task<ApiResponse<bool>?> DeleteSubjectAsync(int id);
    Task<ApiResponse<List<BoardGradeSubjectDto>>?> GetBoardGradeMappingsAsync(int? boardId = null, int? gradeId = null);
    Task<ApiResponse<bool>?> SaveBoardGradeMappingsAsync(ManageBoardGradeSubjectsRequest request);
    Task<ApiResponse<List<AcademicYearDto>>?> GetAcademicYearsAsync();
    Task<ApiResponse<AcademicYearDto>?> CreateAcademicYearAsync(AcademicYearDto dto);
    Task<ApiResponse<bool>?> SetCurrentAcademicYearAsync(int id);
    Task<ApiResponse<List<BatchDto>>?> GetBatchesAsync(int? gradeId = null, int? boardId = null);
    Task<ApiResponse<BatchDto>?> CreateBatchAsync(CreateBatchRequest request);
    Task<ApiResponse<bool>?> UpdateBatchAsync(int id, CreateBatchRequest request);
    Task<ApiResponse<bool>?> DeleteBatchAsync(int id);
    Task<ApiResponse<List<TeacherDto>>?> GetTeachersAsync();
    Task<ApiResponse<TeacherDto>?> CreateTeacherAsync(TeacherDto dto);
    Task<ApiResponse<bool>?> UpdateTeacherAsync(int id, TeacherDto dto);
    Task<ApiResponse<bool>?> DeleteTeacherAsync(int id);

    // Students
    Task<ApiResponse<PaginatedResult<StudentListItemDto>>?> GetStudentsAsync(
        string? search = null, int? gradeId = null, int? boardId = null, int? batchId = null, bool? isActive = true, int page = 1, int pageSize = 15);
    Task<ApiResponse<StudentDto>?> GetStudentByIdAsync(int id);
    Task<ApiResponse<StudentDto>?> CreateStudentAsync(CreateStudentRequest request);
    Task<ApiResponse<bool>?> UpdateStudentAsync(int id, UpdateStudentRequest request);
    Task<ApiResponse<bool>?> DeleteStudentAsync(int id);

    // Assessments & Marks
    Task<ApiResponse<List<AssessmentTypeDto>>?> GetAssessmentTypesAsync();
    Task<ApiResponse<AssessmentTypeDto>?> CreateAssessmentTypeAsync(AssessmentTypeDto dto);
    Task<ApiResponse<List<AssessmentDto>>?> GetAssessmentsAsync(int? gradeId = null, int? boardId = null, int? academicYearId = null);
    Task<ApiResponse<AssessmentDto>?> CreateAssessmentAsync(CreateAssessmentRequest request);
    Task<ApiResponse<BulkMarksGridDto>?> GetMarksGridAsync(int assessmentSubjectId, int? batchId = null);
    Task<ApiResponse<bool>?> SaveBulkMarksAsync(BulkSaveMarksRequest request);
    Task<ApiResponse<StudentReportCardDto>>? GetReportCardAsync(int studentId, int? academicYearId = null);

    // Attendance
    Task<ApiResponse<BatchAttendanceSheetDto>?> GetAttendanceSheetAsync(DateTime date, int? batchId = null, int? gradeId = null);
    Task<ApiResponse<bool>?> SaveAttendanceAsync(SaveDailyAttendanceRequest request);
    Task<ApiResponse<AttendanceSummaryDto>?> GetAttendanceSummaryAsync(DateTime? date = null);

    // Fees
    Task<ApiResponse<List<StudentMonthlyStatusDto>>?> GetMonthlyStatusAsync(int? month = null, int? year = null, int? gradeId = null);
    Task<ApiResponse<bool>?> SetMonthlyFeeAsync(SetMonthlyFeeRequest request);
    Task<ApiResponse<List<FeePlanDto>>?> GetFeePlansAsync(int? gradeId = null, int? academicYearId = null);
    Task<ApiResponse<FeePlanDto>?> CreateFeePlanAsync(FeePlanDto dto);
    Task<ApiResponse<List<DiscountDto>>?> GetDiscountsAsync();
    Task<ApiResponse<DiscountDto>?> CreateDiscountAsync(DiscountDto dto);
    Task<ApiResponse<bool>?> AssignFeePlanAsync(AssignFeePlanRequest request);
    Task<ApiResponse<PaymentDto>?> RecordPaymentAsync(RecordPaymentRequest request);
    Task<ApiResponse<List<PaymentDto>>?> GetPaymentsAsync(int? studentId = null);
    Task<ApiResponse<List<StudentFeeSummaryDto>>?> GetStudentFeeSummariesAsync(int? gradeId = null, PaymentStatus? status = null);
    Task<ApiResponse<StudentFeeLedgerDto>?> GetStudentFeeLedgerAsync(int studentId);
    Task<ApiResponse<PaymentReceiptDto>?> GetPaymentReceiptAsync(int paymentId);
    Task<ApiResponse<FeeDefaultersSummaryDto>?> GetFeeDefaultersSummaryAsync();
    Task<ApiResponse<List<MonthlyCollectionDto>>?> GetMonthlyCollectionTrendAsync();
    Task<ApiResponse<List<PaymentModeBreakdownDto>>?> GetPaymentModeBreakdownAsync();
    Task<ApiResponse<List<GradeFinancialSummaryDto>>?> GetGradeFinancialSummariesAsync();

    // Expenses
    Task<ApiResponse<PaginatedResult<ExpenseDto>>?> GetExpensesAsync(int? month = null, int? year = null, ExpenseCategory? category = null, int page = 1, int pageSize = 25, CancellationToken ct = default);
    Task<ApiResponse<ExpenseDto>?> CreateExpenseAsync(CreateExpenseRequest request);
    Task<ApiResponse<bool>?> DeleteExpenseAsync(int id);

    // Stationery
    Task<ApiResponse<List<StationeryItemDto>>?> GetStationeryItemsAsync();
    Task<ApiResponse<StationeryItemDto>?> CreateStationeryItemAsync(CreateStationeryItemRequest request);
    Task<ApiResponse<bool>?> UpdateStationeryItemAsync(int id, UpdateStationeryItemRequest request);
    Task<ApiResponse<bool>?> DeleteStationeryItemAsync(int id);
    Task<ApiResponse<PaginatedResult<StationeryPurchaseDto>>?> GetStationeryPurchasesAsync(int? month = null, int? year = null, int page = 1, int pageSize = 25, CancellationToken ct = default);
    Task<ApiResponse<StationeryPurchaseDto>?> RecordStationeryPurchaseAsync(RecordStationeryPurchaseRequest request);
    Task<ApiResponse<bool>?> DeleteStationeryPurchaseAsync(int id);
    Task<ApiResponse<PaginatedResult<StationerySaleDto>>?> GetStationerySalesAsync(int? month = null, int? year = null, int? studentId = null, int page = 1, int pageSize = 25, CancellationToken ct = default);
    Task<ApiResponse<StationerySaleDto>?> RecordStationerySaleAsync(RecordStationerySaleRequest request);
    Task<ApiResponse<bool>?> DeleteStationerySaleAsync(int id);
    Task<ApiResponse<bool>?> AdjustStationeryStockAsync(QuickStockAdjustmentRequest request);

    // Financials
    Task<ApiResponse<FinancialOverviewDto>>? GetFinancialOverviewAsync(int? month = null, int? year = null);
    Task<ApiResponse<List<MonthlyFinancialTrendDto>>?> GetFinancialTrendsAsync(int? year = null);

    // Notifications
    Task<ApiResponse<List<NotificationDto>>?> GetNotificationsAsync(bool? unreadOnly = null);
    Task<ApiResponse<NotificationCountDto>?> GetUnreadNotificationCountAsync();
    Task<ApiResponse<bool>?> MarkNotificationAsReadAsync(int id);
    Task<ApiResponse<bool>?> MarkAllNotificationsAsReadAsync();
    Task<ApiResponse<int>?> GenerateSystemAlertsAsync();

    // WhatsApp Communication
    Task<ApiResponse<List<WhatsAppTemplateDto>>?> GetWhatsAppTemplatesAsync();
    Task<ApiResponse<WhatsAppTemplateDto>?> CreateWhatsAppTemplateAsync(CreateOrUpdateTemplateRequest request);
    Task<ApiResponse<bool>?> UpdateWhatsAppTemplateAsync(int id, CreateOrUpdateTemplateRequest request);
    Task<ApiResponse<bool>?> DeleteWhatsAppTemplateAsync(int id);
    Task<ApiResponse<List<WhatsAppMessageLogDto>>?> GetWhatsAppLogsAsync(int? studentId = null);
    Task<ApiResponse<bool>?> LogWhatsAppDispatchAsync(LogWhatsAppDispatchDto request);
    Task<ApiResponse<List<BulkWhatsAppRecipientDto>>?> GetOverdueFeeRecipientsAsync(int? gradeId = null);
    Task<ApiResponse<List<BulkWhatsAppRecipientDto>>?> GetAbsentStudentRecipientsAsync(DateTime? date = null);
    Task<ApiResponse<List<BulkWhatsAppRecipientDto>>?> GetAssessmentResultRecipientsAsync(int assessmentId);

    // Timetable
    Task<ApiResponse<List<TimetableSlotDto>>?> GetTimetableAsync(int? batchId = null, int? teacherId = null, DayOfWeek? dayOfWeek = null);
    Task<ApiResponse<TimetableSlotDto>?> CreateTimetableSlotAsync(CreateOrUpdateTimetableRequest request);
    Task<ApiResponse<bool>?> UpdateTimetableSlotAsync(int id, CreateOrUpdateTimetableRequest request);
    Task<ApiResponse<bool>?> DeleteTimetableSlotAsync(int id);

    // Homework
    Task<ApiResponse<List<HomeworkDto>>?> GetHomeworkListAsync(int? batchId = null);
    Task<ApiResponse<HomeworkDto>?> CreateHomeworkAsync(CreateHomeworkRequest request);
    Task<ApiResponse<HomeworkDetailDto>?> GetHomeworkSubmissionsAsync(int homeworkId);
    Task<ApiResponse<bool>?> BulkUpdateHomeworkSubmissionsAsync(int homeworkId, BulkUpdateHomeworkSubmissionsRequest request);
    Task<ApiResponse<bool>?> DeleteHomeworkAsync(int homeworkId);

    // Reports & Analytics
    Task<ApiResponse<StudentProgressTrendDto>?> GetStudentTrendAsync(int studentId);
    Task<ApiResponse<List<SubjectPerformanceComparisonDto>>?> GetSubjectComparisonAsync();
    Task<ApiResponse<List<BatchPerformanceComparisonDto>>?> GetBatchComparisonAsync();
    Task<ApiResponse<List<AttendancePerformanceCorrelationDto>>?> GetAttendanceCorrelationAsync();

    // Import/Export & Audit & Backup
    Task<ApiResponse<ImportSummaryDto>?> ImportStudentsCsvAsync(ImportStudentsRequest request);
    Task<ApiResponse<List<AuditLogDto>>?> GetAuditLogsAsync(string? entityName = null, string? actionType = null);
    Task<ApiResponse<SystemBackupBundleDto>?> GetBackupSummaryAsync();

    // Settings & Dashboard
    Task<ApiResponse<CenterSettingsDto>?> GetSettingsAsync();
    Task<ApiResponse<bool>?> UpdateSettingsAsync(CenterSettingsDto dto);
    Task<ApiResponse<DashboardStatsDto>?> GetDashboardStatsAsync();
}

public class ApiService : IApiService
{
    private static readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };

    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage;

    public ApiService(HttpClient http, ILocalStorageService localStorage)
    {
        _http = http;
        _localStorage = localStorage;
    }

    private async Task EnsureAuthHeaderAsync()
    {
        var token = await _localStorage.GetItemAsync("authToken");
        if (!string.IsNullOrWhiteSpace(token))
        {
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    // Auth
    public async Task<ApiResponse<LoginResponse>?> LoginAsync(LoginRequest request)
    {
        var res = await _http.PostAsJsonAsync("api/Auth/login", request);
        return await res.Content.ReadFromJsonAsync<ApiResponse<LoginResponse>>();
    }

    public async Task<ApiResponse<bool>?> ChangePasswordAsync(ChangePasswordRequest request)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.PostAsJsonAsync("api/Auth/change-password", request);
        return await res.Content.ReadFromJsonAsync<ApiResponse<bool>>();
    }

    public async Task<ApiResponse<List<UserDto>>?> GetUsersAsync()
    {
        await EnsureAuthHeaderAsync();
        return await _http.GetFromJsonAsync<ApiResponse<List<UserDto>>>("api/Auth/users");
    }

    public async Task<ApiResponse<UserDto>?> CreateUserAsync(CreateUserRequest request)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.PostAsJsonAsync("api/Auth/users", request);
        return await res.Content.ReadFromJsonAsync<ApiResponse<UserDto>>();
    }

    public async Task<ApiResponse<bool>?> UpdateUserAsync(string id, UpdateUserRequest request)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.PutAsJsonAsync($"api/Auth/users/{id}", request);
        return await res.Content.ReadFromJsonAsync<ApiResponse<bool>>();
    }

    public async Task<ApiResponse<List<RoleDto>>?> GetRolesAsync()
    {
        await EnsureAuthHeaderAsync();
        return await _http.GetFromJsonAsync<ApiResponse<List<RoleDto>>>("api/Auth/roles");
    }

    public async Task<ApiResponse<RoleDto>?> CreateRoleAsync(CreateOrUpdateRoleRequest request)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.PostAsJsonAsync("api/Auth/roles", request);
        return await res.Content.ReadFromJsonAsync<ApiResponse<RoleDto>>();
    }

    public async Task<ApiResponse<bool>?> UpdateRoleAsync(string id, CreateOrUpdateRoleRequest request)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.PutAsJsonAsync($"api/Auth/roles/{id}", request);
        return await res.Content.ReadFromJsonAsync<ApiResponse<bool>>();
    }

    public async Task<ApiResponse<IReadOnlyList<string>>?> GetAllPermissionsAsync()
    {
        await EnsureAuthHeaderAsync();
        return await _http.GetFromJsonAsync<ApiResponse<IReadOnlyList<string>>>("api/Auth/permissions");
    }

    // Academic
    public async Task<ApiResponse<List<BoardDto>>?> GetBoardsAsync()
    {
        await EnsureAuthHeaderAsync();
        return await _http.GetFromJsonAsync<ApiResponse<List<BoardDto>>>("api/Academic/boards");
    }

    public async Task<ApiResponse<BoardDto>?> CreateBoardAsync(BoardDto dto)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.PostAsJsonAsync("api/Academic/boards", dto);
        return await res.Content.ReadFromJsonAsync<ApiResponse<BoardDto>>();
    }

    public async Task<ApiResponse<bool>?> UpdateBoardAsync(int id, BoardDto dto)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.PutAsJsonAsync($"api/Academic/boards/{id}", dto);
        return await res.Content.ReadFromJsonAsync<ApiResponse<bool>>();
    }

    public async Task<ApiResponse<bool>?> DeleteBoardAsync(int id)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.DeleteAsync($"api/Academic/boards/{id}");
        return await res.Content.ReadFromJsonAsync<ApiResponse<bool>>();
    }

    public async Task<ApiResponse<List<GradeDto>>?> GetGradesAsync()
    {
        await EnsureAuthHeaderAsync();
        return await _http.GetFromJsonAsync<ApiResponse<List<GradeDto>>>("api/Academic/grades");
    }

    public async Task<ApiResponse<List<SubjectDto>>?> GetSubjectsAsync()
    {
        await EnsureAuthHeaderAsync();
        return await _http.GetFromJsonAsync<ApiResponse<List<SubjectDto>>>("api/Academic/subjects");
    }

    public async Task<ApiResponse<SubjectDto>?> CreateSubjectAsync(SubjectDto dto)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.PostAsJsonAsync("api/Academic/subjects", dto);
        return await res.Content.ReadFromJsonAsync<ApiResponse<SubjectDto>>();
    }

    public async Task<ApiResponse<bool>?> UpdateSubjectAsync(int id, SubjectDto dto)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.PutAsJsonAsync($"api/Academic/subjects/{id}", dto);
        return await res.Content.ReadFromJsonAsync<ApiResponse<bool>>();
    }

    public async Task<ApiResponse<bool>?> DeleteSubjectAsync(int id)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.DeleteAsync($"api/Academic/subjects/{id}");
        return await res.Content.ReadFromJsonAsync<ApiResponse<bool>>();
    }

    public async Task<ApiResponse<List<BoardGradeSubjectDto>>?> GetBoardGradeMappingsAsync(int? boardId = null, int? gradeId = null)
    {
        await EnsureAuthHeaderAsync();
        return await _http.GetFromJsonAsync<ApiResponse<List<BoardGradeSubjectDto>>>($"api/Academic/mappings?boardId={boardId}&gradeId={gradeId}");
    }

    public async Task<ApiResponse<bool>?> SaveBoardGradeMappingsAsync(ManageBoardGradeSubjectsRequest request)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.PostAsJsonAsync("api/Academic/mappings", request);
        return await res.Content.ReadFromJsonAsync<ApiResponse<bool>>();
    }

    public async Task<ApiResponse<List<AcademicYearDto>>?> GetAcademicYearsAsync()
    {
        await EnsureAuthHeaderAsync();
        return await _http.GetFromJsonAsync<ApiResponse<List<AcademicYearDto>>>("api/Academic/years");
    }

    public async Task<ApiResponse<AcademicYearDto>?> CreateAcademicYearAsync(AcademicYearDto dto)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.PostAsJsonAsync("api/Academic/years", dto);
        return await res.Content.ReadFromJsonAsync<ApiResponse<AcademicYearDto>>();
    }

    public async Task<ApiResponse<bool>?> SetCurrentAcademicYearAsync(int id)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.PostAsync($"api/Academic/years/{id}/set-current", null);
        return await res.Content.ReadFromJsonAsync<ApiResponse<bool>>();
    }

    public async Task<ApiResponse<List<BatchDto>>?> GetBatchesAsync(int? gradeId = null, int? boardId = null)
    {
        await EnsureAuthHeaderAsync();
        return await _http.GetFromJsonAsync<ApiResponse<List<BatchDto>>>($"api/Academic/batches?gradeId={gradeId}&boardId={boardId}");
    }

    public async Task<ApiResponse<BatchDto>?> CreateBatchAsync(CreateBatchRequest request)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.PostAsJsonAsync("api/Academic/batches", request);
        return await res.Content.ReadFromJsonAsync<ApiResponse<BatchDto>>();
    }

    public async Task<ApiResponse<bool>?> UpdateBatchAsync(int id, CreateBatchRequest request)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.PutAsJsonAsync($"api/Academic/batches/{id}", request);
        return await res.Content.ReadFromJsonAsync<ApiResponse<bool>>();
    }

    public async Task<ApiResponse<bool>?> DeleteBatchAsync(int id)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.DeleteAsync($"api/Academic/batches/{id}");
        return await res.Content.ReadFromJsonAsync<ApiResponse<bool>>();
    }

    public async Task<ApiResponse<List<TeacherDto>>?> GetTeachersAsync()
    {
        await EnsureAuthHeaderAsync();
        return await _http.GetFromJsonAsync<ApiResponse<List<TeacherDto>>>("api/Academic/teachers");
    }

    public async Task<ApiResponse<TeacherDto>?> CreateTeacherAsync(TeacherDto dto)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.PostAsJsonAsync("api/Academic/teachers", dto);
        return await res.Content.ReadFromJsonAsync<ApiResponse<TeacherDto>>();
    }

    public async Task<ApiResponse<bool>?> UpdateTeacherAsync(int id, TeacherDto dto)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.PutAsJsonAsync($"api/Academic/teachers/{id}", dto);
        return await res.Content.ReadFromJsonAsync<ApiResponse<bool>>();
    }

    public async Task<ApiResponse<bool>?> DeleteTeacherAsync(int id)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.DeleteAsync($"api/Academic/teachers/{id}");
        return await res.Content.ReadFromJsonAsync<ApiResponse<bool>>();
    }

    // Students
    public async Task<ApiResponse<PaginatedResult<StudentListItemDto>>?> GetStudentsAsync(
        string? search = null, int? gradeId = null, int? boardId = null, int? batchId = null, bool? isActive = true, int page = 1, int pageSize = 15)
    {
        await EnsureAuthHeaderAsync();
        var url = $"api/Students?search={Uri.EscapeDataString(search ?? "")}&gradeId={gradeId}&boardId={boardId}&batchId={batchId}&isActive={isActive}&page={page}&pageSize={pageSize}";
        return await _http.GetFromJsonAsync<ApiResponse<PaginatedResult<StudentListItemDto>>>(url, _jsonOptions);
    }

    public async Task<ApiResponse<StudentDto>?> GetStudentByIdAsync(int id)
    {
        await EnsureAuthHeaderAsync();
        return await _http.GetFromJsonAsync<ApiResponse<StudentDto>>($"api/Students/{id}", _jsonOptions);
    }

    public async Task<ApiResponse<StudentDto>?> CreateStudentAsync(CreateStudentRequest request)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.PostAsJsonAsync("api/Students", request, _jsonOptions);
        return await res.Content.ReadFromJsonAsync<ApiResponse<StudentDto>>(_jsonOptions);
    }

    public async Task<ApiResponse<bool>?> UpdateStudentAsync(int id, UpdateStudentRequest request)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.PutAsJsonAsync($"api/Students/{id}", request, _jsonOptions);
        return await res.Content.ReadFromJsonAsync<ApiResponse<bool>>(_jsonOptions);
    }

    public async Task<ApiResponse<bool>?> DeleteStudentAsync(int id)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.DeleteAsync($"api/Students/{id}");
        return await res.Content.ReadFromJsonAsync<ApiResponse<bool>>();
    }

    // Assessments & Marks
    public async Task<ApiResponse<List<AssessmentTypeDto>>?> GetAssessmentTypesAsync()
    {
        await EnsureAuthHeaderAsync();
        return await _http.GetFromJsonAsync<ApiResponse<List<AssessmentTypeDto>>>("api/Assessments/types");
    }

    public async Task<ApiResponse<AssessmentTypeDto>?> CreateAssessmentTypeAsync(AssessmentTypeDto dto)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.PostAsJsonAsync("api/Assessments/types", dto);
        return await res.Content.ReadFromJsonAsync<ApiResponse<AssessmentTypeDto>>();
    }

    public async Task<ApiResponse<List<AssessmentDto>>?> GetAssessmentsAsync(int? gradeId = null, int? boardId = null, int? academicYearId = null)
    {
        await EnsureAuthHeaderAsync();
        return await _http.GetFromJsonAsync<ApiResponse<List<AssessmentDto>>>($"api/Assessments?gradeId={gradeId}&boardId={boardId}&academicYearId={academicYearId}");
    }

    public async Task<ApiResponse<AssessmentDto>?> CreateAssessmentAsync(CreateAssessmentRequest request)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.PostAsJsonAsync("api/Assessments", request);
        return await res.Content.ReadFromJsonAsync<ApiResponse<AssessmentDto>>();
    }

    public async Task<ApiResponse<BulkMarksGridDto>?> GetMarksGridAsync(int assessmentSubjectId, int? batchId = null)
    {
        await EnsureAuthHeaderAsync();
        return await _http.GetFromJsonAsync<ApiResponse<BulkMarksGridDto>>($"api/Marks/grid/{assessmentSubjectId}?batchId={batchId}");
    }

    public async Task<ApiResponse<bool>?> SaveBulkMarksAsync(BulkSaveMarksRequest request)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.PostAsJsonAsync("api/Marks/bulk-save", request);
        return await res.Content.ReadFromJsonAsync<ApiResponse<bool>>();
    }

    public async Task<ApiResponse<StudentReportCardDto>>? GetReportCardAsync(int studentId, int? academicYearId = null)
    {
        await EnsureAuthHeaderAsync();
        return await _http.GetFromJsonAsync<ApiResponse<StudentReportCardDto>>($"api/Marks/report-card/{studentId}?academicYearId={academicYearId}");
    }

    // Attendance
    public async Task<ApiResponse<BatchAttendanceSheetDto>?> GetAttendanceSheetAsync(DateTime date, int? batchId = null, int? gradeId = null)
    {
        await EnsureAuthHeaderAsync();
        var dateStr = date.ToString("yyyy-MM-dd");
        return await _http.GetFromJsonAsync<ApiResponse<BatchAttendanceSheetDto>>($"api/Attendance/sheet?date={dateStr}&batchId={batchId}&gradeId={gradeId}");
    }

    public async Task<ApiResponse<bool>?> SaveAttendanceAsync(SaveDailyAttendanceRequest request)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.PostAsJsonAsync("api/Attendance/save", request);
        return await res.Content.ReadFromJsonAsync<ApiResponse<bool>>();
    }

    public async Task<ApiResponse<AttendanceSummaryDto>?> GetAttendanceSummaryAsync(DateTime? date = null)
    {
        await EnsureAuthHeaderAsync();
        var dateStr = date?.ToString("yyyy-MM-dd") ?? "";
        return await _http.GetFromJsonAsync<ApiResponse<AttendanceSummaryDto>>($"api/Attendance/summary?date={dateStr}");
    }

    // Fees
    public async Task<ApiResponse<List<FeePlanDto>>?> GetFeePlansAsync(int? gradeId = null, int? academicYearId = null)
    {
        await EnsureAuthHeaderAsync();
        return await _http.GetFromJsonAsync<ApiResponse<List<FeePlanDto>>>($"api/Fees/plans?gradeId={gradeId}&academicYearId={academicYearId}");
    }

    public async Task<ApiResponse<FeePlanDto>?> CreateFeePlanAsync(FeePlanDto dto)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.PostAsJsonAsync("api/Fees/plans", dto);
        return await res.Content.ReadFromJsonAsync<ApiResponse<FeePlanDto>>();
    }

    public async Task<ApiResponse<List<DiscountDto>>?> GetDiscountsAsync()
    {
        await EnsureAuthHeaderAsync();
        return await _http.GetFromJsonAsync<ApiResponse<List<DiscountDto>>>("api/Fees/discounts");
    }

    public async Task<ApiResponse<DiscountDto>?> CreateDiscountAsync(DiscountDto dto)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.PostAsJsonAsync("api/Fees/discounts", dto);
        return await res.Content.ReadFromJsonAsync<ApiResponse<DiscountDto>>();
    }

    public async Task<ApiResponse<bool>?> AssignFeePlanAsync(AssignFeePlanRequest request)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.PostAsJsonAsync("api/Fees/assign", request);
        return await res.Content.ReadFromJsonAsync<ApiResponse<bool>>();
    }

    public async Task<ApiResponse<PaymentDto>?> RecordPaymentAsync(RecordPaymentRequest request)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.PostAsJsonAsync("api/Fees/pay", request);
        return await res.Content.ReadFromJsonAsync<ApiResponse<PaymentDto>>();
    }

    public async Task<ApiResponse<List<PaymentDto>>?> GetPaymentsAsync(int? studentId = null)
    {
        await EnsureAuthHeaderAsync();
        return await _http.GetFromJsonAsync<ApiResponse<List<PaymentDto>>>($"api/Fees/payments?studentId={studentId}");
    }

    public async Task<ApiResponse<List<StudentFeeSummaryDto>>?> GetStudentFeeSummariesAsync(int? gradeId = null, PaymentStatus? status = null)
    {
        await EnsureAuthHeaderAsync();
        return await _http.GetFromJsonAsync<ApiResponse<List<StudentFeeSummaryDto>>>($"api/Fees/student-summaries?gradeId={gradeId}&status={status}");
    }

    public async Task<ApiResponse<StudentFeeLedgerDto>?> GetStudentFeeLedgerAsync(int studentId)
    {
        await EnsureAuthHeaderAsync();
        return await _http.GetFromJsonAsync<ApiResponse<StudentFeeLedgerDto>>($"api/Fees/ledger/{studentId}");
    }

    public async Task<ApiResponse<PaymentReceiptDto>?> GetPaymentReceiptAsync(int paymentId)
    {
        await EnsureAuthHeaderAsync();
        return await _http.GetFromJsonAsync<ApiResponse<PaymentReceiptDto>>($"api/Fees/receipt/{paymentId}");
    }

    public async Task<ApiResponse<FeeDefaultersSummaryDto>?> GetFeeDefaultersSummaryAsync()
    {
        await EnsureAuthHeaderAsync();
        return await _http.GetFromJsonAsync<ApiResponse<FeeDefaultersSummaryDto>>("api/Fees/defaulters/summary");
    }

    public async Task<ApiResponse<List<MonthlyCollectionDto>>?> GetMonthlyCollectionTrendAsync()
    {
        await EnsureAuthHeaderAsync();
        return await _http.GetFromJsonAsync<ApiResponse<List<MonthlyCollectionDto>>>("api/Fees/analytics/monthly-trend");
    }

    public async Task<ApiResponse<List<PaymentModeBreakdownDto>>?> GetPaymentModeBreakdownAsync()
    {
        await EnsureAuthHeaderAsync();
        return await _http.GetFromJsonAsync<ApiResponse<List<PaymentModeBreakdownDto>>>("api/Fees/analytics/payment-modes");
    }

    public async Task<ApiResponse<List<GradeFinancialSummaryDto>>?> GetGradeFinancialSummariesAsync()
    {
        await EnsureAuthHeaderAsync();
        return await _http.GetFromJsonAsync<ApiResponse<List<GradeFinancialSummaryDto>>>("api/Fees/analytics/grades");
    }

    public async Task<ApiResponse<List<StudentMonthlyStatusDto>>?> GetMonthlyStatusAsync(int? month = null, int? year = null, int? gradeId = null)
    {
        await EnsureAuthHeaderAsync();
        return await _http.GetFromJsonAsync<ApiResponse<List<StudentMonthlyStatusDto>>>($"api/Fees/monthly-status?month={month}&year={year}&gradeId={gradeId}");
    }

    public async Task<ApiResponse<bool>?> SetMonthlyFeeAsync(SetMonthlyFeeRequest request)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.PutAsJsonAsync("api/Fees/monthly-fee", request);
        return await res.Content.ReadFromJsonAsync<ApiResponse<bool>>();
    }

    // Expenses
    public async Task<ApiResponse<PaginatedResult<ExpenseDto>>?> GetExpensesAsync(
        int? month = null,
        int? year = null,
        ExpenseCategory? category = null,
        int page = 1,
        int pageSize = 25,
        CancellationToken ct = default)
    {
        await EnsureAuthHeaderAsync();
        return await _http.GetFromJsonAsync<ApiResponse<PaginatedResult<ExpenseDto>>>($"api/Expenses?month={month}&year={year}&category={category}&page={page}&pageSize={pageSize}", ct);
    }

    public async Task<ApiResponse<ExpenseDto>?> CreateExpenseAsync(CreateExpenseRequest request)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.PostAsJsonAsync("api/Expenses", request);
        return await res.Content.ReadFromJsonAsync<ApiResponse<ExpenseDto>>();
    }

    public async Task<ApiResponse<bool>?> DeleteExpenseAsync(int id)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.DeleteAsync($"api/Expenses/{id}");
        return await res.Content.ReadFromJsonAsync<ApiResponse<bool>>();
    }

    // Stationery
    public async Task<ApiResponse<List<StationeryItemDto>>?> GetStationeryItemsAsync()
    {
        await EnsureAuthHeaderAsync();
        return await _http.GetFromJsonAsync<ApiResponse<List<StationeryItemDto>>>("api/Stationery/items");
    }

    public async Task<ApiResponse<StationeryItemDto>?> CreateStationeryItemAsync(CreateStationeryItemRequest request)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.PostAsJsonAsync("api/Stationery/items", request);
        return await res.Content.ReadFromJsonAsync<ApiResponse<StationeryItemDto>>();
    }

    public async Task<ApiResponse<bool>?> UpdateStationeryItemAsync(int id, UpdateStationeryItemRequest request)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.PutAsJsonAsync($"api/Stationery/items/{id}", request);
        return await res.Content.ReadFromJsonAsync<ApiResponse<bool>>();
    }

    public async Task<ApiResponse<bool>?> DeleteStationeryItemAsync(int id)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.DeleteAsync($"api/Stationery/items/{id}");
        return await res.Content.ReadFromJsonAsync<ApiResponse<bool>>();
    }

    public async Task<ApiResponse<PaginatedResult<StationeryPurchaseDto>>?> GetStationeryPurchasesAsync(
        int? month = null,
        int? year = null,
        int page = 1,
        int pageSize = 25,
        CancellationToken ct = default)
    {
        await EnsureAuthHeaderAsync();
        return await _http.GetFromJsonAsync<ApiResponse<PaginatedResult<StationeryPurchaseDto>>>($"api/Stationery/purchases?month={month}&year={year}&page={page}&pageSize={pageSize}", ct);
    }

    public async Task<ApiResponse<StationeryPurchaseDto>?> RecordStationeryPurchaseAsync(RecordStationeryPurchaseRequest request)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.PostAsJsonAsync("api/Stationery/purchases", request);
        return await res.Content.ReadFromJsonAsync<ApiResponse<StationeryPurchaseDto>>();
    }

    public async Task<ApiResponse<bool>?> DeleteStationeryPurchaseAsync(int id)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.DeleteAsync($"api/Stationery/purchases/{id}");
        return await res.Content.ReadFromJsonAsync<ApiResponse<bool>>();
    }

    public async Task<ApiResponse<PaginatedResult<StationerySaleDto>>?> GetStationerySalesAsync(
        int? month = null,
        int? year = null,
        int? studentId = null,
        int page = 1,
        int pageSize = 25,
        CancellationToken ct = default)
    {
        await EnsureAuthHeaderAsync();
        return await _http.GetFromJsonAsync<ApiResponse<PaginatedResult<StationerySaleDto>>>($"api/Stationery/sales?month={month}&year={year}&studentId={studentId}&page={page}&pageSize={pageSize}", ct);
    }

    public async Task<ApiResponse<StationerySaleDto>?> RecordStationerySaleAsync(RecordStationerySaleRequest request)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.PostAsJsonAsync("api/Stationery/sales", request);
        return await res.Content.ReadFromJsonAsync<ApiResponse<StationerySaleDto>>();
    }

    public async Task<ApiResponse<bool>?> DeleteStationerySaleAsync(int id)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.DeleteAsync($"api/Stationery/sales/{id}");
        return await res.Content.ReadFromJsonAsync<ApiResponse<bool>>();
    }

    public async Task<ApiResponse<bool>?> AdjustStationeryStockAsync(QuickStockAdjustmentRequest request)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.PostAsJsonAsync("api/Stationery/adjust-stock", request);
        return await res.Content.ReadFromJsonAsync<ApiResponse<bool>>();
    }

    // Financials
    public async Task<ApiResponse<FinancialOverviewDto>>? GetFinancialOverviewAsync(int? month = null, int? year = null)
    {
        await EnsureAuthHeaderAsync();
        return await _http.GetFromJsonAsync<ApiResponse<FinancialOverviewDto>>($"api/Financials/overview?month={month}&year={year}");
    }

    public async Task<ApiResponse<List<MonthlyFinancialTrendDto>>?> GetFinancialTrendsAsync(int? year = null)
    {
        await EnsureAuthHeaderAsync();
        return await _http.GetFromJsonAsync<ApiResponse<List<MonthlyFinancialTrendDto>>>($"api/Financials/trends?year={year}");
    }

    // Notifications
    public async Task<ApiResponse<List<NotificationDto>>?> GetNotificationsAsync(bool? unreadOnly = null)
    {
        await EnsureAuthHeaderAsync();
        return await _http.GetFromJsonAsync<ApiResponse<List<NotificationDto>>>($"api/Notifications?unreadOnly={unreadOnly}");
    }

    public async Task<ApiResponse<NotificationCountDto>?> GetUnreadNotificationCountAsync()
    {
        await EnsureAuthHeaderAsync();
        return await _http.GetFromJsonAsync<ApiResponse<NotificationCountDto>>("api/Notifications/unread-count");
    }

    public async Task<ApiResponse<bool>?> MarkNotificationAsReadAsync(int id)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.PutAsync($"api/Notifications/{id}/read", null);
        return await res.Content.ReadFromJsonAsync<ApiResponse<bool>>();
    }

    public async Task<ApiResponse<bool>?> MarkAllNotificationsAsReadAsync()
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.PostAsync("api/Notifications/read-all", null);
        return await res.Content.ReadFromJsonAsync<ApiResponse<bool>>();
    }

    public async Task<ApiResponse<int>?> GenerateSystemAlertsAsync()
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.PostAsync("api/Notifications/generate-system-alerts", null);
        return await res.Content.ReadFromJsonAsync<ApiResponse<int>>();
    }

    // WhatsApp Communication
    public async Task<ApiResponse<List<WhatsAppTemplateDto>>?> GetWhatsAppTemplatesAsync()
    {
        await EnsureAuthHeaderAsync();
        return await _http.GetFromJsonAsync<ApiResponse<List<WhatsAppTemplateDto>>>("api/WhatsApp/templates");
    }

    public async Task<ApiResponse<WhatsAppTemplateDto>?> CreateWhatsAppTemplateAsync(CreateOrUpdateTemplateRequest request)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.PostAsJsonAsync("api/WhatsApp/templates", request);
        return await res.Content.ReadFromJsonAsync<ApiResponse<WhatsAppTemplateDto>>();
    }

    public async Task<ApiResponse<bool>?> UpdateWhatsAppTemplateAsync(int id, CreateOrUpdateTemplateRequest request)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.PutAsJsonAsync($"api/WhatsApp/templates/{id}", request);
        return await res.Content.ReadFromJsonAsync<ApiResponse<bool>>();
    }

    public async Task<ApiResponse<bool>?> DeleteWhatsAppTemplateAsync(int id)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.DeleteAsync($"api/WhatsApp/templates/{id}");
        return await res.Content.ReadFromJsonAsync<ApiResponse<bool>>();
    }

    public async Task<ApiResponse<List<WhatsAppMessageLogDto>>?> GetWhatsAppLogsAsync(int? studentId = null)
    {
        await EnsureAuthHeaderAsync();
        return await _http.GetFromJsonAsync<ApiResponse<List<WhatsAppMessageLogDto>>>($"api/WhatsApp/logs?studentId={studentId}");
    }

    public async Task<ApiResponse<bool>?> LogWhatsAppDispatchAsync(LogWhatsAppDispatchDto request)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.PostAsJsonAsync("api/WhatsApp/logs", request);
        return await res.Content.ReadFromJsonAsync<ApiResponse<bool>>();
    }

    public async Task<ApiResponse<List<BulkWhatsAppRecipientDto>>?> GetOverdueFeeRecipientsAsync(int? gradeId = null)
    {
        await EnsureAuthHeaderAsync();
        return await _http.GetFromJsonAsync<ApiResponse<List<BulkWhatsAppRecipientDto>>>($"api/WhatsApp/targets/overdue?gradeId={gradeId}");
    }

    public async Task<ApiResponse<List<BulkWhatsAppRecipientDto>>?> GetAbsentStudentRecipientsAsync(DateTime? date = null)
    {
        await EnsureAuthHeaderAsync();
        var dateStr = date?.ToString("yyyy-MM-dd") ?? "";
        return await _http.GetFromJsonAsync<ApiResponse<List<BulkWhatsAppRecipientDto>>>($"api/WhatsApp/targets/absent?date={dateStr}");
    }

    public async Task<ApiResponse<List<BulkWhatsAppRecipientDto>>?> GetAssessmentResultRecipientsAsync(int assessmentId)
    {
        await EnsureAuthHeaderAsync();
        return await _http.GetFromJsonAsync<ApiResponse<List<BulkWhatsAppRecipientDto>>>($"api/WhatsApp/targets/assessment/{assessmentId}");
    }

    // Timetable
    public async Task<ApiResponse<List<TimetableSlotDto>>?> GetTimetableAsync(int? batchId = null, int? teacherId = null, DayOfWeek? dayOfWeek = null)
    {
        await EnsureAuthHeaderAsync();
        return await _http.GetFromJsonAsync<ApiResponse<List<TimetableSlotDto>>>($"api/Timetable?batchId={batchId}&teacherId={teacherId}&dayOfWeek={dayOfWeek}");
    }

    public async Task<ApiResponse<TimetableSlotDto>?> CreateTimetableSlotAsync(CreateOrUpdateTimetableRequest request)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.PostAsJsonAsync("api/Timetable", request);
        return await res.Content.ReadFromJsonAsync<ApiResponse<TimetableSlotDto>>();
    }

    public async Task<ApiResponse<bool>?> UpdateTimetableSlotAsync(int id, CreateOrUpdateTimetableRequest request)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.PutAsJsonAsync($"api/Timetable/{id}", request);
        return await res.Content.ReadFromJsonAsync<ApiResponse<bool>>();
    }

    public async Task<ApiResponse<bool>?> DeleteTimetableSlotAsync(int id)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.DeleteAsync($"api/Timetable/{id}");
        return await res.Content.ReadFromJsonAsync<ApiResponse<bool>>();
    }

    // Homework
    public async Task<ApiResponse<List<HomeworkDto>>?> GetHomeworkListAsync(int? batchId = null)
    {
        await EnsureAuthHeaderAsync();
        return await _http.GetFromJsonAsync<ApiResponse<List<HomeworkDto>>>($"api/Homework?batchId={batchId}");
    }

    public async Task<ApiResponse<HomeworkDto>?> CreateHomeworkAsync(CreateHomeworkRequest request)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.PostAsJsonAsync("api/Homework", request);
        return await res.Content.ReadFromJsonAsync<ApiResponse<HomeworkDto>>();
    }

    public async Task<ApiResponse<HomeworkDetailDto>?> GetHomeworkSubmissionsAsync(int homeworkId)
    {
        await EnsureAuthHeaderAsync();
        return await _http.GetFromJsonAsync<ApiResponse<HomeworkDetailDto>>($"api/Homework/{homeworkId}/submissions");
    }

    public async Task<ApiResponse<bool>?> BulkUpdateHomeworkSubmissionsAsync(int homeworkId, BulkUpdateHomeworkSubmissionsRequest request)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.PostAsJsonAsync($"api/Homework/{homeworkId}/submissions", request);
        return await res.Content.ReadFromJsonAsync<ApiResponse<bool>>();
    }

    public async Task<ApiResponse<bool>?> DeleteHomeworkAsync(int homeworkId)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.DeleteAsync($"api/Homework/{homeworkId}");
        return await res.Content.ReadFromJsonAsync<ApiResponse<bool>>();
    }

    // Reports & Analytics
    public async Task<ApiResponse<StudentProgressTrendDto>?> GetStudentTrendAsync(int studentId)
    {
        await EnsureAuthHeaderAsync();
        return await _http.GetFromJsonAsync<ApiResponse<StudentProgressTrendDto>>($"api/Reports/student-trend/{studentId}");
    }

    public async Task<ApiResponse<List<SubjectPerformanceComparisonDto>>?> GetSubjectComparisonAsync()
    {
        await EnsureAuthHeaderAsync();
        return await _http.GetFromJsonAsync<ApiResponse<List<SubjectPerformanceComparisonDto>>>("api/Reports/subject-comparison");
    }

    public async Task<ApiResponse<List<BatchPerformanceComparisonDto>>?> GetBatchComparisonAsync()
    {
        await EnsureAuthHeaderAsync();
        return await _http.GetFromJsonAsync<ApiResponse<List<BatchPerformanceComparisonDto>>>("api/Reports/batch-comparison");
    }

    public async Task<ApiResponse<List<AttendancePerformanceCorrelationDto>>?> GetAttendanceCorrelationAsync()
    {
        await EnsureAuthHeaderAsync();
        return await _http.GetFromJsonAsync<ApiResponse<List<AttendancePerformanceCorrelationDto>>>("api/Reports/attendance-correlation");
    }

    // Import/Export & Audit & Backup
    public async Task<ApiResponse<ImportSummaryDto>?> ImportStudentsCsvAsync(ImportStudentsRequest request)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.PostAsJsonAsync("api/ImportExport/students/import", request);
        return await res.Content.ReadFromJsonAsync<ApiResponse<ImportSummaryDto>>();
    }

    public async Task<ApiResponse<List<AuditLogDto>>?> GetAuditLogsAsync(string? entityName = null, string? actionType = null)
    {
        await EnsureAuthHeaderAsync();
        return await _http.GetFromJsonAsync<ApiResponse<List<AuditLogDto>>>($"api/Audit?entityName={entityName}&actionType={actionType}");
    }

    public async Task<ApiResponse<SystemBackupBundleDto>?> GetBackupSummaryAsync()
    {
        await EnsureAuthHeaderAsync();
        return await _http.GetFromJsonAsync<ApiResponse<SystemBackupBundleDto>>("api/Backup/summary");
    }

    // Settings & Dashboard
    public async Task<ApiResponse<CenterSettingsDto>?> GetSettingsAsync()
    {
        return await _http.GetFromJsonAsync<ApiResponse<CenterSettingsDto>>("api/Settings");
    }

    public async Task<ApiResponse<bool>?> UpdateSettingsAsync(CenterSettingsDto dto)
    {
        await EnsureAuthHeaderAsync();
        var res = await _http.PutAsJsonAsync("api/Settings", dto);
        return await res.Content.ReadFromJsonAsync<ApiResponse<bool>>();
    }

    public async Task<ApiResponse<DashboardStatsDto>?> GetDashboardStatsAsync()
    {
        await EnsureAuthHeaderAsync();
        return await _http.GetFromJsonAsync<ApiResponse<DashboardStatsDto>>("api/Dashboard/stats");
    }
}
