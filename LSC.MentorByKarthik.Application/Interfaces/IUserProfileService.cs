using LSC.MentorByKarthik.Application.DTOs;
using LSC.MentorByKarthik.Domain.Entities;

namespace LSC.MentorByKarthik.Application.Interfaces
{
    public interface IUserProfileService
    {
        Task UpdateUserProfilePicture(int userId, string pictureUrl);

        Task<UserProfile?> GetUserInfoAsync(int userId);
        Task<UserProfile?> GetUserInfoAsync(string emailId);

        Task<int> CreateOrUpdateUserProfileAsync(UserProfileDto dto);
        Task UpdateUserPreferencesAsync(int userId, UpdateUserPreferencesDto dto);
        Task<UserProfileDto?> GetUserProfileByEmailAsync(string email, string adObjId);
        Task<string> GetUserRolesAsync(int userId);
        Task<string> GetUserRolesAsync(string email);

    }
}
