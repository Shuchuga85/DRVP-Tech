namespace DanceSchoolApp.Server.DTOs.People
{

    public class PersonRequest
    {
        public string? FirstName { get; set; } = null!;
        public string? LastName { get; set; } = null!;
        public DateOnly? BirthDate { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }

    public class PersonDetailResponse
    {
        public int? PersonId { get; set; }
        public string? FirstName { get; set; } = null!;
        public string? LastName { get; set; } = null!;
        public DateOnly? BirthDate { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
    public class PersonListResponse
    {
        public int? PersonId { get; set; }
        public string? FirstName { get; set; } = null!;
        public string? LastName { get; set; } = null!;
    }

}
