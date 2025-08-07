using LSC.MentorByKarthik.Application.Interfaces;
using LSC.MentorByKarthik.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LSC.MentorByKarthik.Infrastructure
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly MentorByKarthikContext _context;

        public UserProfileRepository(MentorByKarthikContext context)
        {
            _context = context;
        }

        public async Task UpdateUserProfilePicture(int userId, string pictureUrl)
        {
            var user = await _context.UserProfiles.FindAsync(userId);
            if (user != null)
            {
                user.ProfileImageUrl = pictureUrl;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<UserProfile?> GetUserInfoAsync(int userId)
        {
            var user = await _context.UserProfiles.FirstOrDefaultAsync(f => f.UserId == userId);
            return user;
        }

        public async Task<UserProfile?> GetUserInfoAsync(string emailId)
        {
            var user = await _context.UserProfiles.FirstOrDefaultAsync(f => f.Email == emailId);
            return user;
        }

        public async Task<UserProfile?> GetByEmailAsync(string email, string adObjId)
        {
            return await _context.UserProfiles.FirstOrDefaultAsync(u => u.Email == email );
        }

        public async Task<UserProfile?> GetByIdAsync(int id)
        {
            return await _context.UserProfiles.FindAsync(id);
        }

        public async Task<string> GetUserRolesAsync(int userId)
        {
            var roles = await _context.UserRoles
                .Where(u => u.UserId == userId)
                .Select(u => u.Role.RoleName)
                .ToListAsync();

            return string.Join(",", roles);
        }

        public async Task<string> GetUserRolesAsync(string email)
        {
            var roles = await _context.UserRoles
                .Where(ur => ur.User.Email == email)
                .Select(ur => ur.Role.RoleName)
                .ToListAsync();

            return string.Join(",", roles);
        }



        public async Task<int> CreateAsync(UserProfile profile)
        {
            _context.UserProfiles.Add(profile);
            await _context.SaveChangesAsync();
            return profile.UserId;
        }

        public async Task UpdateAsync(UserProfile profile)
        {
            _context.UserProfiles.Update(profile);
            await _context.SaveChangesAsync();
        }
    }
}
