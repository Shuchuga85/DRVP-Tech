namespace DanceSchoolApp.Server.DTOs
{
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

    public class CoachActivationRequest
    {
        public int CoachId { get; set; }
        public bool IsActive { get; set; }
    }
    public class CoachCreateRequest : PersonRequest
    {
        public int UserId { get; set; }
        public string? Biography { get; set; }
        public string? PhotoUrl { get; set; }
    }
}
