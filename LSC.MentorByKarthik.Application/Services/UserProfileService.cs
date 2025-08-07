using LSC.MentorByKarthik.Application.DTOs;
using LSC.MentorByKarthik.Application.Interfaces;
using LSC.MentorByKarthik.Domain.Entities;

namespace LSC.MentorByKarthik.Application.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserProfileRepository userProfileRepository;

        public UserProfileService(IUserProfileRepository userProfileRepository)
        {
            this.userProfileRepository = userProfileRepository;
        }

        public async Task UpdateUserProfilePicture(int userId, string pictureUrl)
        {
            await userProfileRepository.UpdateUserProfilePicture(userId, pictureUrl);
        }

        public Task<UserProfile?> GetUserInfoAsync(int userId)
        {
            return userProfileRepository.GetUserInfoAsync(userId);
        }

        public Task<UserProfile?> GetUserInfoAsync(string emailId)
        {
            return userProfileRepository.GetUserInfoAsync(emailId);
        }

        public async Task<int> CreateOrUpdateUserProfileAsync(UserProfileDto dto)
        {
            var user = await userProfileRepository.GetByEmailAsync(dto.Email, dto.AdObjId);
            if (user == null)
            {
                var newUser = new UserProfile
                {
                    DisplayName = dto.DisplayName,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    AdObjId = dto.AdObjId,
                    CreatedOn = DateTime.UtcNow,
                    ProfileImageUrl = dto.ProfileImageUrl
                };
                return await userProfileRepository.CreateAsync(newUser);
            }
            else
            {
                user.DisplayName = dto.DisplayName;
                user.FirstName = dto.FirstName;
                user.LastName = dto.LastName;
                //user.ProfileImageUrl = dto.ProfileImageUrl;

                await userProfileRepository.UpdateAsync(user);
                return user.UserId;
            }
        }

        public async Task UpdateUserPreferencesAsync(int userId, UpdateUserPreferencesDto dto)
        {
            var user = await userProfileRepository.GetByIdAsync(userId)
                       ?? throw new KeyNotFoundException("User not found");

            user.MyTimeZone = dto.MyTimeZone;
            user.PreferedLanguage = dto.PreferedLanguage;

            await userProfileRepository.UpdateAsync(user);
        }

        public async Task<UserProfileDto?> GetUserProfileByEmailAsync(string email, string adObjId)
        {
            var user = await userProfileRepository.GetByEmailAsync(email, adObjId);
            if (user == null) return null;

            return new UserProfileDto
            {
                DisplayName = user.DisplayName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                MyTimeZone = user.MyTimeZone,
                Email = user.Email,
                AdObjId = user.AdObjId,
                PreferedLanguage = user.PreferedLanguage,
                ProfileImageUrl = user.ProfileImageUrl
            };
        }

        public async Task<string> GetUserRolesAsync(int userId)
        {
            return await userProfileRepository.GetUserRolesAsync(userId);
        }

        public async Task<string> GetUserRolesAsync(string email)
        {
            return await userProfileRepository.GetUserRolesAsync(email);
        }

    }
}
