using LSC.MentorByKarthik.Application.DTOs;
using LSC.MentorByKarthik.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LSC.MentorByKarthik.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserProfileService _service;
        private readonly IUserClaims _userClaims;

        public UserController(IUserProfileService service, IUserClaims userClaims)
        {
            _service = service;
            _userClaims = userClaims;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrUpdateUserProfile()
        {
            var userModel = _userClaims.GetUserInfo();
            var userId = await _service.CreateOrUpdateUserProfileAsync(userModel);
            return Ok(userId);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserPreferences([FromBody] UpdateUserPreferencesDto dto)
        {
            var userId = await _userClaims.GetUserUserIdAsync();
            await _service.UpdateUserPreferencesAsync(userId, dto);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetUserProfile()
        {
            var isUserAuthenticated = _userClaims.IsUserAuthenticated();
            if (!isUserAuthenticated)
            {
                return Ok(new UserProfileDto());
            }
            var userModel = _userClaims.GetUserInfo();
            var profile = await _service.GetUserProfileByEmailAsync(userModel.Email, userModel.AdObjId);
            if (profile != null)
            {
                profile.UserRole = _userClaims.GetUserRole();
            }
            return profile == null ? NotFound() : Ok(profile);
        }
    }

}
