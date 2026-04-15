using System.ComponentModel.DataAnnotations;

namespace DanceSchoolApp.Server.DTOs.People
{

    // ─── Responses ────────────────────────────────────────────────────────────

    public class StudentListResponse
    {
        public int StudentId { get; set; }
        public int ParentUserId { get; set; }
        public bool IsActive { get; set; }
        public PersonListResponse? PersonInfo { get; set; }
    }

    public class StudentDetailResponse
    {
        public int StudentId { get; set; }
        public int ParentUserId { get; set; }
        public bool IsActive { get; set; }
        public PersonDetailResponse? PersonInfo { get; set; }
    }


    // ─── Requests ─────────────────────────────────────────────────────────────

    public class StudentCreateRequest
    {
        [Required]
        public int ParentId { get; set; }

        [Required]
        [MaxLength(64)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(64)]
        public string LastName { get; set; } = null!;

        public DateOnly? BirthDate { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }

        [MaxLength(128)]
        public string? Address { get; set; }

        [MaxLength(9)]
        public string? Nif { get; set; }
    }

    public class StudentUpdateRequest
    {
        [Required]
        [MaxLength(64)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(64)]
        public string LastName { get; set; } = null!;

        public DateOnly? BirthDate { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }

        [MaxLength(128)]
        public string? Address { get; set; }
        [MaxLength(9)]
        public string? Nif { get; set; }
    }

}
