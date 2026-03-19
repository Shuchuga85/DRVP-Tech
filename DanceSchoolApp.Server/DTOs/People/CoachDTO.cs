namespace DanceSchoolApp.Server.DTOs.People
{
    // ─── Responses ────────────────────────────────────────────────────────────

    public class CoachListResponse
    {
        public int CoachId { get; set; }
        public string? Biography { get; set; }
        public string? PhotoUrl { get; set; }
        public bool IsActive { get; set; }
        public PersonListResponse? PersonInfo { get; set; }
    }
    public class CoachDetailResponse
    {
        public int CoachId { get; set; }
        public string? Biography { get; set; }
        public string? PhotoUrl { get; set; }
        public bool IsActive { get; set; }
        public PersonDetailResponse? PersonInfo { get; set; }
    }

}
