namespace DanceSchoolApp.Server.DTOs
{
    public class StudentListResponse
    {
        public int StudentId { get; set; }
        public int IdParent { get; set; }
        public bool IsActive { get; set; }
        public PersonListResponse? PersonInfo { get; set; }
    }
    public class StudentDetailResponse
    {
        public int StudentId { get; set; }
        public int IdParent { get; set; }
        public bool IsActive { get; set; }
        public PersonDetailResponse? PersonInfo { get; set; }
    }

    public class StudentCreateRequest : PersonRequest
    {
        public int ParentId { get; set; }
    }

    public class StudentActivationRequest
    {
        public int StudentId { get; set; }
        public bool IsActive { get; set; }
    }


}
