using System.ComponentModel.DataAnnotations;

namespace LSC.MentorByKarthik.Application.DTOs
{
    public class UserProfileDto
    {
        public string DisplayName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        [StringLength(100)]
        public string Email { get; set; } = null!;

        [StringLength(128)]
        public string AdObjId { get; set; } = null!;

        [StringLength(100)]
        public string? MyTimeZone { get; set; } = string.Empty;

        

        [StringLength(500)]
        public string? ProfileImageUrl { get; set; } = string.Empty;

        [StringLength(100)]
        public string? PreferedLanguage { get; set; } = string.Empty;
        public string UserRole { get; set; } = string.Empty;

    }

    public class UpdateUserPreferencesDto
    {
        public string? MyTimeZone { get; set; }
        public string? PreferedLanguage { get; set; }
    }

}
