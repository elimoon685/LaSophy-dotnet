using System.Net.WebSockets;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserApi.DTO;
using UserApi.Service.UserService;

namespace UserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]

        public async Task<IActionResult> Register([FromBody] RegisterRequestDto model )
        {

            if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Email) ||
                string.IsNullOrWhiteSpace(model.Password) || string.IsNullOrWhiteSpace(model.ConfirmPassword))
            {
                return StatusCode(400, ApiResponse<string>.Fail("Please fill out form properly", "400"));
            }

            var result=await _userService.RegisterUserAsync(model);
            if (result.Succeeded)
            {
                return StatusCode(200, ApiResponse<IdentityResult>.Success(result, "Register Successfully"));
            }
            return StatusCode(500, ApiResponse<string>.Fail("Unknown errow", "500"));
        }

        

        [HttpGet("get-user/{userId}")]
        public async Task <ActionResult> GetUserInfoById(string userId) 
        {

            var userInfo = await _userService.GetUserInfoByIdAsync(userId);

            return StatusCode(200, ApiResponse<UserInfoResponseDto>.Success(userInfo, "get user info successfully"));
            
            
        }
       
        [HttpPatch("userInfo-update")]
        public async Task <ActionResult> UpdateUserInfoById(UpdateUserInfoDto userInfo)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(400, ApiResponse<string>.Fail("User not authenticated properly", "400"));
            }
            var updatedResult = await _userService.UpdateUserInfoByIdAsync(userId, userInfo);

            return StatusCode(200, ApiResponse<UserInfoResponseDto>.Success(updatedResult, "Update user info successfully"));


        }














































    }
}
