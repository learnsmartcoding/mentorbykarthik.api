using LSC.MentorByKarthik.Application.DTOs;

namespace LSC.MentorByKarthik.Application.Interfaces
{
    public interface IUserClaims
    {
        string GetUserEmail();
        Task<int> GetUserUserIdAsync();
        int GetUserIdFromClaims();
        string GetAdObjectId();
        List<string> GetUserRolesFromClaims();
        UserProfileDto GetUserInfo();
        bool IsUserAuthenticated();
        string GetUserRole();
    }
}
