using System.ComponentModel.DataAnnotations;

namespace DanceSchoolApp.Server.DTOs.Social
{
    // ─── Responses ────────────────────────────────────────────────────────────

    public class NewsPostListResponse
    {
        public int PostId { get; set; }
        public string Title { get; set; } = null!;
        public string? Subtitle { get; set; }
        public string? ImageUrl { get; set; }
        public DateOnly? CreatedAt { get; set; }
        public string? CreatedByName { get; set; }
    }

    public class NewsPostDetailResponse
    {
        public int PostId { get; set; }
        public string Title { get; set; } = null!;
        public string? Subtitle { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public DateOnly? CreatedAt { get; set; }
        public int? CreatedByUserId { get; set; }
        public string? CreatedByName { get; set; }
    }

    // ─── Requests ─────────────────────────────────────────────────────────────

    public class NewsPostCreateRequest
    {
        [Required]
        [MaxLength(64)]
        public string Title { get; set; } = null!;

        [MaxLength(128)]
        public string? Subtitle { get; set; }

        [MaxLength(256)]
        public string? Description { get; set; }

        [MaxLength(256)]
        public string? ImageUrl { get; set; }
    }

    public class NewsPostUpdateRequest
    {
        [Required]
        [MaxLength(64)]
        public string Title { get; set; } = null!;

        [MaxLength(128)]
        public string? Subtitle { get; set; }

        [MaxLength(256)]
        public string? Description { get; set; }

        [MaxLength(256)]
        public string? ImageUrl { get; set; }
    }
}