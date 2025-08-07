using LSC.MentorByKarthik.Domain.Entities;

namespace LSC.MentorByKarthik.Application.Interfaces
{
    public interface IUserProfileRepository
    {
        Task UpdateUserProfilePicture(int userId, string pictureUrl);

        Task<UserProfile?> GetUserInfoAsync(int userId);
        Task<UserProfile?> GetUserInfoAsync(string emailId);
        Task<UserProfile?> GetByEmailAsync(string email, string adObjId);
        Task<UserProfile?> GetByIdAsync(int id);
        Task<int> CreateAsync(UserProfile profile);
        Task UpdateAsync(UserProfile profile);
        Task<string> GetUserRolesAsync(int userId);
        Task<string> GetUserRolesAsync(string email);
    }
}
