namespace DanceSchoolApp.Server.DTOs
{
    public class PersonRequest
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateOnly BirthDate { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }

    public class StaffRequest : PersonRequest
    {
        public int UserId { get; set; }
        public string Position { get; set; }
    }

    public class ParentRequest : PersonRequest
    {
        public int UserId { get; set; }
    }

    public class CoachRequest : PersonRequest
    {
        public int UserId { get; set; }
        public string Biography { get; set; }
        public string PhotoUrl { get; set; }
    }

    public class StudentRequest : PersonRequest 
    {
        public int ParentId { get; set; }
    }

}
