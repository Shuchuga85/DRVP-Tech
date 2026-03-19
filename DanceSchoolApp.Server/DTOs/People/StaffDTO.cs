namespace DanceSchoolApp.Server.DTOs.People
{

    // ─── Responses ────────────────────────────────────────────────────────────

    public class StaffListResponse
    {
        public int StaffId { get; set; }
        public bool IsActive { get; set; }
        public PersonListResponse? PersonInfo { get; set; }
    }
    public class StaffDetailResponse
    {
        public int StaffId { get; set; }
        public bool IsActive { get; set; }
        public PersonDetailResponse? PersonInfo { get; set; }
    }

}
