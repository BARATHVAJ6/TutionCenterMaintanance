# Graph Report - TutionCenterMaintanance  (2026-11-17)

## Corpus Check
- 116 files · ~67,427 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 6 file(s) not represented in the graph (top: .css 3, (none) 2, .db 1)

## Summary
- 3009 nodes · 5167 edges · 155 communities (135 shown, 20 thin omitted)
- Extraction: 95% EXTRACTED · 5% INFERRED · 0% AMBIGUOUS · INFERRED: 259 edges (avg confidence: 0.87)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `adcf709c`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- Task
- TutionCenterMaintanance.Client/_Imports.razor
- WhatsAppController
- FeesPage.razor
- WhatsAppQuickMessage.razor
- HomeworkPage.razor
- DashboardStatsDto
- TimetableSlotDto
- Batch
- MainLayout.razor
- StudentList.razor
- BatchesPage.razor
- 20261027154545_InitialCreate.Designer.cs
- TimetablePage.razor
- FinancialsPage.razor
- UserManagementPage.razor
- RoleDto
- .ImportStudents
- Homework
- AcademicConfigPage.razor
- AuditLogDto
- BulkMarksEntry.razor
- ReportsPage.razor
- TeachersPage.razor
- BatchAttendanceSheetDto
- AttendancePage.razor
- AssessmentsPage.razor
- AppDbContext
- StudentLedgerDialog.razor
- CreateAssessmentDialog.razor
- .Ok
- StudentDialog.razor
- FeeDefaulterItemDto
- AuditLogsPage.razor
- BatchDto
- ExpenseDto
- CustomAuthStateProvider
- .OnModelCreating
- Student
- PaymentReceiptDto
- NotificationDto
- ReportCardPage.razor
- RecordPaymentDialog.razor
- ApiResponse
- BaseEntity
- Payment
- StudentDto
- TutionCenterMaintanance.csproj
- DataManagementPage.razor
- PaymentReceiptDialog.razor
- Login.razor
- Home.razor
- Attendance
- CreateStudentRequest
- HomeworkDto
- http
- RecordPaymentRequest
- FeePlanDto
- DynamicPermissionPolicyProvider
- AssessmentTypeDto
- FeeInstallmentDto
- CenterSettingsPage.razor
- .CreateStudent
- AssessmentDto
- StationerySale
- PaymentDto
- StudentFeeLedgerDto
- StudentFeeSummaryDto
- FinancialOverviewDto
- CreateAssessmentRequest
- AppUser
- PaginatedResult
- TeacherDto
- TutionCenterMaintanance.Data.Entities
- BulkMarksGridDto
- StudentListItemDto
- Timetable
- ReportCardExamSectionDto
- PaymentMode
- SubjectMarkDetailDto
- Routes.razor
- ControllerBase
- AcademicYearDto
- BoardDto
- RecordSaleDialog.razor
- SubjectDto
- Components/_Imports.razor
- ParentInfo
- BatchPerformanceComparisonDto
- DiscountDto
- LoginResponse
- AuthController
- BoardGradeSubjectDto
- StudentReportCardDto
- ChangePasswordRequest
- .GetStudentReportCard
- TutionCenterMaintanance/Program.cs
- RecordPurchaseDialog.razor
- BulkSaveMarksRequest
- .InitializeAsync
- GradeFinancialSummaryDto
- AddExpenseDialog.razor
- UserDto
- ParentInfoDto
- AddStationeryItemDialog.razor
- .BuildModel
- BulkWhatsAppRecipientDto
- StationeryController
- AssessmentSubjectDto
- .GetGradesAsync
- CenterSettings
- NotificationsController
- MonthlyCollectionDto
- App.razor
- AdjustStockDialog.razor
- NavMenu.razor
- Error.razor
- Gender
- Counter.razor
- Weather.razor
- UpdateStudentRequest
- StudentMonthlyStatusDto
- SetMonthlyFeeDialog.razor
- WhatsAppTemplateDto
- .RecordPayment
- UpcomingAssessmentDto
- FeeDefaultersSummaryDto
- StationeryItemDto
- ManageBoardGradeSubjectsRequest
- StationerySaleDto
- .GetStats
- WhatsAppMessageLogDto
- MonthlyFinancialTrendDto
- StationeryPurchaseDto
- CreateStationeryItemRequest
- WhatsAppMessageLog
- AssessmentScoreTrendItemDto
- CenterSettingsDto
- CustomAuthStateProvider.cs
- RecordStationeryPurchaseRequest
- .GetAttendanceCorrelation
- AttendancePerformanceCorrelationDto
- SubjectPerformanceComparisonDto
- .GetOverview
- StudentProgressTrendDto
- RecordStationerySaleRequest
- UpdateUserRequest
- LogWhatsAppDispatchDto
- ExpenseCategoryBreakdownDto
- .SetMonthlyFeeAsync
- WhatsAppTemplate

## God Nodes (most connected - your core abstractions)
1. `ApiResponse` - 332 edges
2. `ApiService` - 117 edges
3. `IApiService` - 113 edges
4. `AppDbContext` - 89 edges
5. `Student` - 39 edges
6. `BaseEntity` - 35 edges
7. `StudentDto` - 33 edges
8. `FinancialOverviewDto` - 32 edges
9. `PaymentReceiptDto` - 30 edges
10. `TutionCenterMaintanance.Data.Entities` - 29 edges

## Surprising Connections (you probably didn't know these)
- `Student` --references--> `Gender`  [EXTRACTED]
  TutionCenterMaintanance/Data/Entities/StudentEntities.cs → TutionCenterMaintanance.Shared/Enums/AppEnums.cs
- `Attendance` --references--> `AttendanceStatus`  [EXTRACTED]
  TutionCenterMaintanance/Data/Entities/AssessmentEntities.cs → TutionCenterMaintanance.Shared/Enums/AppEnums.cs
- `Payment` --references--> `PaymentMode`  [EXTRACTED]
  TutionCenterMaintanance/Data/Entities/FeeEntities.cs → TutionCenterMaintanance.Shared/Enums/AppEnums.cs
- `AssessmentType` --references--> `AssessmentCategory`  [EXTRACTED]
  TutionCenterMaintanance/Data/Entities/AssessmentEntities.cs → TutionCenterMaintanance.Shared/Enums/AppEnums.cs
- `Expense` --references--> `ExpenseCategory`  [EXTRACTED]
  TutionCenterMaintanance/Data/Entities/ExpenseEntity.cs → TutionCenterMaintanance.Shared/Enums/AppEnums.cs

## Import Cycles
- None detected.

## Communities (155 total, 20 thin omitted)

### Community 1 - "TutionCenterMaintanance.Client/_Imports.razor"
Cohesion: 0.07
Nodes (30): TutionCenterMaintanance.Shared.DTOs.Reports, TutionCenterMaintanance.Shared.DTOs.Financials, TutionCenterMaintanance.Shared.DTOs.Academic, TutionCenterMaintanance.Shared.DTOs.Fees, TutionCenterMaintanance.Shared.DTOs.Homework, TutionCenterMaintanance.Shared.DTOs.Notifications, TutionCenterMaintanance.Shared.DTOs.Auth, TutionCenterMaintanance.Shared.DTOs.Stationery (+22 more)

### Community 3 - "FeesPage.razor"
Cohesion: 0.05
Nodes (35): RecordPaymentDialog, route:/fees, SetMonthlyFeeDialog, StudentLedgerDialog, GoToCurrentMonth, LoadData, LoadGrades, NextMonth (+27 more)

### Community 4 - "WhatsAppQuickMessage.razor"
Cohesion: 0.04
Nodes (44): route:/whatsapp, DeleteTemplate, GetSamplePreview, LoadAbsentRecipients, LoadAssessmentRecipients, LoadLogs, LoadOverdueRecipients, LoadTemplates (+36 more)

### Community 5 - "HomeworkPage.razor"
Cohesion: 0.04
Nodes (43): MudCardActions, route:/homework, DeleteHomework, LoadHomework, OnBatchFilterChanged, OnInitializedAsync, OpenCreateHomeworkDialog, OpenSubmissionsDialog (+35 more)

### Community 6 - "DashboardStatsDto"
Cohesion: 0.12
Nodes (15): DashboardStatsDto, NetOperatingProfitMonth, OverdueStudentsCount, RecentActivities, TodayAbsentCount, TodayAttendancePercentage, TodayPresentCount, TotalActiveStudents (+7 more)

### Community 7 - "TimetableSlotDto"
Cohesion: 0.06
Nodes (28): TimetableController, BatchWeeklyTimetableDto, BatchId, BatchName, GradeName, Slots, CreateOrUpdateTimetableRequest, BatchId (+20 more)

### Community 8 - "Batch"
Cohesion: 0.06
Nodes (39): AcademicYear, Assessments, Batches, EndDate, IsCurrent, Name, StartDate, Batch (+31 more)

### Community 9 - "MainLayout.razor"
Cohesion: 0.05
Nodes (35): ActivatorContent, Authorized, AuthorizeView, ChildContent, LayoutComponentBase, MudAppBar, MudBadge, MudDialogProvider (+27 more)

### Community 10 - "StudentList.razor"
Cohesion: 0.04
Nodes (40): MudPagination, PagerContent, route:/students, StudentDialog, ApplyFilters, ConfirmDelete, Dispose, LoadLookups (+32 more)

### Community 11 - "BatchesPage.razor"
Cohesion: 0.05
Nodes (38): route:/batches, DeleteBatch, LoadBatches, OnInitializedAsync, OpenCreateBatchDialog, OpenEditBatchDialog, AcademicYearDto, BatchDto (+30 more)

### Community 12 - "20261027154545_InitialCreate.Designer.cs"
Cohesion: 0.09
Nodes (3): TutionCenterMaintanance.Migrations, InitialCreate, Phase2_FinancialAndCommunication

### Community 13 - "TimetablePage.razor"
Cohesion: 0.05
Nodes (35): route:/timetable, DeleteSlot, GetDayColor, LoadSlots, OnBatchChanged, OnInitializedAsync, OnTeacherChanged, OpenCreateSlotDialog (+27 more)

### Community 14 - "FinancialsPage.razor"
Cohesion: 0.04
Nodes (51): AddExpenseDialog, AddStationeryItemDialog, AdjustStockDialog, RecordPurchaseDialog, RecordSaleDialog, route:/financials, DeleteExpense, DeleteSale (+43 more)

### Community 15 - "UserManagementPage.razor"
Cohesion: 0.05
Nodes (34): route:/users, LoadData, OnInitializedAsync, OnRoleSelectionChanged, OpenCreateUserDialog, DialogActions, DialogContent, HeaderContent (+26 more)

### Community 16 - "RoleDto"
Cohesion: 0.21
Nodes (9): CreateOrUpdateRoleRequest, Description, Name, Permissions, RoleDto, Description, Id, Name (+1 more)

### Community 17 - ".ImportStudents"
Cohesion: 0.07
Nodes (21): ImportExportController, ImportStudentsRequest, CsvContent, ImportSummaryDto, Errors, FailureCount, SuccessCount, SuccessMessages (+13 more)

### Community 18 - "Homework"
Cohesion: 0.06
Nodes (35): AuditLog, Action, EntityId, EntityName, Id, NewValues, OldValues, Timestamp (+27 more)

### Community 19 - "AcademicConfigPage.razor"
Cohesion: 0.05
Nodes (34): route:/academic-setup, AddBoard, AddSubject, AddYear, LoadAll, LoadMapping, OnInitializedAsync, OnMapBoardChanged (+26 more)

### Community 20 - "AuditLogDto"
Cohesion: 0.07
Nodes (20): AuditController, BackupController, AuditLogDto, Action, EntityId, EntityName, Id, NewValues (+12 more)

### Community 21 - "BulkMarksEntry.razor"
Cohesion: 0.06
Nodes (31): AssessmentSubjectDto, route:/marks/bulk-entry, LoadGrid, OnAssessmentChanged, OnBatchChanged, OnInitializedAsync, OnSubjectChanged, AssessmentDto (+23 more)

### Community 22 - "ReportsPage.razor"
Cohesion: 0.06
Nodes (31): route:/reports, GetCategoryColor, LoadAttendanceCorrelation, LoadBatchComparisons, LoadStudentTrend, LoadSubjectComparisons, OnInitializedAsync, OnStudentChanged (+23 more)

### Community 23 - "TeachersPage.razor"
Cohesion: 0.06
Nodes (31): route:/teachers, DeleteTeacher, LoadTeachers, OnInitializedAsync, OpenCreateDialog, OpenEditDialog, DialogActions, DialogContent (+23 more)

### Community 24 - "BatchAttendanceSheetDto"
Cohesion: 0.07
Nodes (26): AttendanceController, AttendanceSummaryDto, AbsentCount, AttendancePercentage, PresentCount, TotalStudents, BatchAttendanceSheetDto, BatchId (+18 more)

### Community 25 - "AttendancePage.razor"
Cohesion: 0.06
Nodes (30): MudButtonGroup, route:/attendance, LoadAttendanceSheet, MarkAllAbsent, MarkAllPresent, OnBatchChanged, OnDateChanged, OnInitializedAsync (+22 more)

### Community 26 - "AssessmentsPage.razor"
Cohesion: 0.06
Nodes (27): CreateAssessmentDialog, route:/assessments, LoadAssessments, OnInitializedAsync, OpenCreateDialog, AssessmentDto, BoardDto, GradeDto (+19 more)

### Community 27 - "AppDbContext"
Cohesion: 0.06
Nodes (30): AppDbContext, AcademicYears, Assessments, AssessmentSubjects, AssessmentTypes, Attendances, AuditLogs, Batches (+22 more)

### Community 28 - "StudentLedgerDialog.razor"
Cohesion: 0.08
Nodes (21): Close, LoadLedger, OnInitializedAsync, OpenReceipt, DialogActions, DialogContent, HeaderContent, MudAlert (+13 more)

### Community 29 - "CreateAssessmentDialog.razor"
Cohesion: 0.06
Nodes (29): AddSubjectRow, Cancel, OnBoardChanged, OnGradeOrBoardChanged, OnInitializedAsync, AcademicYearDto, AssessmentTypeDto, BoardDto (+21 more)

### Community 31 - "StudentDialog.razor"
Cohesion: 0.04
Nodes (42): InputFile, StudentDraftDto, Cancel, CleanIndianPhoneNumber, ClearDraftAsync, GetBoardName, GetGradeName, GetStepTitle (+34 more)

### Community 32 - "FeeDefaulterItemDto"
Cohesion: 0.12
Nodes (16): FeeDefaulterItemDto, AgingBracket, BoardName, FeePlanName, FormattedReminderMessage, GradeName, MaxDaysOverdue, OldestDueDate (+8 more)

### Community 33 - "AuditLogsPage.razor"
Cohesion: 0.07
Nodes (24): route:/audit, GetActionColor, LoadLogs, OnActionChanged, OnEntityChanged, OnInitializedAsync, AuditLogDto, HeaderContent (+16 more)

### Community 34 - "BatchDto"
Cohesion: 0.09
Nodes (21): BatchDto, AcademicYearId, AcademicYearName, BoardId, BoardName, GradeId, GradeName, Id (+13 more)

### Community 35 - "ExpenseDto"
Cohesion: 0.05
Nodes (36): ExpensesController, Expense, Amount, Category, Date, Notes, PaidTo, RecordedByUser (+28 more)

### Community 36 - "CustomAuthStateProvider"
Cohesion: 0.12
Nodes (3): CustomAuthStateProvider, ILocalStorageService, LocalStorageService

### Community 37 - ".OnModelCreating"
Cohesion: 0.09
Nodes (24): Assessment, AcademicYear, AcademicYearId, AssessmentSubjects, AssessmentType, AssessmentTypeId, Board, BoardId (+16 more)

### Community 38 - "Student"
Cohesion: 0.08
Nodes (23): Student, Address, AdmissionDate, Attendances, BloodGroup, Board, BoardId, DateOfBirth (+15 more)

### Community 39 - "PaymentReceiptDto"
Cohesion: 0.08
Nodes (25): PaymentReceiptDto, AmountPaid, BoardName, CenterAddress, CenterEmail, CenterName, CenterPhone, CenterTagline (+17 more)

### Community 40 - "NotificationDto"
Cohesion: 0.22
Nodes (8): NotificationDto, CreatedAt, Id, IsRead, Message, RelativeTime, Title, Type

### Community 41 - "ReportCardPage.razor"
Cohesion: 0.09
Nodes (19): MudSimpleTable, route:/report-cards, LoadReportCard, OnInitializedAsync, OnStudentChanged, PrintReportCard, MudButton, MudCard (+11 more)

### Community 42 - "RecordPaymentDialog.razor"
Cohesion: 0.10
Nodes (16): Cancel, OnInitialized, DialogActions, DialogContent, MudButton, MudDatePicker, MudDialog, MudGrid (+8 more)

### Community 43 - "ApiResponse"
Cohesion: 0.07
Nodes (5): ApiResponse, Data, Errors, Message, Success

### Community 44 - "BaseEntity"
Cohesion: 0.07
Nodes (27): BoardGradeSubject, Board, BoardId, Grade, GradeId, Subject, SubjectId, Subject (+19 more)

### Community 45 - "Payment"
Cohesion: 0.15
Nodes (12): Payment, Amount, FeeInstallmentId, PaymentDate, PaymentMode, ReceiptNumber, ReceivedByUser, ReceivedByUserId (+4 more)

### Community 46 - "StudentDto"
Cohesion: 0.09
Nodes (23): StudentDto, Address, AdmissionDate, BatchIds, BatchNames, BloodGroup, BoardId, BoardName (+15 more)

### Community 47 - "TutionCenterMaintanance.csproj"
Cohesion: 0.09
Nodes (21): Microsoft.AspNetCore.Authentication.JwtBearer (10.0.14), Microsoft.AspNetCore.Components.Authorization (10.0.14), Microsoft.AspNetCore.Components.WebAssembly.Server (10.0.14), Microsoft.AspNetCore.Identity.EntityFrameworkCore (10.0.14), Microsoft.EntityFrameworkCore.Sqlite (10.0.14), Microsoft.EntityFrameworkCore.SqlServer (10.0.14), Microsoft.EntityFrameworkCore.Tools (10.0.14), System.Security.Cryptography.Pkcs (10.0.15) (+13 more)

### Community 48 - "DataManagementPage.razor"
Cohesion: 0.10
Nodes (17): route:/data-management, ImportCsv, OnInitializedAsync, MudAlert, MudButton, MudCard, MudChip, MudGrid (+9 more)

### Community 49 - "PaymentReceiptDialog.razor"
Cohesion: 0.10
Nodes (17): Close, LoadReceiptAsync, OnInitializedAsync, PrintReceipt, DialogActions, DialogContent, MudAlert, MudButton (+9 more)

### Community 50 - "Login.razor"
Cohesion: 0.12
Nodes (10): DataAnnotationsValidator, EditForm, route:/login, FillDemoCredentials, HandleLogin, MudButton, MudIcon, MudProgressCircular (+2 more)

### Community 51 - "Home.razor"
Cohesion: 0.15
Nodes (9): route:/, LoadDashboard, OnInitializedAsync, MudButton, MudGrid, MudIcon, MudItem, MudProgressLinear (+1 more)

### Community 52 - "Attendance"
Cohesion: 0.10
Nodes (19): Attendance, Batch, BatchId, Date, MarkedByUserId, Remarks, Status, Student (+11 more)

### Community 53 - "CreateStudentRequest"
Cohesion: 0.11
Nodes (17): CreateStudentRequest, Address, AdmissionDate, BatchIds, BloodGroup, BoardId, DateOfBirth, FirstName (+9 more)

### Community 54 - "HomeworkDto"
Cohesion: 0.05
Nodes (45): BulkUpdateHomeworkSubmissionsRequest, HomeworkId, Submissions, CreateHomeworkRequest, AssignedDate, BatchId, Description, DueDate (+37 more)

### Community 55 - "http"
Cohesion: 0.12
Nodes (17): ASPNETCORE_ENVIRONMENT, applicationUrl, commandName, dotnetRunMessages, environmentVariables, inspectUri, launchBrowser, applicationUrl (+9 more)

### Community 56 - "RecordPaymentRequest"
Cohesion: 0.12
Nodes (16): AssignFeePlanRequest, DiscountId, FeePlanId, FinalAmount, GenerateInstallments, StartDate, StudentId, RecordPaymentRequest (+8 more)

### Community 57 - "FeePlanDto"
Cohesion: 0.12
Nodes (17): FeePlanDto, AcademicYearId, AcademicYearName, Amount, Frequency, GradeId, GradeName, Id (+9 more)

### Community 58 - "DynamicPermissionPolicyProvider"
Cohesion: 0.17
Nodes (4): DynamicPermissionPolicyProvider, PermissionAuthorizationHandler, PermissionRequirement, Permission

### Community 59 - "AssessmentTypeDto"
Cohesion: 0.23
Nodes (6): AssessmentsController, AssessmentTypeDto, Category, Description, Id, Name

### Community 60 - "FeeInstallmentDto"
Cohesion: 0.12
Nodes (16): FeeInstallmentDto, Amount, DaysOverdue, DueDate, Id, IsOverdue, PaidAmount, RemainingAmount (+8 more)

### Community 61 - "CenterSettingsPage.razor"
Cohesion: 0.13
Nodes (12): route:/settings, OnInitializedAsync, MudButton, MudCard, MudGrid, MudItem, MudProgressCircular, MudProgressLinear (+4 more)

### Community 63 - "AssessmentDto"
Cohesion: 0.13
Nodes (15): AssessmentDto, AcademicYearId, AcademicYearName, AssessmentTypeId, AssessmentTypeName, BoardId, BoardName, Category (+7 more)

### Community 64 - "StationerySale"
Cohesion: 0.07
Nodes (27): StationeryItem, DefaultSellingPrice, Description, IsActive, Name, Purchases, Sales, Unit (+19 more)

### Community 65 - "PaymentDto"
Cohesion: 0.13
Nodes (15): PaymentDto, Amount, BoardName, FeeInstallmentId, GradeName, Id, InstallmentTitle, PaymentDate (+7 more)

### Community 66 - "StudentFeeLedgerDto"
Cohesion: 0.13
Nodes (15): StudentFeeLedgerDto, BoardName, DueAmount, FatherName, FeePlanName, GradeName, Installments, PaidAmount (+7 more)

### Community 67 - "StudentFeeSummaryDto"
Cohesion: 0.13
Nodes (15): StudentFeeSummaryDto, BoardName, DueAmount, FeePlanName, GradeName, NextDueDate, OverdueAmount, OverdueInstallmentsCount (+7 more)

### Community 68 - "FinancialOverviewDto"
Cohesion: 0.07
Nodes (27): FinancialOverviewDto, ActiveStudentsCount, CollectionRate, ExpenseBreakdown, FeeRevenue, Month, NetOperatingSurplus, NetProfitChange (+19 more)

### Community 69 - "CreateAssessmentRequest"
Cohesion: 0.15
Nodes (11): CreateAssessmentRequest, AcademicYearId, AssessmentTypeId, BoardId, Date, GradeId, Name, Subjects (+3 more)

### Community 70 - "AppUser"
Cohesion: 0.21
Nodes (6): AppUser, CreatedAt, FullName, IsActive, ITokenService, TokenService

### Community 71 - "PaginatedResult"
Cohesion: 0.18
Nodes (8): PaginatedResult, HasNextPage, HasPreviousPage, Items, PageNumber, PageSize, TotalCount, TotalPages

### Community 72 - "TeacherDto"
Cohesion: 0.16
Nodes (9): TeacherDto, Email, Id, IsActive, Name, Phone, Specialization, SubjectIds (+1 more)

### Community 73 - "TutionCenterMaintanance.Data.Entities"
Cohesion: 0.32
Nodes (6): TutionCenterMaintanance.Shared.Models, TutionCenterMaintanance.Controllers, TutionCenterMaintanance.Shared.Constants, TutionCenterMaintanance.Data, TutionCenterMaintanance.Data.Entities, Microsoft.AspNetCore.Authorization

### Community 74 - "BulkMarksGridDto"
Cohesion: 0.15
Nodes (13): BulkMarksGridDto, AssessmentId, AssessmentName, AssessmentSubjectId, MaxMarks, Rows, SubjectName, StudentMarkRowDto (+5 more)

### Community 75 - "StudentListItemDto"
Cohesion: 0.13
Nodes (13): StudentListItemDto, BatchNames, BoardName, FatherName, FullName, GradeName, Id, IsActive (+5 more)

### Community 76 - "Timetable"
Cohesion: 0.11
Nodes (17): Teacher, Email, IsActive, Name, Phone, Specialization, TeacherBatchSubjects, Timetable (+9 more)

### Community 77 - "ReportCardExamSectionDto"
Cohesion: 0.17
Nodes (12): ReportCardExamSectionDto, AssessmentName, AssessmentType, Category, Date, Marks, Percentage, TotalMaximum (+4 more)

### Community 78 - "PaymentMode"
Cohesion: 0.17
Nodes (12): PaymentModeBreakdownDto, Mode, ModeName, Percentage, TotalAmount, TransactionCount, PaymentMode, BankTransfer (+4 more)

### Community 79 - "SubjectMarkDetailDto"
Cohesion: 0.33
Nodes (6): SubjectMarkDetailDto, IsAbsent, MarksObtained, MaxMarks, Remarks, SubjectName

### Community 80 - "Routes.razor"
Cohesion: 0.18
Nodes (10): AuthorizeRouteView, Authorizing, CascadingAuthenticationState, FocusOnNavigate, Found, Router, Login, MudProgressCircular (+2 more)

### Community 82 - "AcademicYearDto"
Cohesion: 0.22
Nodes (6): AcademicYearDto, EndDate, Id, IsCurrent, Name, StartDate

### Community 83 - "BoardDto"
Cohesion: 0.24
Nodes (5): BoardDto, Code, Id, IsActive, Name

### Community 84 - "RecordSaleDialog.razor"
Cohesion: 0.08
Nodes (21): Cancel, OnInitialized, OnItemSelected, DialogActions, DialogContent, MudButton, MudDatePicker, MudDialog (+13 more)

### Community 85 - "SubjectDto"
Cohesion: 0.24
Nodes (5): SubjectDto, Code, Id, IsActive, Name

### Community 86 - "Components/_Imports.razor"
Cohesion: 0.18
Nodes (10): TutionCenterMaintanance.Components, Microsoft.AspNetCore.Components.Forms, Microsoft.AspNetCore.Components.Routing, Microsoft.AspNetCore.Components.Web, Microsoft.AspNetCore.Components.Web.RenderMode, Microsoft.AspNetCore.Components.Web.Virtualization, Microsoft.JSInterop, System.Net.Http.Json (+2 more)

### Community 87 - "ParentInfo"
Cohesion: 0.18
Nodes (11): ParentInfo, EmergencyContactName, EmergencyContactPhone, FatherName, FatherPhone, FatherWhatsApp, MotherName, MotherPhone (+3 more)

### Community 88 - "BatchPerformanceComparisonDto"
Cohesion: 0.18
Nodes (11): BatchPerformanceComparisonDto, AverageAcademicScore, AverageAttendancePercentage, BatchId, BatchName, BoardName, GradeName, StudentCount (+3 more)

### Community 89 - "DiscountDto"
Cohesion: 0.15
Nodes (11): DiscountDto, Id, IsPercentage, Name, Type, Value, DiscountType, Custom (+3 more)

### Community 90 - "LoginResponse"
Cohesion: 0.24
Nodes (7): LoginRequest, Password, UserName, LoginResponse, Expiry, Token, User

### Community 92 - "BoardGradeSubjectDto"
Cohesion: 0.25
Nodes (8): BoardGradeSubjectDto, BoardId, BoardName, GradeId, GradeName, Id, SubjectId, SubjectName

### Community 93 - "StudentReportCardDto"
Cohesion: 0.33
Nodes (6): StudentReportCardDto, BoardName, Exams, GradeName, StudentId, StudentName

### Community 94 - "ChangePasswordRequest"
Cohesion: 0.67
Nodes (3): ChangePasswordRequest, CurrentPassword, NewPassword

### Community 97 - "RecordPurchaseDialog.razor"
Cohesion: 0.09
Nodes (19): Cancel, OnInitialized, DialogActions, DialogContent, MudButton, MudDatePicker, MudDialog, MudGrid (+11 more)

### Community 98 - "BulkSaveMarksRequest"
Cohesion: 0.22
Nodes (8): BulkSaveMarksRequest, AssessmentSubjectId, Marks, StudentMarkSaveItem, IsAbsent, MarksObtained, Remarks, StudentId

### Community 99 - ".InitializeAsync"
Cohesion: 0.10
Nodes (11): CenterSettings, AppRole, Description, RolePermissions, RolePermission, Id, Permission, Role (+3 more)

### Community 100 - "GradeFinancialSummaryDto"
Cohesion: 0.22
Nodes (9): GradeFinancialSummaryDto, CollectionRate, DefaulterCount, GradeId, GradeName, StudentCount, TotalCollected, TotalExpected (+1 more)

### Community 101 - "AddExpenseDialog.razor"
Cohesion: 0.10
Nodes (17): Cancel, DialogActions, DialogContent, MudButton, MudDatePicker, MudDialog, MudGrid, MudIcon (+9 more)

### Community 102 - "UserDto"
Cohesion: 0.12
Nodes (16): CreateUserRequest, Email, FullName, Password, PhoneNumber, RoleName, UserName, UserDto (+8 more)

### Community 103 - "ParentInfoDto"
Cohesion: 0.22
Nodes (9): ParentInfoDto, EmergencyContactName, EmergencyContactPhone, FatherName, FatherPhone, FatherWhatsApp, MotherName, MotherPhone (+1 more)

### Community 104 - "AddStationeryItemDialog.razor"
Cohesion: 0.10
Nodes (17): Cancel, OnInitialized, DialogActions, DialogContent, MudButton, MudDialog, MudGrid, MudIcon (+9 more)

### Community 106 - "BulkWhatsAppRecipientDto"
Cohesion: 0.15
Nodes (13): BulkWhatsAppRecipientDto, Category, CleanPhone, Details, GradeName, HasValidPhone, IsSent, ParentName (+5 more)

### Community 108 - "AssessmentSubjectDto"
Cohesion: 0.33
Nodes (6): AssessmentSubjectDto, AssessmentId, Id, MaxMarks, SubjectId, SubjectName

### Community 109 - ".GetGradesAsync"
Cohesion: 0.40
Nodes (4): GradeDto, Id, Name, OrderIndex

### Community 110 - "CenterSettings"
Cohesion: 0.22
Nodes (9): CenterSettings, Address, CenterName, CurrencySymbol, Email, LogoBase64, Phone, Tagline (+1 more)

### Community 111 - "NotificationsController"
Cohesion: 0.20
Nodes (3): NotificationsController, NotificationCountDto, UnreadCount

### Community 112 - "MonthlyCollectionDto"
Cohesion: 0.33
Nodes (6): MonthlyCollectionDto, CollectedAmount, Month, MonthName, TransactionCount, Year

### Community 113 - "App.razor"
Cohesion: 0.40
Nodes (4): HeadOutlet, ImportMap, ResourcePreloader, Routes

### Community 114 - "AdjustStockDialog.razor"
Cohesion: 0.11
Nodes (15): Cancel, OnInitialized, DialogActions, DialogContent, MudButton, MudDialog, MudGrid, MudIcon (+7 more)

### Community 115 - "NavMenu.razor"
Cohesion: 0.50
Nodes (3): MudNavGroup, MudNavLink, MudNavMenu

### Community 116 - "Error.razor"
Cohesion: 0.50
Nodes (3): System.Diagnostics, OnInitialized, PageTitle

### Community 117 - "Gender"
Cohesion: 0.50
Nodes (4): Gender, Female, Male, Other

### Community 120 - "UpdateStudentRequest"
Cohesion: 0.50
Nodes (3): UpdateStudentRequest, Id, IsActive

### Community 124 - "StudentMonthlyStatusDto"
Cohesion: 0.12
Nodes (16): StudentMonthlyStatusDto, BoardName, GradeName, LastPaymentDate, Month, MonthlyFeeAmount, PaidAmount, ParentName (+8 more)

### Community 125 - "SetMonthlyFeeDialog.razor"
Cohesion: 0.13
Nodes (12): Cancel, OnInitialized, DialogActions, DialogContent, MudButton, MudDialog, MudIcon, MudNumericField (+4 more)

### Community 126 - "WhatsAppTemplateDto"
Cohesion: 0.18
Nodes (10): CreateOrUpdateTemplateRequest, BodyTemplate, Category, Name, WhatsAppTemplateDto, BodyTemplate, Category, Id (+2 more)

### Community 128 - "UpcomingAssessmentDto"
Cohesion: 0.17
Nodes (11): RecentActivityDto, Description, Icon, Timestamp, User, UpcomingAssessmentDto, Date, GradeName (+3 more)

### Community 129 - "FeeDefaultersSummaryDto"
Cohesion: 0.15
Nodes (12): FeeDefaultersSummaryDto, Amount16To30Days, Amount1To15Days, Amount31To60Days, Amount60PlusDays, Count16To30Days, Count1To15Days, Count31To60Days (+4 more)

### Community 130 - "StationeryItemDto"
Cohesion: 0.15
Nodes (13): StationeryItemDto, CurrentStock, DefaultSellingPrice, Description, Id, IsActive, Name, ProfitMargin (+5 more)

### Community 131 - "ManageBoardGradeSubjectsRequest"
Cohesion: 0.18
Nodes (8): ManageBoardGradeSubjectsRequest, BoardId, GradeId, SubjectIds, QuickStockAdjustmentRequest, Reason, StationeryItemId, TargetStock

### Community 132 - "StationerySaleDto"
Cohesion: 0.17
Nodes (12): StationerySaleDto, Date, Id, ItemName, Notes, Quantity, StationeryItemId, StudentId (+4 more)

### Community 134 - "WhatsAppMessageLogDto"
Cohesion: 0.18
Nodes (10): WhatsAppMessageLogDto, Id, MessageBody, MessageCategory, RecipientName, RecipientPhone, SentAt, SentByName (+2 more)

### Community 135 - "MonthlyFinancialTrendDto"
Cohesion: 0.18
Nodes (11): MonthlyFinancialTrendDto, FeeRevenue, Month, MonthLabel, NetProfit, OperatingExpenses, StationeryPurchasesCost, StationeryRevenue (+3 more)

### Community 136 - "StationeryPurchaseDto"
Cohesion: 0.18
Nodes (11): StationeryPurchaseDto, Date, Id, ItemName, Notes, Quantity, StationeryItemId, Supplier (+3 more)

### Community 137 - "CreateStationeryItemRequest"
Cohesion: 0.22
Nodes (8): CreateStationeryItemRequest, DefaultSellingPrice, Description, Name, Unit, UpdateStationeryItemRequest, Id, IsActive

### Community 138 - "WhatsAppMessageLog"
Cohesion: 0.20
Nodes (10): WhatsAppMessageLog, MessageBody, MessageCategory, RecipientName, RecipientPhone, SentAt, SentByUser, SentByUserId (+2 more)

### Community 139 - "AssessmentScoreTrendItemDto"
Cohesion: 0.20
Nodes (9): AssessmentScoreTrendItemDto, AssessmentId, AssessmentName, AssessmentTypeName, ClassAveragePercentage, Date, MarksObtained, MaxMarks (+1 more)

### Community 140 - "CenterSettingsDto"
Cohesion: 0.20
Nodes (10): CenterSettingsDto, Address, CenterName, CurrencySymbol, Email, Id, LogoBase64, Phone (+2 more)

### Community 141 - "CustomAuthStateProvider.cs"
Cohesion: 0.25
Nodes (3): TutionCenterMaintanance.Client.Services, TutionCenterMaintanance.Client.Auth, Microsoft.AspNetCore.Components.Authorization

### Community 142 - "RecordStationeryPurchaseRequest"
Cohesion: 0.25
Nodes (7): RecordStationeryPurchaseRequest, Date, Notes, Quantity, StationeryItemId, Supplier, UnitCost

### Community 144 - "AttendancePerformanceCorrelationDto"
Cohesion: 0.22
Nodes (9): AttendancePerformanceCorrelationDto, AcademicScorePercentage, AttendancePercentage, BatchName, CategoryStatus, GradeName, Recommendation, StudentId (+1 more)

### Community 145 - "SubjectPerformanceComparisonDto"
Cohesion: 0.22
Nodes (9): SubjectPerformanceComparisonDto, AverageScorePercentage, HighestScorePercentage, LowestScorePercentage, PassPercentage, SubjectId, SubjectName, TotalAssessmentsEvaluated (+1 more)

### Community 147 - "StudentProgressTrendDto"
Cohesion: 0.25
Nodes (6): StudentProgressTrendDto, GradeName, OverallAveragePercentage, StudentId, StudentName, TrendPoints

### Community 148 - "RecordStationerySaleRequest"
Cohesion: 0.25
Nodes (7): RecordStationerySaleRequest, Date, Notes, Quantity, StationeryItemId, StudentId, UnitPrice

### Community 150 - "UpdateUserRequest"
Cohesion: 0.33
Nodes (6): UpdateUserRequest, Email, FullName, IsActive, PhoneNumber, RoleName

### Community 151 - "LogWhatsAppDispatchDto"
Cohesion: 0.33
Nodes (6): LogWhatsAppDispatchDto, MessageBody, MessageCategory, RecipientName, RecipientPhone, StudentId

### Community 152 - "ExpenseCategoryBreakdownDto"
Cohesion: 0.33
Nodes (6): ExpenseCategoryBreakdownDto, Category, CategoryName, Percentage, TotalAmount, TransactionCount

### Community 153 - ".SetMonthlyFeeAsync"
Cohesion: 0.50
Nodes (3): SetMonthlyFeeRequest, MonthlyFeeAmount, StudentId

### Community 154 - "WhatsAppTemplate"
Cohesion: 0.40
Nodes (5): WhatsAppTemplate, BodyTemplate, Category, IsSystem, Name

## Knowledge Gaps
- **1969 isolated node(s):** `LayoutComponentBase`, `TutionCenterMaintanance.Client.Pages.Auth`, `MudThemeProvider`, `MudPopoverProvider`, `MudDialogProvider` (+1964 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 2187 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **20 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `ApiResponse` connect `ApiResponse` to `Task`, `WhatsAppController`, `ManageBoardGradeSubjectsRequest`, `.GetStats`, `TimetableSlotDto`, `CreateStationeryItemRequest`, `RecordStationeryPurchaseRequest`, `.GetAttendanceCorrelation`, `RoleDto`, `.ImportStudents`, `.GetOverview`, `AuditLogDto`, `.GetAbsentStudentRecipientsAsync`, `BatchAttendanceSheetDto`, `.SetMonthlyFeeAsync`, `.Ok`, `BatchDto`, `ExpenseDto`, `CreateStudentRequest`, `HomeworkDto`, `AssessmentTypeDto`, `.CreateStudent`, `CreateAssessmentRequest`, `PaginatedResult`, `TeacherDto`, `StudentListItemDto`, `ControllerBase`, `AcademicYearDto`, `BoardDto`, `SubjectDto`, `LoginResponse`, `AuthController`, `.GetStudentReportCard`, `UserDto`, `StationeryController`, `.GetGradesAsync`, `NotificationsController`, `UpdateStudentRequest`, `WhatsAppTemplateDto`, `.RecordPayment`?**
  _High betweenness centrality (0.162) - this node is a cross-community bridge._
- **Why does `AppDbContext` connect `AppDbContext` to `WhatsAppController`, `.GetStats`, `TimetableSlotDto`, `Batch`, `WhatsAppMessageLog`, `.GetAttendanceCorrelation`, `.ImportStudents`, `.GetOverview`, `Homework`, `AuditLogDto`, `BatchAttendanceSheetDto`, `WhatsAppTemplate`, `.Ok`, `ExpenseDto`, `.OnModelCreating`, `Student`, `BaseEntity`, `Payment`, `Attendance`, `DynamicPermissionPolicyProvider`, `AssessmentTypeDto`, `.CreateStudent`, `StationerySale`, `AppUser`, `TutionCenterMaintanance.Data.Entities`, `Timetable`, `ControllerBase`, `ParentInfo`, `AuthController`, `.GetStudentReportCard`, `.InitializeAsync`, `StationeryController`, `CenterSettings`, `NotificationsController`, `.RecordPayment`?**
  _High betweenness centrality (0.069) - this node is a cross-community bridge._
- **Why does `WhatsAppMessageLogDto` connect `WhatsAppMessageLogDto` to `TutionCenterMaintanance.Client/_Imports.razor`, `WhatsAppController`, `ApiResponse`?**
  _High betweenness centrality (0.021) - this node is a cross-community bridge._
- **What connects `LayoutComponentBase`, `TutionCenterMaintanance.Client.Pages.Auth`, `MudThemeProvider` to the rest of the system?**
  _1969 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Task` be split into smaller, more focused modules?**
  _Cohesion score 0.059278350515463915 - nodes in this community are weakly interconnected._
- **Should `TutionCenterMaintanance.Client/_Imports.razor` be split into smaller, more focused modules?**
  _Cohesion score 0.06666666666666667 - nodes in this community are weakly interconnected._
- **Should `FeesPage.razor` be split into smaller, more focused modules?**
  _Cohesion score 0.04878048780487805 - nodes in this community are weakly interconnected._