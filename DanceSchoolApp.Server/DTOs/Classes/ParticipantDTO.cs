using System.ComponentModel.DataAnnotations;

namespace DanceSchoolApp.Server.DTOs.Classes
{
    // ─── Validation status enum ───────────────────────────────────────────────
    // Per-participant attendance confirmation, separate from CoachClassStatus.
    public enum ParticipantValidationStatus : byte
    {
        Pending = 0,  // neither party has responded yet
        ParentConfirmed = 1,  // parent confirmed student attended
        Disputed = 2   // parent said student did not attend
    }

    // ─── Responses ────────────────────────────────────────────────────────────

    public class ParticipantListResponse
    {
        public int ParticipantId { get; set; }
        public int ClassId { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = null!;
        public int ParentUserId { get; set; }
        public DateOnly JoinedAt { get; set; }
        public decimal ClassPrice { get; set; }
        public ParticipantValidationStatus ValidationStatus { get; set; }
        public DateTime? ParentValidatedAt { get; set; }
    }

    // ─── Requests ─────────────────────────────────────────────────────────────

    public class ParticipantJoinRequest
    {
        [Required]
        public int ClassId { get; set; }

        [Required]
        public int StudentId { get; set; }

        // Priority: custom override (staff-set) > AppSetting rate (weekend/weekday) > default fallback.
        // Null means "resolve from app settings at enrollment time".
        public decimal? ClassPrice { get; set; }
    }

    public class ParticipantValidateRequest
    {
        [Required]
        public bool Attended { get; set; }
    }
}