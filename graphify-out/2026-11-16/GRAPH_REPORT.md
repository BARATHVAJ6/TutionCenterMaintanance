# Graph Report - TutionCenterMaintanance  (2026-11-16)

## Corpus Check
- 102 files · ~57,479 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 6 file(s) not represented in the graph (top: .css 3, (none) 2, .db 1)

## Summary
- 2645 nodes · 4570 edges · 124 communities (112 shown, 12 thin omitted)
- Extraction: 95% EXTRACTED · 5% INFERRED · 0% AMBIGUOUS · INFERRED: 230 edges (avg confidence: 0.87)
- Token cost: 0 input · 0 output

## Community Hubs (Navigation)
- Financial & Billing
- IAM & Security
- IAM & Security
- Financial & Billing
- Examinations & Grading
- Homework Management
- IAM & Security
- IAM & Security
- Examinations & Grading
- IAM & Security
- Student Directory & Admissions
- Database Schema & EF Core
- Database Schema & EF Core
- Timetable & Scheduling
- Financial & Billing
- IAM & Security
- IAM & Security
- IAM & Security
- IAM & Security
- Database Schema & EF Core
- IAM & Security
- Examinations & Grading
- Student Directory & Admissions
- Database Schema & EF Core
- IAM & Security
- Examinations & Grading
- Examinations & Grading
- Financial & Billing
- Financial & Billing
- Examinations & Grading
- IAM & Security
- Student Directory & Admissions
- Financial & Billing
- Administration & Data Management
- Student Directory & Admissions
- Financial & Billing
- IAM & Security
- Examinations & Grading
- Student Directory & Admissions
- Financial & Billing
- Examinations & Grading
- Student Directory & Admissions
- Financial & Billing
- IAM & Security
- Database Schema & EF Core
- Financial & Billing
- Student Directory & Admissions
- IAM & Security
- Parent Messaging & Communication
- Financial & Billing
- IAM & Security
- Database Schema & EF Core
- IAM & Security
- Student Directory & Admissions
- Homework Management
- Parent Messaging & Communication
- Financial & Billing
- Financial & Billing
- IAM & Security
- IAM & Security
- Financial & Billing
- Database Schema & EF Core
- Module 62 Core
- Examinations & Grading
- Blazor Client Shell & UI Layout
- Financial & Billing
- Financial & Billing
- Financial & Billing
- IAM & Security
- Examinations & Grading
- IAM & Security
- Student Directory & Admissions
- IAM & Security
- Student Directory & Admissions
- Student Directory & Admissions
- Student Directory & Admissions
- Timetable & Scheduling
- Examinations & Grading
- Financial & Billing
- Student Directory & Admissions
- IAM & Security
- IAM & Security
- Module 82 Core
- Module 83 Core
- Homework Management
- Module 85 Core
- Administration & Data Management
- Student Directory & Admissions
- Student Directory & Admissions
- IAM & Security
- IAM & Security
- IAM & Security
- Examinations & Grading
- IAM & Security
- Homework Management
- IAM & Security
- Student Directory & Admissions
- Module 97 Core
- Student Directory & Admissions
- IAM & Security
- Student Directory & Admissions
- Student Directory & Admissions
- Student Directory & Admissions
- Student Directory & Admissions
- Module 104 Core
- Student Directory & Admissions
- Module 107 Core
- Examinations & Grading
- Student Directory & Admissions
- Examinations & Grading
- IAM & Security
- Module 112 Core
- Administration & Data Management
- Module 114 Core
- Blazor Client Shell & UI Layout
- Module 116 Core
- Module 117 Core
- Module 118 Core
- Module 119 Core
- Student Directory & Admissions

## God Nodes (most connected - your core abstractions)
1. `ApiResponse` - 288 edges
2. `ApiService` - 98 edges
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

## Communities (124 total, 12 thin omitted)

### Community 0 - "Financial & Billing"
Cohesion: 0.06
Nodes (7): ApiService, IApiService, ApiResponse, Data, Errors, Message, Success

### Community 1 - "IAM & Security"
Cohesion: 0.07
Nodes (38): TutionCenterMaintanance.Shared.DTOs.Reports, TutionCenterMaintanance.Shared.DTOs.Academic, TutionCenterMaintanance.Infrastructure, TutionCenterMaintanance.Shared.Models, TutionCenterMaintanance.Shared.DTOs.Fees, TutionCenterMaintanance.Controllers, TutionCenterMaintanance.Shared.DTOs.Homework, TutionCenterMaintanance.Shared.Constants (+30 more)

### Community 2 - "IAM & Security"
Cohesion: 0.05
Nodes (45): WhatsAppController, WhatsAppTemplate, BodyTemplate, Category, IsSystem, Name, BulkWhatsAppRecipientDto, Category (+37 more)

### Community 3 - "Financial & Billing"
Cohesion: 0.04
Nodes (47): route:/fees, StudentLedgerDialog, GetAgingColor, GetModeColor, GetStatusColor, LoadAnalytics, LoadDefaulters, LoadFeeSummaries (+39 more)

### Community 4 - "Examinations & Grading"
Cohesion: 0.04
Nodes (44): route:/whatsapp, DeleteTemplate, GetSamplePreview, LoadAbsentRecipients, LoadAssessmentRecipients, LoadLogs, LoadOverdueRecipients, LoadTemplates (+36 more)

### Community 5 - "Homework Management"
Cohesion: 0.04
Nodes (43): MudCardActions, route:/homework, DeleteHomework, LoadHomework, OnBatchFilterChanged, OnInitializedAsync, OpenCreateHomeworkDialog, OpenSubmissionsDialog (+35 more)

### Community 6 - "IAM & Security"
Cohesion: 0.05
Nodes (35): DashboardController, SettingsController, DashboardStatsDto, OverdueStudentsCount, RecentActivities, TodayAbsentCount, TodayAttendancePercentage, TodayPresentCount (+27 more)

### Community 7 - "IAM & Security"
Cohesion: 0.06
Nodes (28): TimetableController, BatchWeeklyTimetableDto, BatchId, BatchName, GradeName, Slots, CreateOrUpdateTimetableRequest, BatchId (+20 more)

### Community 8 - "Examinations & Grading"
Cohesion: 0.06
Nodes (40): AcademicYear, Assessments, Batches, EndDate, IsCurrent, Name, StartDate, Board (+32 more)

### Community 9 - "IAM & Security"
Cohesion: 0.05
Nodes (38): ActivatorContent, Authorized, AuthorizeView, ChildContent, LayoutComponentBase, MudAppBar, MudBadge, MudDialogProvider (+30 more)

### Community 10 - "Student Directory & Admissions"
Cohesion: 0.05
Nodes (38): MudPagination, PagerContent, route:/students, StudentDialog, ApplyFilters, ConfirmDelete, LoadLookups, LoadStudents (+30 more)

### Community 11 - "Database Schema & EF Core"
Cohesion: 0.05
Nodes (38): route:/batches, DeleteBatch, LoadBatches, OnInitializedAsync, OpenCreateBatchDialog, OpenEditBatchDialog, AcademicYearDto, BatchDto (+30 more)

### Community 12 - "Database Schema & EF Core"
Cohesion: 0.07
Nodes (4): TutionCenterMaintanance.Migrations, InitialCreate, Phase2_FinancialAndCommunication, AppDbContextModelSnapshot

### Community 13 - "Timetable & Scheduling"
Cohesion: 0.05
Nodes (35): route:/timetable, DeleteSlot, GetDayColor, LoadSlots, OnBatchChanged, OnInitializedAsync, OnTeacherChanged, OpenCreateSlotDialog (+27 more)

### Community 14 - "Financial & Billing"
Cohesion: 0.05
Nodes (35): route:/fees/plans, LoadData, OnInitializedAsync, OpenCreateDiscountDialog, OpenCreatePlanDialog, AcademicYearDto, CardHeaderContent, DialogActions (+27 more)

### Community 15 - "IAM & Security"
Cohesion: 0.05
Nodes (34): route:/users, LoadData, OnInitializedAsync, OnRoleSelectionChanged, OpenCreateUserDialog, DialogActions, DialogContent, HeaderContent (+26 more)

### Community 16 - "IAM & Security"
Cohesion: 0.11
Nodes (10): AuthController, CreateOrUpdateRoleRequest, Description, Name, Permissions, RoleDto, Description, Id (+2 more)

### Community 17 - "IAM & Security"
Cohesion: 0.07
Nodes (21): ImportExportController, ImportStudentsRequest, CsvContent, ImportSummaryDto, Errors, FailureCount, SuccessCount, SuccessMessages (+13 more)

### Community 18 - "IAM & Security"
Cohesion: 0.06
Nodes (35): AuditLog, Action, EntityId, EntityName, Id, NewValues, OldValues, Timestamp (+27 more)

### Community 19 - "Database Schema & EF Core"
Cohesion: 0.05
Nodes (34): route:/academic-setup, AddBoard, AddSubject, AddYear, LoadAll, LoadMapping, OnInitializedAsync, OnMapBoardChanged (+26 more)

### Community 20 - "IAM & Security"
Cohesion: 0.07
Nodes (20): AuditController, BackupController, AuditLogDto, Action, EntityId, EntityName, Id, NewValues (+12 more)

### Community 21 - "Examinations & Grading"
Cohesion: 0.06
Nodes (31): AssessmentSubjectDto, route:/marks/bulk-entry, LoadGrid, OnAssessmentChanged, OnBatchChanged, OnInitializedAsync, OnSubjectChanged, AssessmentDto (+23 more)

### Community 22 - "Student Directory & Admissions"
Cohesion: 0.06
Nodes (31): route:/reports, GetCategoryColor, LoadAttendanceCorrelation, LoadBatchComparisons, LoadStudentTrend, LoadSubjectComparisons, OnInitializedAsync, OnStudentChanged (+23 more)

### Community 23 - "Database Schema & EF Core"
Cohesion: 0.06
Nodes (31): route:/teachers, DeleteTeacher, LoadTeachers, OnInitializedAsync, OpenCreateDialog, OpenEditDialog, DialogActions, DialogContent (+23 more)

### Community 24 - "IAM & Security"
Cohesion: 0.08
Nodes (23): AttendanceController, AttendanceSummaryDto, AbsentCount, AttendancePercentage, PresentCount, TotalStudents, BatchAttendanceSheetDto, BatchId (+15 more)

### Community 25 - "Examinations & Grading"
Cohesion: 0.06
Nodes (30): MudButtonGroup, route:/attendance, LoadAttendanceSheet, MarkAllAbsent, MarkAllPresent, OnBatchChanged, OnDateChanged, OnInitializedAsync (+22 more)

### Community 26 - "Examinations & Grading"
Cohesion: 0.06
Nodes (27): CreateAssessmentDialog, route:/assessments, LoadAssessments, OnInitializedAsync, OpenCreateDialog, AssessmentDto, BoardDto, GradeDto (+19 more)

### Community 27 - "Financial & Billing"
Cohesion: 0.06
Nodes (30): AppDbContext, AcademicYears, Assessments, AssessmentSubjects, AssessmentTypes, Attendances, AuditLogs, Batches (+22 more)

### Community 28 - "Financial & Billing"
Cohesion: 0.06
Nodes (28): Close, GetStatusColor, LoadLedgerAsync, OnInitializedAsync, OpenRecordPayment, DialogActions, DialogContent, HeaderContent (+20 more)

### Community 29 - "Examinations & Grading"
Cohesion: 0.06
Nodes (29): AddSubjectRow, Cancel, OnBoardChanged, OnGradeOrBoardChanged, OnInitializedAsync, AcademicYearDto, AssessmentTypeDto, BoardDto (+21 more)

### Community 31 - "Student Directory & Admissions"
Cohesion: 0.07
Nodes (26): Cancel, LoadLookups, OnInitializedAsync, BatchDto, BoardDto, DialogActions, DialogContent, GradeDto (+18 more)

### Community 32 - "Financial & Billing"
Cohesion: 0.07
Nodes (28): FeeDefaulterItemDto, AgingBracket, BoardName, FeePlanName, FormattedReminderMessage, GradeName, MaxDaysOverdue, OldestDueDate (+20 more)

### Community 33 - "Administration & Data Management"
Cohesion: 0.07
Nodes (24): route:/audit, GetActionColor, LoadLogs, OnActionChanged, OnEntityChanged, OnInitializedAsync, AuditLogDto, HeaderContent (+16 more)

### Community 34 - "Student Directory & Admissions"
Cohesion: 0.08
Nodes (25): BatchDto, AcademicYearId, AcademicYearName, BoardId, BoardName, GradeId, GradeName, Id (+17 more)

### Community 35 - "Financial & Billing"
Cohesion: 0.08
Nodes (25): Discount, IsPercentage, Name, StudentFeePlans, Type, Value, FeePlan, AcademicYear (+17 more)

### Community 36 - "IAM & Security"
Cohesion: 0.12
Nodes (3): CustomAuthStateProvider, ILocalStorageService, LocalStorageService

### Community 37 - "Examinations & Grading"
Cohesion: 0.09
Nodes (24): Assessment, AcademicYear, AcademicYearId, AssessmentSubjects, AssessmentType, AssessmentTypeId, Board, BoardId (+16 more)

### Community 38 - "Student Directory & Admissions"
Cohesion: 0.08
Nodes (23): Student, Address, AdmissionDate, Attendances, BloodGroup, Board, BoardId, DateOfBirth (+15 more)

### Community 39 - "Financial & Billing"
Cohesion: 0.08
Nodes (25): PaymentReceiptDto, AmountPaid, BoardName, CenterAddress, CenterEmail, CenterName, CenterPhone, CenterTagline (+17 more)

### Community 40 - "Examinations & Grading"
Cohesion: 0.12
Nodes (11): NotificationsController, NotificationCountDto, UnreadCount, NotificationDto, CreatedAt, Id, IsRead, Message (+3 more)

### Community 41 - "Student Directory & Admissions"
Cohesion: 0.09
Nodes (19): MudSimpleTable, route:/report-cards, LoadReportCard, OnInitializedAsync, OnStudentChanged, PrintReportCard, MudButton, MudCard (+11 more)

### Community 42 - "Financial & Billing"
Cohesion: 0.09
Nodes (19): Cancel, OnInitialized, DialogActions, DialogContent, MudButton, MudCheckBox, MudDatePicker, MudDialog (+11 more)

### Community 44 - "Database Schema & EF Core"
Cohesion: 0.09
Nodes (21): Teacher, Email, IsActive, Name, Phone, Specialization, TeacherBatchSubjects, TeacherBatchSubject (+13 more)

### Community 45 - "Financial & Billing"
Cohesion: 0.09
Nodes (22): FeeInstallment, Amount, DueDate, PaidAmount, Payments, Status, StudentFeePlan, StudentFeePlanId (+14 more)

### Community 46 - "Student Directory & Admissions"
Cohesion: 0.09
Nodes (22): StudentDto, Address, AdmissionDate, BatchIds, BatchNames, BloodGroup, BoardId, BoardName (+14 more)

### Community 47 - "IAM & Security"
Cohesion: 0.11
Nodes (17): Microsoft.AspNetCore.Authentication.JwtBearer (10.0.14), Microsoft.AspNetCore.Components.Authorization (10.0.14), Microsoft.AspNetCore.Components.WebAssembly.Server (10.0.14), Microsoft.AspNetCore.Identity.EntityFrameworkCore (10.0.14), Microsoft.EntityFrameworkCore.Sqlite (10.0.0-*), Microsoft.EntityFrameworkCore.SqlServer (10.0.14), Microsoft.EntityFrameworkCore.Tools (10.0.14), Microsoft.NET.Sdk (+9 more)

### Community 48 - "Parent Messaging & Communication"
Cohesion: 0.10
Nodes (17): route:/data-management, ImportCsv, OnInitializedAsync, MudAlert, MudButton, MudCard, MudChip, MudGrid (+9 more)

### Community 49 - "Financial & Billing"
Cohesion: 0.10
Nodes (17): Close, LoadReceiptAsync, OnInitializedAsync, PrintReceipt, DialogActions, DialogContent, MudAlert, MudButton (+9 more)

### Community 50 - "IAM & Security"
Cohesion: 0.10
Nodes (14): DataAnnotationsValidator, EditForm, route:/login, HandleLogin, MudAlert, MudButton, MudCard, MudCardContent (+6 more)

### Community 51 - "Database Schema & EF Core"
Cohesion: 0.10
Nodes (16): MudList, MudListItem, route:/, LoadDashboard, OnInitializedAsync, MudAvatar, MudButton, MudCard (+8 more)

### Community 52 - "IAM & Security"
Cohesion: 0.10
Nodes (19): Attendance, Batch, BatchId, Date, MarkedByUserId, Remarks, Status, Student (+11 more)

### Community 53 - "Student Directory & Admissions"
Cohesion: 0.11
Nodes (16): CreateStudentRequest, Address, AdmissionDate, BatchIds, BloodGroup, BoardId, DateOfBirth, FirstName (+8 more)

### Community 54 - "Homework Management"
Cohesion: 0.11
Nodes (19): HomeworkDto, AssignedDate, BatchId, BatchName, CheckedCount, Description, DueDate, GradeName (+11 more)

### Community 55 - "Parent Messaging & Communication"
Cohesion: 0.12
Nodes (17): ASPNETCORE_ENVIRONMENT, applicationUrl, commandName, dotnetRunMessages, environmentVariables, inspectUri, launchBrowser, applicationUrl (+9 more)

### Community 56 - "Financial & Billing"
Cohesion: 0.12
Nodes (16): AssignFeePlanRequest, DiscountId, FeePlanId, FinalAmount, GenerateInstallments, StartDate, StudentId, RecordPaymentRequest (+8 more)

### Community 57 - "Financial & Billing"
Cohesion: 0.12
Nodes (17): FeePlanDto, AcademicYearId, AcademicYearName, Amount, Frequency, GradeId, GradeName, Id (+9 more)

### Community 58 - "IAM & Security"
Cohesion: 0.17
Nodes (4): DynamicPermissionPolicyProvider, PermissionAuthorizationHandler, PermissionRequirement, Permission

### Community 59 - "IAM & Security"
Cohesion: 0.23
Nodes (6): AssessmentsController, AssessmentTypeDto, Category, Description, Id, Name

### Community 60 - "Financial & Billing"
Cohesion: 0.12
Nodes (16): FeeInstallmentDto, Amount, DaysOverdue, DueDate, Id, IsOverdue, PaidAmount, RemainingAmount (+8 more)

### Community 61 - "Database Schema & EF Core"
Cohesion: 0.13
Nodes (12): route:/settings, OnInitializedAsync, MudButton, MudCard, MudGrid, MudItem, MudProgressCircular, MudProgressLinear (+4 more)

### Community 62 - "Module 62 Core"
Cohesion: 0.16
Nodes (9): TeacherDto, Email, Id, IsActive, Name, Phone, Specialization, SubjectIds (+1 more)

### Community 63 - "Examinations & Grading"
Cohesion: 0.13
Nodes (15): AssessmentDto, AcademicYearId, AcademicYearName, AssessmentTypeId, AssessmentTypeName, BoardId, BoardName, Category (+7 more)

### Community 64 - "Blazor Client Shell & UI Layout"
Cohesion: 0.13
Nodes (14): DiscountDto, Id, IsPercentage, Name, Type, Value, AttendanceStatus, Absent (+6 more)

### Community 65 - "Financial & Billing"
Cohesion: 0.13
Nodes (15): PaymentDto, Amount, BoardName, FeeInstallmentId, GradeName, Id, InstallmentTitle, PaymentDate (+7 more)

### Community 66 - "Financial & Billing"
Cohesion: 0.13
Nodes (15): StudentFeeLedgerDto, BoardName, DueAmount, FatherName, FeePlanName, GradeName, Installments, PaidAmount (+7 more)

### Community 67 - "Financial & Billing"
Cohesion: 0.13
Nodes (15): StudentFeeSummaryDto, BoardName, DueAmount, FeePlanName, GradeName, NextDueDate, OverdueAmount, OverdueInstallmentsCount (+7 more)

### Community 69 - "Examinations & Grading"
Cohesion: 0.15
Nodes (11): CreateAssessmentRequest, AcademicYearId, AssessmentTypeId, BoardId, Date, GradeId, Name, Subjects (+3 more)

### Community 70 - "IAM & Security"
Cohesion: 0.21
Nodes (6): AppUser, CreatedAt, FullName, IsActive, ITokenService, TokenService

### Community 71 - "Student Directory & Admissions"
Cohesion: 0.17
Nodes (8): PaginatedResult, HasNextPage, HasPreviousPage, Items, PageNumber, PageSize, TotalCount, TotalPages

### Community 73 - "Student Directory & Admissions"
Cohesion: 0.15
Nodes (13): Batch, AcademicYear, AcademicYearId, Board, BoardId, Grade, GradeId, IsActive (+5 more)

### Community 74 - "Student Directory & Admissions"
Cohesion: 0.15
Nodes (13): BulkMarksGridDto, AssessmentId, AssessmentName, AssessmentSubjectId, MaxMarks, Rows, SubjectName, StudentMarkRowDto (+5 more)

### Community 75 - "Student Directory & Admissions"
Cohesion: 0.15
Nodes (12): StudentListItemDto, BatchNames, BoardName, FatherName, FullName, GradeName, Id, IsActive (+4 more)

### Community 76 - "Timetable & Scheduling"
Cohesion: 0.17
Nodes (10): Timetable, Batch, BatchId, DayOfWeek, EndTime, StartTime, Subject, SubjectId (+2 more)

### Community 77 - "Examinations & Grading"
Cohesion: 0.17
Nodes (12): ReportCardExamSectionDto, AssessmentName, AssessmentType, Category, Date, Marks, Percentage, TotalMaximum (+4 more)

### Community 78 - "Financial & Billing"
Cohesion: 0.17
Nodes (12): PaymentModeBreakdownDto, Mode, ModeName, Percentage, TotalAmount, TransactionCount, PaymentMode, BankTransfer (+4 more)

### Community 79 - "Student Directory & Admissions"
Cohesion: 0.17
Nodes (12): HomeworkSubmissionItemDto, GradeName, HomeworkId, ParentName, ParentPhone, Remarks, Status, StudentId (+4 more)

### Community 80 - "IAM & Security"
Cohesion: 0.18
Nodes (10): AuthorizeRouteView, Authorizing, CascadingAuthenticationState, FocusOnNavigate, Found, Router, Login, MudProgressCircular (+2 more)

### Community 81 - "IAM & Security"
Cohesion: 0.18
Nodes (3): CenterSettings, SeedData, Permissions

### Community 82 - "Module 82 Core"
Cohesion: 0.22
Nodes (6): AcademicYearDto, EndDate, Id, IsCurrent, Name, StartDate

### Community 83 - "Module 83 Core"
Cohesion: 0.24
Nodes (5): BoardDto, Code, Id, IsActive, Name

### Community 84 - "Homework Management"
Cohesion: 0.20
Nodes (8): CreateHomeworkRequest, AssignedDate, BatchId, Description, DueDate, SubjectId, TeacherId, Title

### Community 85 - "Module 85 Core"
Cohesion: 0.24
Nodes (5): SubjectDto, Code, Id, IsActive, Name

### Community 86 - "Administration & Data Management"
Cohesion: 0.18
Nodes (10): TutionCenterMaintanance.Components, Microsoft.AspNetCore.Components.Forms, Microsoft.AspNetCore.Components.Routing, Microsoft.AspNetCore.Components.Web, Microsoft.AspNetCore.Components.Web.RenderMode, Microsoft.AspNetCore.Components.Web.Virtualization, Microsoft.JSInterop, System.Net.Http.Json (+2 more)

### Community 87 - "Student Directory & Admissions"
Cohesion: 0.18
Nodes (11): ParentInfo, EmergencyContactName, EmergencyContactPhone, FatherName, FatherPhone, FatherWhatsApp, MotherName, MotherPhone (+3 more)

### Community 88 - "Student Directory & Admissions"
Cohesion: 0.18
Nodes (11): BatchPerformanceComparisonDto, AverageAcademicScore, AverageAttendancePercentage, BatchId, BatchName, BoardName, GradeName, StudentCount (+3 more)

### Community 89 - "IAM & Security"
Cohesion: 0.20
Nodes (8): AppRole, Description, RolePermissions, RolePermission, Id, Permission, Role, RoleId

### Community 90 - "IAM & Security"
Cohesion: 0.24
Nodes (7): LoginRequest, Password, UserName, LoginResponse, Expiry, Token, User

### Community 91 - "IAM & Security"
Cohesion: 0.20
Nodes (10): WhatsAppMessageLog, MessageBody, MessageCategory, RecipientName, RecipientPhone, SentAt, SentByUser, SentByUserId (+2 more)

### Community 92 - "Examinations & Grading"
Cohesion: 0.20
Nodes (9): AssessmentScoreTrendItemDto, AssessmentId, AssessmentName, AssessmentTypeName, ClassAveragePercentage, Date, MarksObtained, MaxMarks (+1 more)

### Community 93 - "IAM & Security"
Cohesion: 0.25
Nodes (7): CreateUserRequest, Email, FullName, Password, PhoneNumber, RoleName, UserName

### Community 94 - "Homework Management"
Cohesion: 0.25
Nodes (6): BulkUpdateHomeworkSubmissionsRequest, HomeworkId, Submissions, HomeworkDetailDto, Homework, Submissions

### Community 97 - "Module 97 Core"
Cohesion: 0.22
Nodes (9): CenterSettings, Address, CenterName, CurrencySymbol, Email, LogoBase64, Phone, Tagline (+1 more)

### Community 98 - "Student Directory & Admissions"
Cohesion: 0.22
Nodes (8): BulkSaveMarksRequest, AssessmentSubjectId, Marks, StudentMarkSaveItem, IsAbsent, MarksObtained, Remarks, StudentId

### Community 99 - "IAM & Security"
Cohesion: 0.22
Nodes (9): UserDto, Email, FullName, Id, IsActive, Permissions, PhoneNumber, RoleName (+1 more)

### Community 100 - "Student Directory & Admissions"
Cohesion: 0.22
Nodes (9): GradeFinancialSummaryDto, CollectionRate, DefaulterCount, GradeId, GradeName, StudentCount, TotalCollected, TotalExpected (+1 more)

### Community 101 - "Student Directory & Admissions"
Cohesion: 0.22
Nodes (9): AttendancePerformanceCorrelationDto, AcademicScorePercentage, AttendancePercentage, BatchName, CategoryStatus, GradeName, Recommendation, StudentId (+1 more)

### Community 102 - "Student Directory & Admissions"
Cohesion: 0.22
Nodes (9): SubjectPerformanceComparisonDto, AverageScorePercentage, HighestScorePercentage, LowestScorePercentage, PassPercentage, SubjectId, SubjectName, TotalAssessmentsEvaluated (+1 more)

### Community 103 - "Student Directory & Admissions"
Cohesion: 0.22
Nodes (9): ParentInfoDto, EmergencyContactName, EmergencyContactPhone, FatherName, FatherPhone, FatherWhatsApp, MotherName, MotherPhone (+1 more)

### Community 104 - "Module 104 Core"
Cohesion: 0.25
Nodes (8): BoardGradeSubjectDto, BoardId, BoardName, GradeId, GradeName, Id, SubjectId, SubjectName

### Community 105 - "Student Directory & Admissions"
Cohesion: 0.25
Nodes (6): StudentProgressTrendDto, GradeName, OverallAveragePercentage, StudentId, StudentName, TrendPoints

### Community 107 - "Module 107 Core"
Cohesion: 0.40
Nodes (4): GradeDto, Id, Name, OrderIndex

### Community 108 - "Examinations & Grading"
Cohesion: 0.33
Nodes (6): AssessmentSubjectDto, AssessmentId, Id, MaxMarks, SubjectId, SubjectName

### Community 109 - "Student Directory & Admissions"
Cohesion: 0.33
Nodes (6): StudentReportCardDto, BoardName, Exams, GradeName, StudentId, StudentName

### Community 110 - "Examinations & Grading"
Cohesion: 0.33
Nodes (6): SubjectMarkDetailDto, IsAbsent, MarksObtained, MaxMarks, Remarks, SubjectName

### Community 111 - "IAM & Security"
Cohesion: 0.33
Nodes (6): UpdateUserRequest, Email, FullName, IsActive, PhoneNumber, RoleName

### Community 112 - "Module 112 Core"
Cohesion: 0.33
Nodes (6): MonthlyCollectionDto, CollectedAmount, Month, MonthName, TransactionCount, Year

### Community 113 - "Administration & Data Management"
Cohesion: 0.40
Nodes (4): HeadOutlet, ImportMap, ResourcePreloader, Routes

### Community 114 - "Module 114 Core"
Cohesion: 0.50
Nodes (3): ChangePasswordRequest, CurrentPassword, NewPassword

### Community 115 - "Blazor Client Shell & UI Layout"
Cohesion: 0.50
Nodes (3): MudNavGroup, MudNavLink, MudNavMenu

### Community 116 - "Module 116 Core"
Cohesion: 0.50
Nodes (3): System.Diagnostics, OnInitialized, PageTitle

### Community 117 - "Module 117 Core"
Cohesion: 0.50
Nodes (4): Gender, Female, Male, Other

### Community 120 - "Student Directory & Admissions"
Cohesion: 0.67
Nodes (3): UpdateStudentRequest, Id, IsActive

## Knowledge Gaps
- **1725 isolated node(s):** `LayoutComponentBase`, `TutionCenterMaintanance.Client.Pages.Auth`, `MudThemeProvider`, `MudPopoverProvider`, `MudDialogProvider` (+1720 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 1917 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **12 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `ApiResponse` connect `Financial & Billing` to `IAM & Security`, `IAM & Security`, `IAM & Security`, `IAM & Security`, `IAM & Security`, `IAM & Security`, `IAM & Security`, `IAM & Security`, `Student Directory & Admissions`, `Examinations & Grading`, `IAM & Security`, `Student Directory & Admissions`, `IAM & Security`, `Module 62 Core`, `IAM & Security`, `Examinations & Grading`, `Student Directory & Admissions`, `IAM & Security`, `Module 82 Core`, `Module 83 Core`, `Homework Management`, `Module 85 Core`, `IAM & Security`, `IAM & Security`, `Homework Management`, `IAM & Security`, `Student Directory & Admissions`, `Student Directory & Admissions`, `Module 107 Core`, `Module 114 Core`?**
  _High betweenness centrality (0.133) - this node is a cross-community bridge._
- **Why does `AppDbContext` connect `Financial & Billing` to `IAM & Security`, `IAM & Security`, `IAM & Security`, `IAM & Security`, `Examinations & Grading`, `IAM & Security`, `IAM & Security`, `IAM & Security`, `IAM & Security`, `IAM & Security`, `IAM & Security`, `Financial & Billing`, `Examinations & Grading`, `Student Directory & Admissions`, `Examinations & Grading`, `IAM & Security`, `Database Schema & EF Core`, `Financial & Billing`, `IAM & Security`, `IAM & Security`, `IAM & Security`, `IAM & Security`, `IAM & Security`, `IAM & Security`, `Student Directory & Admissions`, `Timetable & Scheduling`, `IAM & Security`, `Student Directory & Admissions`, `IAM & Security`, `IAM & Security`, `IAM & Security`, `Student Directory & Admissions`, `Module 97 Core`?**
  _High betweenness centrality (0.080) - this node is a cross-community bridge._
- **Why does `PaymentReceiptDto` connect `Financial & Billing` to `Financial & Billing`, `IAM & Security`, `IAM & Security`, `Financial & Billing`, `Financial & Billing`?**
  _High betweenness centrality (0.024) - this node is a cross-community bridge._
- **What connects `LayoutComponentBase`, `TutionCenterMaintanance.Client.Pages.Auth`, `MudThemeProvider` to the rest of the system?**
  _1725 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Financial & Billing` be split into smaller, more focused modules?**
  _Cohesion score 0.058492336565530184 - nodes in this community are weakly interconnected._
- **Should `IAM & Security` be split into smaller, more focused modules?**
  _Cohesion score 0.06872370266479663 - nodes in this community are weakly interconnected._
- **Should `IAM & Security` be split into smaller, more focused modules?**
  _Cohesion score 0.050203527815468114 - nodes in this community are weakly interconnected._