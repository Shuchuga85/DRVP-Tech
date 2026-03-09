namespace DanceSchoolApp.Server.DTOs
{
    public class StaffListResponse
    {
        public int StaffId { get; set; }
        public string? Position { get; set; }
        public bool IsActive { get; set; }
        public PersonListResponse? PersonInfo { get; set; }
    }
    public class StaffDetailResponse
    {
        public int StaffId { get; set; }
        public string? Position { get; set; }
        public bool IsActive { get; set; }
        public PersonDetailResponse? PersonInfo { get; set; }
    }

    public class StaffActivationRequest
    {
        public int StaffId { get; set; }
        public bool IsActive { get; set; }
    }

    public class StaffCreateRequest : PersonRequest
    {
        public int UserId { get; set; }
        public string? Position { get; set; }
    }
}
