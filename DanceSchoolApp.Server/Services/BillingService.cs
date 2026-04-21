using DanceSchoolApp.Server.Data;
using DanceSchoolApp.Server.DTOs.Billing;
using DanceSchoolApp.Server.DTOs.Classes;
using DanceSchoolApp.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace DanceSchoolApp.Server.Services
{
    public class BillingService
    {
        private readonly AppDbContext _context;
        private readonly AppSettingService _appSettings;

        public BillingService(AppDbContext context, AppSettingService appSettings)
        {
            _context  = context;
            _appSettings = appSettings;
        }

        // ─── Student billing ──────────────────────────────────────────────────
        // Sums hours and revenue per student across all Validated classes in the
        // requested month. Summary reflects the full month; search + paging apply
        // only to the Items list.

        public async Task<PagedBillingStudentResponse> GetStudentBillingAsync(
            int year, int month, string? search, int page, int pageSize)
        {
            decimal weekdayRate = await _appSettings.GetDecimalAsync(
                "class_price_weekday", 36.00m);
            decimal weekendRate = await _appSettings.GetDecimalAsync(
                "class_price_weekend", 43.50m);

            // One query: validated classes in the month, with every participant's
            // student record eagerly loaded.
            var classes = await _context.CoachClasses
                .Include(c => c.Participants)
                    .ThenInclude(p => p.IdStudentNavigation)
                        .ThenInclude(s => s.PersonInfo)
                .Where(c =>
                    c.Status == (byte)CoachClassStatus.Validated &&
                    c.StartDatetime.Year  == year &&
                    c.StartDatetime.Month == month)
                .ToListAsync();

            // Aggregate per-student in-memory so the rate branch stays in C#.
            var studentTotals = new Dictionary<int, (string Name, decimal Hours, decimal Amount)>();

            foreach (var cls in classes)
            {
                decimal durationHours = DurationHours(cls);
                bool isWeekend = cls.StartDatetime.DayOfWeek is
                    DayOfWeek.Saturday or DayOfWeek.Sunday;
                decimal rate   = isWeekend ? weekendRate : weekdayRate;
                decimal amount = durationHours * rate;

                foreach (var p in cls.Participants)
                {
                    var student = p.IdStudentNavigation;
                    int sid = student.StudentId;
                    string name = ResolveStudentName(student);

                    if (studentTotals.TryGetValue(sid, out var existing))
                        studentTotals[sid] = (name,
                            existing.Hours  + durationHours,
                            existing.Amount + amount);
                    else
                        studentTotals[sid] = (name, durationHours, amount);
                }
            }

            // Build the full row list (for summary totals + search + paging).
            var allRows = studentTotals
                .Select(kv => new BillingStudentRow
                {
                    StudentId      = kv.Key,
                    StudentName    = kv.Value.Name,
                    HoursCompleted = Math.Round(kv.Value.Hours,  2),
                    TotalAmount    = Math.Round(kv.Value.Amount, 2),
                    PaymentStatus  = null,
                    LastPaymentDate = null
                })
                .OrderBy(r => r.StudentName)
                .ToList();

            // Summary covers the entire month, not just the search slice.
            var summary = new BillingStudentSummary
            {
                TotalStudents = allRows.Count,
                TotalRevenue  = allRows.Sum(r => r.TotalAmount),
                TotalHours    = allRows.Sum(r => r.HoursCompleted),
                PendingCount  = 0
            };

            // Apply optional name search.
            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLowerInvariant();
                allRows = allRows
                    .Where(r => r.StudentName.ToLowerInvariant().Contains(term))
                    .ToList();
            }

            var pagedItems = allRows
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedBillingStudentResponse
            {
                Summary    = summary,
                Items      = pagedItems,
                TotalCount = allRows.Count,
                Page       = page,
                PageSize   = pageSize
            };
        }

        // ─── Coach billing ────────────────────────────────────────────────────
        // Sums hours taught and school expense per coach across all Validated
        // classes in the requested month.

        public async Task<PagedBillingCoachResponse> GetCoachBillingAsync(
            int year, int month, string? search, int page, int pageSize)
        {
            decimal weekdayRate = await _appSettings.GetDecimalAsync(
                "class_price_weekday", 36.00m);
            decimal weekendRate = await _appSettings.GetDecimalAsync(
                "class_price_weekend", 43.50m);

            // One query: validated classes in the month, with coach → user info
            // and coach modalities.
            var classes = await _context.CoachClasses
                .Include(c => c.IdCoachNavigation)
                    .ThenInclude(coach => coach.CoachNavigation)
                        .ThenInclude(u => u.PersonInfo)
                .Include(c => c.IdCoachNavigation)
                    .ThenInclude(coach => coach.IdModalities)
                .Where(c =>
                    c.Status == (byte)CoachClassStatus.Validated &&
                    c.StartDatetime.Year  == year &&
                    c.StartDatetime.Month == month)
                .ToListAsync();

            // Group by coach and accumulate hours + amount using weekday/weekend
            // rates (same pricing as student billing).
            var coachTotals = new Dictionary<int, (Coach Coach, decimal Hours, decimal Amount)>();

            foreach (var cls in classes)
            {
                decimal durationHours = DurationHours(cls);
                bool isWeekend = cls.StartDatetime.DayOfWeek is
                    DayOfWeek.Saturday or DayOfWeek.Sunday;
                decimal rate   = isWeekend ? weekendRate : weekdayRate;
                decimal amount = durationHours * rate;

                int cid = cls.IdCoach;
                if (coachTotals.TryGetValue(cid, out var existing))
                    coachTotals[cid] = (existing.Coach,
                        existing.Hours  + durationHours,
                        existing.Amount + amount);
                else
                    coachTotals[cid] = (cls.IdCoachNavigation, durationHours, amount);
            }

            var allRows = coachTotals
                .Select(kv =>
                {
                    var coach      = kv.Value.Coach;
                    var modalities = coach.IdModalities
                        .Select(m => m.Name)
                        .OrderBy(n => n)
                        .ToList();

                    return new BillingCoachRow
                    {
                        CoachId         = kv.Key,
                        CoachName       = ResolveCoachName(coach),
                        Modalities      = modalities,
                        HoursTaught     = Math.Round(kv.Value.Hours,  2),
                        TotalAmount     = Math.Round(kv.Value.Amount, 2),
                        PaymentStatus   = null,
                        LastPaymentDate = null
                    };
                })
                .OrderBy(r => r.CoachName)
                .ToList();

            // Summary covers the entire month.
            var summary = new BillingCoachSummary
            {
                TotalCoaches = allRows.Count,
                TotalExpense = allRows.Sum(r => r.TotalAmount),
                TotalHours   = allRows.Sum(r => r.HoursTaught),
                PendingCount = 0
            };

            // Apply optional name search.
            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLowerInvariant();
                allRows = allRows
                    .Where(r => r.CoachName.ToLowerInvariant().Contains(term))
                    .ToList();
            }

            var pagedItems = allRows
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedBillingCoachResponse
            {
                Summary    = summary,
                Items      = pagedItems,
                TotalCount = allRows.Count,
                Page       = page,
                PageSize   = pageSize
            };
        }

        // ─── Private helpers ──────────────────────────────────────────────────

        private static decimal DurationHours(CoachClass cls) =>
            (decimal)(cls.EndDatetime - cls.StartDatetime).TotalMinutes / 60.0m;

        private static string ResolveStudentName(Student student)
        {
            var p = student.PersonInfo;
            return p is not null
                ? $"{p.FirstName} {p.LastName}".Trim()
                : $"Student {student.StudentId}";
        }

        private static string ResolveCoachName(Coach coach)
        {
            var p = coach.CoachNavigation?.PersonInfo;
            return p is not null
                ? $"{p.FirstName} {p.LastName}".Trim()
                : coach.CoachNavigation?.Username ?? $"Coach {coach.CoachId}";
        }
    }
}
