using System.Text.Json.Serialization;

namespace TutionCenterMaintanance.Shared.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Gender
{
    Male = 1,
    Female = 2,
    Other = 3
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AttendanceStatus
{
    Present = 1,
    Absent = 2
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PaymentMode
{
    Cash = 1,
    UPI = 2,
    BankTransfer = 3,
    Cheque = 4,
    Other = 5
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PaymentStatus
{
    Pending = 1,
    PartiallyPaid = 2,
    Paid = 3,
    Overdue = 4
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AssessmentCategory
{
    TuitionTest = 1,
    SchoolExam = 2
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DiscountType
{
    Scholarship = 1,
    SiblingDiscount = 2,
    EarlyBird = 3,
    Custom = 4
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ExpenseCategory
{
    Rent = 1,
    Electricity = 2,
    WaterBill = 3,
    Salary = 4,
    Maintenance = 5,
    Equipment = 6,
    Internet = 7,
    Miscellaneous = 8
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum FeeFrequency
{
    Monthly = 1,
    Quarterly = 2,
    HalfYearly = 3,
    Yearly = 4,
    OneTime = 5,
    PerSubject = 6
}
