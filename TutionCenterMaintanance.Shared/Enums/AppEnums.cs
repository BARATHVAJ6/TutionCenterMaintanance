namespace TutionCenterMaintanance.Shared.Enums;

public enum Gender
{
    Male = 1,
    Female = 2,
    Other = 3
}

public enum AttendanceStatus
{
    Present = 1,
    Absent = 2
}

public enum PaymentMode
{
    Cash = 1,
    UPI = 2,
    BankTransfer = 3,
    Cheque = 4,
    Other = 5
}

public enum PaymentStatus
{
    Pending = 1,
    PartiallyPaid = 2,
    Paid = 3,
    Overdue = 4
}

public enum AssessmentCategory
{
    TuitionTest = 1,
    SchoolExam = 2
}

public enum DiscountType
{
    Scholarship = 1,
    SiblingDiscount = 2,
    EarlyBird = 3,
    Custom = 4
}

public enum FeeFrequency
{
    Monthly = 1,
    Quarterly = 2,
    HalfYearly = 3,
    Yearly = 4,
    OneTime = 5,
    PerSubject = 6
}
