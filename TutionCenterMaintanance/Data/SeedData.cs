using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TutionCenterMaintanance.Data.Entities;
using TutionCenterMaintanance.Shared.Constants;
using TutionCenterMaintanance.Shared.Enums;

namespace TutionCenterMaintanance.Data;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

        // Ensure local SQLite database and schema are created
        await context.Database.EnsureCreatedAsync();

        // 1. Seed Roles & Permissions
        var superAdminRole = await roleManager.FindByNameAsync("SuperAdmin");
        if (superAdminRole == null)
        {
            superAdminRole = new AppRole
            {
                Name = "SuperAdmin",
                Description = "System Administrator with complete access"
            };
            await roleManager.CreateAsync(superAdminRole);
        }

        var allPermissions = Permissions.GetAll();
        var existingPermissions = await context.RolePermissions
            .Where(rp => rp.RoleId == superAdminRole.Id)
            .Select(rp => rp.Permission)
            .ToListAsync();

        foreach (var perm in allPermissions)
        {
            if (!existingPermissions.Contains(perm))
            {
                context.RolePermissions.Add(new RolePermission
                {
                    RoleId = superAdminRole.Id,
                    Permission = perm
                });
            }
        }
        await context.SaveChangesAsync();

        // 2. Seed SubAdmin Role (limited permissions for data entry)
        var subAdminRole = await roleManager.FindByNameAsync("SubAdmin");
        if (subAdminRole == null)
        {
            subAdminRole = new AppRole
            {
                Name = "SubAdmin",
                Description = "Staff / Tutor with student data entry & marks management"
            };
            await roleManager.CreateAsync(subAdminRole);

            var subAdminPerms = new[]
            {
                Permissions.StudentsView, Permissions.StudentsCreate, Permissions.StudentsEdit,
                Permissions.AttendanceView, Permissions.AttendanceMark,
                Permissions.MarksView, Permissions.MarksEntry, Permissions.MarksEdit,
                Permissions.FeesView, Permissions.PaymentsRecord,
                Permissions.ReportsView, Permissions.MessagingSend
            };

            foreach (var perm in subAdminPerms)
            {
                context.RolePermissions.Add(new RolePermission
                {
                    RoleId = subAdminRole.Id,
                    Permission = perm
                });
            }
            await context.SaveChangesAsync();
        }

        // 3. Seed Default Admin User
        var adminUser = await userManager.FindByNameAsync("admin");
        if (adminUser == null)
        {
            adminUser = new AppUser
            {
                UserName = "admin",
                Email = "admin@brightminds.edu",
                FullName = "Administrator",
                IsActive = true,
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow
            };
            var result = await userManager.CreateAsync(adminUser, "Admin@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "SuperAdmin");
            }
        }

        // 4. Seed Academic Year if empty
        if (!await context.AcademicYears.AnyAsync())
        {
            context.AcademicYears.Add(new AcademicYear
            {
                Name = "2026-2027",
                StartDate = new DateTime(2026, 4, 1),
                EndDate = new DateTime(2027, 3, 31),
                IsCurrent = true
            });
            await context.SaveChangesAsync();
        }

        // 5. Seed Boards if empty
        if (!await context.Boards.AnyAsync())
        {
            context.Boards.AddRange(
                new Board { Name = "CBSE", Code = "CBSE", IsActive = true },
                new Board { Name = "ICSE", Code = "ICSE", IsActive = true },
                new Board { Name = "State Board", Code = "STATE", IsActive = true }
            );
            await context.SaveChangesAsync();
        }

        // 6. Seed Subjects if empty
        if (!await context.Subjects.AnyAsync())
        {
            context.Subjects.AddRange(
                new Subject { Name = "Mathematics", Code = "MATH", IsActive = true },
                new Subject { Name = "Science", Code = "SCI", IsActive = true },
                new Subject { Name = "Physics", Code = "PHY", IsActive = true },
                new Subject { Name = "Chemistry", Code = "CHEM", IsActive = true },
                new Subject { Name = "Biology", Code = "BIO", IsActive = true },
                new Subject { Name = "English", Code = "ENG", IsActive = true },
                new Subject { Name = "Social Science", Code = "SOC", IsActive = true },
                new Subject { Name = "Computer Science", Code = "CS", IsActive = true },
                new Subject { Name = "Tamil", Code = "TAM", IsActive = true },
                new Subject { Name = "Hindi", Code = "HIN", IsActive = true }
            );
            await context.SaveChangesAsync();
        }

        // 7. Seed Assessment Types if empty
        if (!await context.AssessmentTypes.AnyAsync())
        {
            context.AssessmentTypes.AddRange(
                new AssessmentType { Name = "Weekly Test", Category = AssessmentCategory.TuitionTest, Description = "Regular tuition center weekly review test" },
                new AssessmentType { Name = "Monthly Assessment", Category = AssessmentCategory.TuitionTest, Description = "Tuition internal monthly assessment" },
                new AssessmentType { Name = "Mock Exam", Category = AssessmentCategory.TuitionTest, Description = "Comprehensive mock test for board preparation" },
                new AssessmentType { Name = "School Quarterly Exam", Category = AssessmentCategory.SchoolExam, Description = "Quarterly examination conducted by student's school" },
                new AssessmentType { Name = "School Half-Yearly Exam", Category = AssessmentCategory.SchoolExam, Description = "Half-yearly examination conducted by student's school" },
                new AssessmentType { Name = "School Annual Exam", Category = AssessmentCategory.SchoolExam, Description = "Annual final board/school exam" }
            );
            await context.SaveChangesAsync();
        }

        // 8. Seed Discounts if empty
        if (!await context.Discounts.AnyAsync())
        {
            context.Discounts.AddRange(
                new Discount { Name = "Merit Scholarship", Type = DiscountType.Scholarship, Value = 25, IsPercentage = true },
                new Discount { Name = "Sibling Discount", Type = DiscountType.SiblingDiscount, Value = 15, IsPercentage = true },
                new Discount { Name = "Early Bird Discount", Type = DiscountType.EarlyBird, Value = 10, IsPercentage = true }
            );
            await context.SaveChangesAsync();
        }

        // 9. Seed WhatsApp Templates if empty
        if (!await context.WhatsAppTemplates.AnyAsync())
        {
            context.WhatsAppTemplates.AddRange(
                new WhatsAppTemplate
                {
                    Name = "Tuition Fee Overdue Reminder",
                    Category = "FeeReminder",
                    IsSystem = true,
                    BodyTemplate = "Dear {ParentName},\n\nThis is a friendly reminder from {CenterName}.\nThe tuition fee for {StudentName} ({Grade} - {Board}) of ₹{DueAmount} is pending. Kindly clear the dues at your earliest convenience.\n\nThank you for your cooperation.\nBest regards,\n{CenterName}\nPhone: {CenterPhone}"
                },
                new WhatsAppTemplate
                {
                    Name = "Payment Receipt Confirmation",
                    Category = "PaymentConfirmation",
                    IsSystem = true,
                    BodyTemplate = "Dear {ParentName},\n\nWe have received payment of ₹{Amount} for {StudentName} ({Grade}).\nReceipt Number: {ReceiptNumber}\nPayment Mode: {PaymentMode}\nRemaining Balance: ₹{RemainingBalance}\n\nThank you!\n{CenterName}"
                },
                new WhatsAppTemplate
                {
                    Name = "Test & Exam Score Report",
                    Category = "ExamResult",
                    IsSystem = true,
                    BodyTemplate = "Dear {ParentName},\n\nAssessment Result for {StudentName} ({Grade}):\nTest: {AssessmentName}\nScore: {MarksObtained} / {TotalMarks} ({Percentage}%)\nTeacher's Remarks: {Remarks}\n\nKeep encouraging your child!\n{CenterName}"
                },
                new WhatsAppTemplate
                {
                    Name = "Student Absence Alert",
                    Category = "Attendance",
                    IsSystem = true,
                    BodyTemplate = "Dear {ParentName},\n\nAttendance Notice from {CenterName}:\n{StudentName} ({Grade}) was marked ABSENT today ({AttendanceDate}) for tuition classes.\nKindly ensure regular attendance for consistent academic progress.\n\nRegards,\n{CenterName}"
                },
                new WhatsAppTemplate
                {
                    Name = "General Announcement / Holiday",
                    Category = "Announcement",
                    IsSystem = true,
                    BodyTemplate = "Dear Parents,\n\nNotice from {CenterName}:\n{MessageContent}\n\nFor any queries, contact {CenterPhone}.\nWarm regards,\n{CenterName}"
                }
            );
            await context.SaveChangesAsync();
        }

        // 10. Seed Initial System Notifications if empty
        if (!await context.Notifications.AnyAsync())
        {
            context.Notifications.AddRange(
                new Notification
                {
                    Title = "Welcome to Tuition Center Management System",
                    Message = "Phase 2 Financial & Communication features are now active.",
                    Type = "Success",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                }
            );
            await context.SaveChangesAsync();
        }

        // 11. Seed Center Settings if empty
        if (!await context.CenterSettings.AnyAsync())
        {
            context.CenterSettings.Add(new CenterSettings
            {
                CenterName = "Bright Minds Tuition Center",
                Tagline = "Excellence in Learning from UKG to Grade 12",
                Address = "123 Academic Avenue, Knowledge City",
                Phone = "+91 98765 43210",
                Email = "support@brightminds.edu"
            });
            await context.SaveChangesAsync();
        }
    }
}
