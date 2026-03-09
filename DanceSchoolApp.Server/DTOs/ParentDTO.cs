namespace DanceSchoolApp.Server.DTOs
{
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

    public class ParentActivationRequest
    {
        public int ParentId { get; set; }
        public bool IsActive { get; set; }
    }
    public class ParentCreateRequest : PersonRequest
    {
        public int UserId { get; set; }
    }
}
