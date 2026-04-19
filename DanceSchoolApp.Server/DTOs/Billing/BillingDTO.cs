namespace DanceSchoolApp.Server.DTOs.Billing
{
    // ─── Student billing ──────────────────────────────────────────────────────

    public class BillingStudentSummary
    {
        public int TotalStudents { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalHours { get; set; }
        public int PendingCount { get; set; }   // always 0 — no payment table yet
    }

    public class BillingStudentRow
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = null!;
        public decimal HoursCompleted { get; set; }
        public decimal TotalAmount { get; set; }
        public string? PaymentStatus { get; set; }    // null — deferred
        public DateTime? LastPaymentDate { get; set; } // null — deferred
    }

    public class PagedBillingStudentResponse
    {
        public BillingStudentSummary Summary { get; set; } = null!;
        public List<BillingStudentRow> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    // ─── Coach billing ────────────────────────────────────────────────────────

    public class BillingCoachSummary
    {
        public int TotalCoaches { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal TotalHours { get; set; }
        public int PendingCount { get; set; }   // always 0 — no payment table yet
    }

    public class BillingCoachRow
    {
        public int CoachId { get; set; }
        public string CoachName { get; set; } = null!;
        public List<string> Modalities { get; set; } = new();
        public decimal HoursTaught { get; set; }
        public decimal RatePerHour { get; set; }
        public decimal TotalAmount { get; set; }
        public string? PaymentStatus { get; set; }    // null — deferred
        public DateTime? LastPaymentDate { get; set; } // null — deferred
    }

    public class PagedBillingCoachResponse
    {
        public BillingCoachSummary Summary { get; set; } = null!;
        public List<BillingCoachRow> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
