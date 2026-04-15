using System.ComponentModel.DataAnnotations;

namespace DanceSchoolApp.Server.DTOs
{
    public class AppSettingResponse
    {
        public int SettingId { get; set; }
        public string? Key { get; set; }
        public string? Value { get; set; }
        public DateOnly? UpdatedAt { get; set; }
    }

    public class AppSettingUpdateRequest
    {
        [Required]
        [MaxLength(128)]
        public string Value { get; set; } = null!;
    }
}
