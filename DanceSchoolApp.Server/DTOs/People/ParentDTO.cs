namespace DanceSchoolApp.Server.DTOs.People
{

    // ─── Responses ────────────────────────────────────────────────────────────

    public class ParentListResponse
    {
        public int ParentId { get; set; }
        public bool IsActive { get; set; }
        public PersonListResponse? PersonInfo { get; set; }
    }
    public class ParentDetailResponse
    {
        public int ParentId { get; set; }
        public bool IsActive { get; set; }
        public PersonDetailResponse? PersonInfo { get; set; }
    }

}
