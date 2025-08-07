using LSC.MentorByKarthik.Application.DTOs;
using LSC.MentorByKarthik.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Security.Principal;

namespace LSC.MentorByKarthik.Application.Services
{
    public class UserClaims : IUserClaims
    {
        private readonly IUserProfileService userProfileService;

        public UserClaims(IHttpContextAccessor httpContextAccessor, IUserProfileService userProfileService)
        {
            HttpContextAccessor = httpContextAccessor;
            this.userProfileService = userProfileService;
        }

        public IHttpContextAccessor HttpContextAccessor { get; }

        public bool IsUserAuthenticated()
        {
            var identity = HttpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            return (identity != null && identity.IsAuthenticated);
        }
        private string GetClaimInfo(string property)
        {
            var propertyData = "";
            var identity = HttpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null && identity.IsAuthenticated)
            {
                IEnumerable<Claim> claims = identity.Claims;
                // or
                propertyData = identity.Claims.FirstOrDefault(d => d.Type.Contains(property))?.Value;

            }
            return propertyData;
        }


        public string GetAdObjectId()
        {
            return GetClaimInfo("objectidentifier");
        }
        public List<string> GetUserRolesFromClaims()
        {
            var roles = GetClaimInfo("extension_userRoles"); ;
            return string.IsNullOrEmpty(roles) ? new List<string>() : roles.Split(',').ToList();
        }

        public int GetUserIdFromClaims()
        {
            var userId = GetClaimInfo("extension_userId");
            return Convert.ToInt32(userId);
        }

        public string GetUserEmail()
        {
            var email = GetClaimInfo("emailaddress");
            return email;
        }

        public async Task<int> GetUserUserIdAsync()
        {
            var email = GetClaimInfo("emailaddress");
            var emailFromClaims = string.IsNullOrEmpty(email) ? string.Empty : email;

            var userInfo = await userProfileService.GetUserInfoAsync(emailFromClaims);
            return userInfo.UserId;
        }

        public string GetUserRole()
        {
            var email = GetClaimInfo("emailaddress");
            var role = email.Equals("learnsmartcoding@gmail.com")? "Admin" : "User";
            return role;
        }
        public UserProfileDto GetUserInfo()
        {
            return new UserProfileDto()
            {
                DisplayName = GetClaimInfo("name"),
                FirstName = GetClaimInfo("givenname"),
                LastName = GetClaimInfo("surname"),
                AdObjId = GetClaimInfo("objectidentifier"),
                Email = GetClaimInfo("emailaddress")
            };
        }
    }
}
