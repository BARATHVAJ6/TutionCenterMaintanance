using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TutionCenterMaintanance.Data.Entities;

namespace TutionCenterMaintanance.Data;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, string>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<AcademicYear> AcademicYears => Set<AcademicYear>();
    public DbSet<Board> Boards => Set<Board>();
    public DbSet<Grade> Grades => Set<Grade>();
    public DbSet<Subject> Subjects => Set<Subject>();
    public DbSet<BoardGradeSubject> BoardGradeSubjects => Set<BoardGradeSubject>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<ParentInfo> ParentInfos => Set<ParentInfo>();
    public DbSet<Batch> Batches => Set<Batch>();
    public DbSet<StudentBatch> StudentBatches => Set<StudentBatch>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<TeacherBatchSubject> TeacherBatchSubjects => Set<TeacherBatchSubject>();
    public DbSet<Attendance> Attendances => Set<Attendance>();
    public DbSet<AssessmentType> AssessmentTypes => Set<AssessmentType>();
    public DbSet<Assessment> Assessments => Set<Assessment>();
    public DbSet<AssessmentSubject> AssessmentSubjects => Set<AssessmentSubject>();
    public DbSet<Mark> Marks => Set<Mark>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Expense> Expenses => Set<Expense>();
    public DbSet<StationeryItem> StationeryItems => Set<StationeryItem>();
    public DbSet<StationeryPurchase> StationeryPurchases => Set<StationeryPurchase>();
    public DbSet<StationerySale> StationerySales => Set<StationerySale>();
    public DbSet<Timetable> Timetables => Set<Timetable>();
    public DbSet<Homework> Homeworks => Set<Homework>();
    public DbSet<HomeworkSubmission> HomeworkSubmissions => Set<HomeworkSubmission>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<WhatsAppTemplate> WhatsAppTemplates => Set<WhatsAppTemplate>();
    public DbSet<WhatsAppMessageLog> WhatsAppMessageLogs => Set<WhatsAppMessageLog>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<CenterSettings> CenterSettings => Set<CenterSettings>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Global query filter for soft deletes
        builder.Entity<AcademicYear>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<Board>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<Grade>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<Subject>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<BoardGradeSubject>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<Student>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<ParentInfo>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<Batch>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<Teacher>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<TeacherBatchSubject>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<Attendance>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<AssessmentType>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<Assessment>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<AssessmentSubject>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<Mark>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<Payment>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<Expense>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<StationeryItem>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<StationeryPurchase>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<StationerySale>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<Timetable>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<Homework>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<HomeworkSubmission>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<Notification>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<WhatsAppTemplate>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<WhatsAppMessageLog>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<CenterSettings>().HasQueryFilter(e => !e.IsDeleted);

        // Student - ParentInfo 1:1
        builder.Entity<Student>()
            .HasOne(s => s.Parent)
            .WithOne(p => p.Student)
            .HasForeignKey<ParentInfo>(p => p.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        // StudentBatch composite key
        builder.Entity<StudentBatch>()
            .HasKey(sb => new { sb.StudentId, sb.BatchId });

        // Set default DeleteBehavior to Restrict across foreign keys to prevent cascade cycles
        foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            if (relationship.DeclaringEntityType.ClrType != typeof(ParentInfo))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        // Decimal precision (18, 2)
        foreach (var property in builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
        {
            property.SetPrecision(18);
            property.SetScale(2);
        }

        // Seed default Grades (LKG, UKG to 12th Std)
        builder.Entity<Grade>().HasData(
            new Grade { Id = 14, Name = "LKG", OrderIndex = -1, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Grade { Id = 1, Name = "UKG", OrderIndex = 0, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Grade { Id = 2, Name = "1st Std", OrderIndex = 1, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Grade { Id = 3, Name = "2nd Std", OrderIndex = 2, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Grade { Id = 4, Name = "3rd Std", OrderIndex = 3, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Grade { Id = 5, Name = "4th Std", OrderIndex = 4, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Grade { Id = 6, Name = "5th Std", OrderIndex = 5, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Grade { Id = 7, Name = "6th Std", OrderIndex = 6, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Grade { Id = 8, Name = "7th Std", OrderIndex = 7, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Grade { Id = 9, Name = "8th Std", OrderIndex = 8, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Grade { Id = 10, Name = "9th Std", OrderIndex = 9, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Grade { Id = 11, Name = "10th Std", OrderIndex = 10, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Grade { Id = 12, Name = "11th Std", OrderIndex = 11, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Grade { Id = 13, Name = "12th Std", OrderIndex = 12, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );

        // Seed default CenterSettings
        builder.Entity<CenterSettings>().HasData(
            new CenterSettings
            {
                Id = 1,
                CenterName = "Sai Tuition Center",
                Tagline = "Excellence in Learning from UKG to Grade 12",
                Address = "123 Education Lane, Knowledge City",
                Phone = "+91 98765 43210",
                Email = "info@brightminds.edu",
                CurrencySymbol = "₹",
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}
