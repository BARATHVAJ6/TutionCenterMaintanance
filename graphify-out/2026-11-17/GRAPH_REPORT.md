# Graph Report - TutionCenterMaintanance  (2026-11-17)

## Corpus Check
- 102 files · ~60,978 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 6 file(s) not represented in the graph (top: .css 3, (none) 2, .db 1)

## Summary
- 2654 nodes · 4582 edges · 124 communities (109 shown, 15 thin omitted)
- Extraction: 95% EXTRACTED · 5% INFERRED · 0% AMBIGUOUS · INFERRED: 230 edges (avg confidence: 0.87)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `adcf709c`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- Task
- TutionCenterMaintanance.Client/_Imports.razor
- WhatsAppMessageLogDto
- FeesPage.razor
- WhatsAppQuickMessage.razor
- HomeworkPage.razor
- DashboardStatsDto
- TimetableSlotDto
- .OnModelCreating
- MainLayout.razor
- StudentList.razor
- BatchesPage.razor
- 20261027154545_InitialCreate.Designer.cs
- TimetablePage.razor
- FeePlansPage.razor
- UserManagementPage.razor
- RoleDto
- .ImportStudents
- WhatsAppMessageLog
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
- ApiResponse
- StudentDialog.razor
- FeeDefaulterItemDto
- AuditLogsPage.razor
- BatchDto
- FeePlan
- CustomAuthStateProvider
- Assessment
- Student
- PaymentReceiptDto
- NotificationDto
- ReportCardPage.razor
- RecordPaymentDialog.razor
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
- HomeworkSubmissionItemDto
- PaymentDto
- StudentFeeLedgerDto
- StudentFeeSummaryDto
- CreateHomeworkRequest
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
- .CreateHomework
- AcademicYearDto
- BoardDto
- SubjectDto
- Components/_Imports.razor
- ParentInfo
- BatchPerformanceComparisonDto
- DiscountDto
- LoginResponse
- Homework
- BoardGradeSubjectDto
- StudentReportCardDto
- .ChangePasswordAsync
- .GetStudentReportCard
- TutionCenterMaintanance/Program.cs
- AssignFeePlanRequest
- BulkSaveMarksRequest
- .InitializeAsync
- GradeFinancialSummaryDto
- RolePermission
- UserDto
- ParentInfoDto
- HomeworkDetailDto
- .BuildModel
- BulkWhatsAppRecipientDto
- CreateUserRequest
- AssessmentSubjectDto
- .GetGradesAsync
- CenterSettings
- .GetUnreadNotificationCountAsync
- MonthlyCollectionDto
- App.razor
- NavMenu.razor
- Error.razor
- Gender
- Counter.razor
- Weather.razor
- UpdateStudentRequest

## God Nodes (most connected - your core abstractions)
1. `ApiResponse` - 288 edges
2. `ApiService` - 99 edges
3. `IApiService` - 95 edges
4. `AppDbContext` - 86 edges
5. `Student` - 39 edges
6. `BaseEntity` - 35 edges
7. `StudentDto` - 32 edges
8. `PaymentReceiptDto` - 30 edges
9. `HomeworkDto` - 27 edges
10. `AssessmentDto` - 25 edges

## Surprising Connections (you probably didn't know these)
- `Student` --references--> `Gender`  [EXTRACTED]
  TutionCenterMaintanance/Data/Entities/StudentEntities.cs → TutionCenterMaintanance.Shared/Enums/AppEnums.cs
- `Attendance` --references--> `AttendanceStatus`  [EXTRACTED]
  TutionCenterMaintanance/Data/Entities/AssessmentEntities.cs → TutionCenterMaintanance.Shared/Enums/AppEnums.cs
- `Payment` --references--> `PaymentMode`  [EXTRACTED]
  TutionCenterMaintanance/Data/Entities/FeeEntities.cs → TutionCenterMaintanance.Shared/Enums/AppEnums.cs
- `FeeInstallment` --references--> `PaymentStatus`  [EXTRACTED]
  TutionCenterMaintanance/Data/Entities/FeeEntities.cs → TutionCenterMaintanance.Shared/Enums/AppEnums.cs
- `AssessmentType` --references--> `AssessmentCategory`  [EXTRACTED]
  TutionCenterMaintanance/Data/Entities/AssessmentEntities.cs → TutionCenterMaintanance.Shared/Enums/AppEnums.cs

## Import Cycles
- None detected.

## Communities (124 total, 15 thin omitted)

### Community 1 - "TutionCenterMaintanance.Client/_Imports.razor"
Cohesion: 0.07
Nodes (29): TutionCenterMaintanance.Shared.DTOs.Reports, TutionCenterMaintanance.Shared.DTOs.Academic, TutionCenterMaintanance.Shared.DTOs.Fees, TutionCenterMaintanance.Shared.DTOs.Homework, TutionCenterMaintanance.Shared.DTOs.Notifications, TutionCenterMaintanance.Shared.DTOs.Auth, TutionCenterMaintanance.Client.Services, TutionCenterMaintanance.Shared.DTOs.Assessments (+21 more)

### Community 2 - "WhatsAppMessageLogDto"
Cohesion: 0.07
Nodes (32): WhatsAppController, WhatsAppTemplate, BodyTemplate, Category, IsSystem, Name, CreateOrUpdateTemplateRequest, BodyTemplate (+24 more)

### Community 3 - "FeesPage.razor"
Cohesion: 0.04
Nodes (47): route:/fees, StudentLedgerDialog, GetAgingColor, GetModeColor, GetStatusColor, LoadAnalytics, LoadDefaulters, LoadFeeSummaries (+39 more)

### Community 4 - "WhatsAppQuickMessage.razor"
Cohesion: 0.04
Nodes (44): route:/whatsapp, DeleteTemplate, GetSamplePreview, LoadAbsentRecipients, LoadAssessmentRecipients, LoadLogs, LoadOverdueRecipients, LoadTemplates (+36 more)

### Community 5 - "HomeworkPage.razor"
Cohesion: 0.04
Nodes (43): MudCardActions, route:/homework, DeleteHomework, LoadHomework, OnBatchFilterChanged, OnInitializedAsync, OpenCreateHomeworkDialog, OpenSubmissionsDialog (+35 more)

### Community 6 - "DashboardStatsDto"
Cohesion: 0.05
Nodes (35): DashboardController, SettingsController, DashboardStatsDto, OverdueStudentsCount, RecentActivities, TodayAbsentCount, TodayAttendancePercentage, TodayPresentCount (+27 more)

### Community 7 - "TimetableSlotDto"
Cohesion: 0.06
Nodes (28): TimetableController, BatchWeeklyTimetableDto, BatchId, BatchName, GradeName, Slots, CreateOrUpdateTimetableRequest, BatchId (+20 more)

### Community 8 - ".OnModelCreating"
Cohesion: 0.05
Nodes (46): AcademicYear, Assessments, Batches, EndDate, IsCurrent, Name, StartDate, Batch (+38 more)

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
Cohesion: 0.11
Nodes (3): TutionCenterMaintanance.Migrations, InitialCreate, Phase2_FinancialAndCommunication

### Community 13 - "TimetablePage.razor"
Cohesion: 0.05
Nodes (35): route:/timetable, DeleteSlot, GetDayColor, LoadSlots, OnBatchChanged, OnInitializedAsync, OnTeacherChanged, OpenCreateSlotDialog (+27 more)

### Community 14 - "FeePlansPage.razor"
Cohesion: 0.05
Nodes (35): route:/fees/plans, LoadData, OnInitializedAsync, OpenCreateDiscountDialog, OpenCreatePlanDialog, AcademicYearDto, CardHeaderContent, DialogActions (+27 more)

### Community 15 - "UserManagementPage.razor"
Cohesion: 0.05
Nodes (34): route:/users, LoadData, OnInitializedAsync, OnRoleSelectionChanged, OpenCreateUserDialog, DialogActions, DialogContent, HeaderContent (+26 more)

### Community 16 - "RoleDto"
Cohesion: 0.17
Nodes (9): CreateOrUpdateRoleRequest, Description, Name, Permissions, RoleDto, Description, Id, Name (+1 more)

### Community 17 - ".ImportStudents"
Cohesion: 0.07
Nodes (21): ImportExportController, ImportStudentsRequest, CsvContent, ImportSummaryDto, Errors, FailureCount, SuccessCount, SuccessMessages (+13 more)

### Community 18 - "WhatsAppMessageLog"
Cohesion: 0.06
Nodes (33): AuditLog, Action, EntityId, EntityName, Id, NewValues, OldValues, Timestamp (+25 more)

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
Cohesion: 0.09
Nodes (21): AttendanceController, BatchAttendanceSheetDto, BatchId, BatchName, Date, GradeId, Students, DailyAttendanceItemDto (+13 more)

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
Cohesion: 0.06
Nodes (28): Close, GetStatusColor, LoadLedgerAsync, OnInitializedAsync, OpenRecordPayment, DialogActions, DialogContent, HeaderContent (+20 more)

### Community 29 - "CreateAssessmentDialog.razor"
Cohesion: 0.06
Nodes (29): AddSubjectRow, Cancel, OnBoardChanged, OnGradeOrBoardChanged, OnInitializedAsync, AcademicYearDto, AssessmentTypeDto, BoardDto (+21 more)

### Community 30 - "ApiResponse"
Cohesion: 0.07
Nodes (10): AcademicController, AuthController, FeesController, NotificationsController, ReportsController, ApiResponse, Data, Errors (+2 more)

### Community 31 - "StudentDialog.razor"
Cohesion: 0.04
Nodes (42): InputFile, StudentDraftDto, Cancel, CleanIndianPhoneNumber, ClearDraftAsync, GetBoardName, GetGradeName, GetStepTitle (+34 more)

### Community 32 - "FeeDefaulterItemDto"
Cohesion: 0.07
Nodes (28): FeeDefaulterItemDto, AgingBracket, BoardName, FeePlanName, FormattedReminderMessage, GradeName, MaxDaysOverdue, OldestDueDate (+20 more)

### Community 33 - "AuditLogsPage.razor"
Cohesion: 0.07
Nodes (24): route:/audit, GetActionColor, LoadLogs, OnActionChanged, OnEntityChanged, OnInitializedAsync, AuditLogDto, HeaderContent (+16 more)

### Community 34 - "BatchDto"
Cohesion: 0.08
Nodes (25): BatchDto, AcademicYearId, AcademicYearName, BoardId, BoardName, GradeId, GradeName, Id (+17 more)

### Community 35 - "FeePlan"
Cohesion: 0.08
Nodes (25): Discount, IsPercentage, Name, StudentFeePlans, Type, Value, FeePlan, AcademicYear (+17 more)

### Community 36 - "CustomAuthStateProvider"
Cohesion: 0.12
Nodes (3): CustomAuthStateProvider, ILocalStorageService, LocalStorageService

### Community 37 - "Assessment"
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
Cohesion: 0.09
Nodes (19): Cancel, OnInitialized, DialogActions, DialogContent, MudButton, MudCheckBox, MudDatePicker, MudDialog (+11 more)

### Community 44 - "BaseEntity"
Cohesion: 0.07
Nodes (28): Board, Batches, BoardGradeSubjects, Code, IsActive, Name, Students, BoardGradeSubject (+20 more)

### Community 45 - "Payment"
Cohesion: 0.09
Nodes (22): FeeInstallment, Amount, DueDate, PaidAmount, Payments, Status, StudentFeePlan, StudentFeePlanId (+14 more)

### Community 46 - "StudentDto"
Cohesion: 0.09
Nodes (22): StudentDto, Address, AdmissionDate, BatchIds, BatchNames, BloodGroup, BoardId, BoardName (+14 more)

### Community 47 - "TutionCenterMaintanance.csproj"
Cohesion: 0.09
Nodes (20): Microsoft.AspNetCore.Authentication.JwtBearer (10.0.14), Microsoft.AspNetCore.Components.Authorization (10.0.14), Microsoft.AspNetCore.Components.WebAssembly.Server (10.0.14), Microsoft.AspNetCore.Identity.EntityFrameworkCore (10.0.14), Microsoft.EntityFrameworkCore.Sqlite (10.0.14), Microsoft.EntityFrameworkCore.SqlServer (10.0.14), Microsoft.EntityFrameworkCore.Tools (10.0.14), Microsoft.NET.Sdk (+12 more)

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
Nodes (16): CreateStudentRequest, Address, AdmissionDate, BatchIds, BloodGroup, BoardId, DateOfBirth, FirstName (+8 more)

### Community 54 - "HomeworkDto"
Cohesion: 0.11
Nodes (19): HomeworkDto, AssignedDate, BatchId, BatchName, CheckedCount, Description, DueDate, GradeName (+11 more)

### Community 55 - "http"
Cohesion: 0.12
Nodes (17): ASPNETCORE_ENVIRONMENT, applicationUrl, commandName, dotnetRunMessages, environmentVariables, inspectUri, launchBrowser, applicationUrl (+9 more)

### Community 56 - "RecordPaymentRequest"
Cohesion: 0.20
Nodes (9): RecordPaymentRequest, Amount, AutoAllocateToInstallments, FeeInstallmentId, PaymentDate, PaymentMode, Remarks, StudentId (+1 more)

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

### Community 64 - "HomeworkSubmissionItemDto"
Cohesion: 0.17
Nodes (12): HomeworkSubmissionItemDto, GradeName, HomeworkId, ParentName, ParentPhone, Remarks, Status, StudentId (+4 more)

### Community 65 - "PaymentDto"
Cohesion: 0.13
Nodes (15): PaymentDto, Amount, BoardName, FeeInstallmentId, GradeName, Id, InstallmentTitle, PaymentDate (+7 more)

### Community 66 - "StudentFeeLedgerDto"
Cohesion: 0.13
Nodes (15): StudentFeeLedgerDto, BoardName, DueAmount, FatherName, FeePlanName, GradeName, Installments, PaidAmount (+7 more)

### Community 67 - "StudentFeeSummaryDto"
Cohesion: 0.13
Nodes (15): StudentFeeSummaryDto, BoardName, DueAmount, FeePlanName, GradeName, NextDueDate, OverdueAmount, OverdueInstallmentsCount (+7 more)

### Community 68 - "CreateHomeworkRequest"
Cohesion: 0.20
Nodes (8): CreateHomeworkRequest, AssignedDate, BatchId, Description, DueDate, SubjectId, TeacherId, Title

### Community 69 - "CreateAssessmentRequest"
Cohesion: 0.15
Nodes (11): CreateAssessmentRequest, AcademicYearId, AssessmentTypeId, BoardId, Date, GradeId, Name, Subjects (+3 more)

### Community 70 - "AppUser"
Cohesion: 0.21
Nodes (6): AppUser, CreatedAt, FullName, IsActive, ITokenService, TokenService

### Community 71 - "PaginatedResult"
Cohesion: 0.22
Nodes (8): PaginatedResult, HasNextPage, HasPreviousPage, Items, PageNumber, PageSize, TotalCount, TotalPages

### Community 72 - "TeacherDto"
Cohesion: 0.16
Nodes (9): TeacherDto, Email, Id, IsActive, Name, Phone, Specialization, SubjectIds (+1 more)

### Community 73 - "TutionCenterMaintanance.Data.Entities"
Cohesion: 0.30
Nodes (7): TutionCenterMaintanance.Shared.Models, TutionCenterMaintanance.Controllers, TutionCenterMaintanance.Shared.Constants, TutionCenterMaintanance.Shared.Enums, TutionCenterMaintanance.Data, TutionCenterMaintanance.Data.Entities, Microsoft.AspNetCore.Authorization

### Community 74 - "BulkMarksGridDto"
Cohesion: 0.15
Nodes (13): BulkMarksGridDto, AssessmentId, AssessmentName, AssessmentSubjectId, MaxMarks, Rows, SubjectName, StudentMarkRowDto (+5 more)

### Community 75 - "StudentListItemDto"
Cohesion: 0.14
Nodes (12): StudentListItemDto, BatchNames, BoardName, FatherName, FullName, GradeName, Id, IsActive (+4 more)

### Community 76 - "Timetable"
Cohesion: 0.17
Nodes (10): Timetable, Batch, BatchId, DayOfWeek, EndTime, StartTime, Subject, SubjectId (+2 more)

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
Cohesion: 0.04
Nodes (44): AssessmentScoreTrendItemDto, AssessmentId, AssessmentName, AssessmentTypeName, ClassAveragePercentage, Date, MarksObtained, MaxMarks (+36 more)

### Community 89 - "DiscountDto"
Cohesion: 0.18
Nodes (11): DiscountDto, Id, IsPercentage, Name, Type, Value, DiscountType, Custom (+3 more)

### Community 90 - "LoginResponse"
Cohesion: 0.24
Nodes (7): LoginRequest, Password, UserName, LoginResponse, Expiry, Token, User

### Community 91 - "Homework"
Cohesion: 0.15
Nodes (12): Homework, AssignedDate, Batch, BatchId, Description, DueDate, Subject, SubjectId (+4 more)

### Community 92 - "BoardGradeSubjectDto"
Cohesion: 0.25
Nodes (8): BoardGradeSubjectDto, BoardId, BoardName, GradeId, GradeName, Id, SubjectId, SubjectName

### Community 93 - "StudentReportCardDto"
Cohesion: 0.33
Nodes (6): StudentReportCardDto, BoardName, Exams, GradeName, StudentId, StudentName

### Community 94 - ".ChangePasswordAsync"
Cohesion: 0.50
Nodes (3): ChangePasswordRequest, CurrentPassword, NewPassword

### Community 97 - "AssignFeePlanRequest"
Cohesion: 0.22
Nodes (7): AssignFeePlanRequest, DiscountId, FeePlanId, FinalAmount, GenerateInstallments, StartDate, StudentId

### Community 98 - "BulkSaveMarksRequest"
Cohesion: 0.20
Nodes (8): BulkSaveMarksRequest, AssessmentSubjectId, Marks, StudentMarkSaveItem, IsAbsent, MarksObtained, Remarks, StudentId

### Community 99 - ".InitializeAsync"
Cohesion: 0.18
Nodes (3): CenterSettings, SeedData, Permissions

### Community 100 - "GradeFinancialSummaryDto"
Cohesion: 0.12
Nodes (15): UpdateUserRequest, Email, FullName, IsActive, PhoneNumber, RoleName, GradeFinancialSummaryDto, CollectionRate (+7 more)

### Community 101 - "RolePermission"
Cohesion: 0.20
Nodes (8): AppRole, Description, RolePermissions, RolePermission, Id, Permission, Role, RoleId

### Community 102 - "UserDto"
Cohesion: 0.22
Nodes (9): UserDto, Email, FullName, Id, IsActive, Permissions, PhoneNumber, RoleName (+1 more)

### Community 103 - "ParentInfoDto"
Cohesion: 0.22
Nodes (9): ParentInfoDto, EmergencyContactName, EmergencyContactPhone, FatherName, FatherPhone, FatherWhatsApp, MotherName, MotherPhone (+1 more)

### Community 104 - "HomeworkDetailDto"
Cohesion: 0.25
Nodes (6): BulkUpdateHomeworkSubmissionsRequest, HomeworkId, Submissions, HomeworkDetailDto, Homework, Submissions

### Community 106 - "BulkWhatsAppRecipientDto"
Cohesion: 0.09
Nodes (18): AttendanceSummaryDto, AbsentCount, AttendancePercentage, PresentCount, TotalStudents, BulkWhatsAppRecipientDto, Category, CleanPhone (+10 more)

### Community 107 - "CreateUserRequest"
Cohesion: 0.29
Nodes (7): CreateUserRequest, Email, FullName, Password, PhoneNumber, RoleName, UserName

### Community 108 - "AssessmentSubjectDto"
Cohesion: 0.33
Nodes (6): AssessmentSubjectDto, AssessmentId, Id, MaxMarks, SubjectId, SubjectName

### Community 109 - ".GetGradesAsync"
Cohesion: 0.40
Nodes (4): GradeDto, Id, Name, OrderIndex

### Community 110 - "CenterSettings"
Cohesion: 0.22
Nodes (9): CenterSettings, Address, CenterName, CurrencySymbol, Email, LogoBase64, Phone, Tagline (+1 more)

### Community 112 - "MonthlyCollectionDto"
Cohesion: 0.33
Nodes (6): MonthlyCollectionDto, CollectedAmount, Month, MonthName, TransactionCount, Year

### Community 113 - "App.razor"
Cohesion: 0.40
Nodes (4): HeadOutlet, ImportMap, ResourcePreloader, Routes

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

## Knowledge Gaps
- **1732 isolated node(s):** `LayoutComponentBase`, `TutionCenterMaintanance.Client.Pages.Auth`, `MudThemeProvider`, `MudPopoverProvider`, `MudDialogProvider` (+1727 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 1925 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **15 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `ApiResponse` connect `ApiResponse` to `Task`, `WhatsAppMessageLogDto`, `DashboardStatsDto`, `TimetableSlotDto`, `RoleDto`, `.ImportStudents`, `AuditLogDto`, `BatchAttendanceSheetDto`, `BatchDto`, `.EnsureAuthHeaderAsync`, `CreateStudentRequest`, `RecordPaymentRequest`, `AssessmentTypeDto`, `.CreateStudent`, `CreateHomeworkRequest`, `CreateAssessmentRequest`, `PaginatedResult`, `TeacherDto`, `StudentListItemDto`, `.CreateHomework`, `AcademicYearDto`, `BoardDto`, `.GetTimetableAsync`, `SubjectDto`, `LoginResponse`, `.ChangePasswordAsync`, `.GetStudentReportCard`, `AssignFeePlanRequest`, `BulkSaveMarksRequest`, `HomeworkDetailDto`, `BulkWhatsAppRecipientDto`, `.GetGradesAsync`, `.GetUnreadNotificationCountAsync`, `UpdateStudentRequest`?**
  _High betweenness centrality (0.130) - this node is a cross-community bridge._
- **Why does `AppDbContext` connect `AppDbContext` to `WhatsAppMessageLogDto`, `DashboardStatsDto`, `TimetableSlotDto`, `.OnModelCreating`, `.ImportStudents`, `WhatsAppMessageLog`, `AuditLogDto`, `BatchAttendanceSheetDto`, `ApiResponse`, `FeePlan`, `Assessment`, `Student`, `BaseEntity`, `Payment`, `Attendance`, `DynamicPermissionPolicyProvider`, `AssessmentTypeDto`, `.CreateStudent`, `AppUser`, `TutionCenterMaintanance.Data.Entities`, `Timetable`, `.CreateHomework`, `ParentInfo`, `Homework`, `.GetStudentReportCard`, `.InitializeAsync`, `RolePermission`, `CenterSettings`?**
  _High betweenness centrality (0.073) - this node is a cross-community bridge._
- **Why does `PaymentReceiptDto` connect `PaymentReceiptDto` to `Task`, `TutionCenterMaintanance.Client/_Imports.razor`, `AssignFeePlanRequest`, `PaymentMode`, `ApiResponse`?**
  _High betweenness centrality (0.025) - this node is a cross-community bridge._
- **What connects `LayoutComponentBase`, `TutionCenterMaintanance.Client.Pages.Auth`, `MudThemeProvider` to the rest of the system?**
  _1732 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Task` be split into smaller, more focused modules?**
  _Cohesion score 0.06479081821547575 - nodes in this community are weakly interconnected._
- **Should `TutionCenterMaintanance.Client/_Imports.razor` be split into smaller, more focused modules?**
  _Cohesion score 0.0663265306122449 - nodes in this community are weakly interconnected._
- **Should `WhatsAppMessageLogDto` be split into smaller, more focused modules?**
  _Cohesion score 0.06708595387840671 - nodes in this community are weakly interconnected._