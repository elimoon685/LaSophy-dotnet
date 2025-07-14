using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using UserApi.DTO;
using UserApi.Models;
using UserApi.Service.UserService;
using UserApi.Utils;

namespace UserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        //private UserManager<User> _userManager;
        //private IConfiguration _configuration;
        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
                
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginRequestDto>> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            string? token=await _userService.AuthenticateUserAsync(loginRequestDto);

            return Ok(ApiResponse<string?>.Success(token,"create the token successfully"));
          
           
            
        }

        [HttpPost("forget-password")]
        public async Task<ActionResult> SendEmail(string email)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<string>.Fail("Email field can not be null", "400"));
            }

            var result= await _userService.SendEmail(email);

            return StatusCode(200, ApiResponse<bool>.Success(true, "Send varify eamil"));
        }

        [HttpPost("reset-password")]

        public async Task<ActionResult> UpdatePassword([FromBody] ResetPasswordRequestDto request)
        {
            if ( string.IsNullOrWhiteSpace(request.Email) ||
               string.IsNullOrWhiteSpace(request.NewPassword) || string.IsNullOrWhiteSpace(request.ConfirmNewPassword))
            {
                return StatusCode(400, ApiResponse<string>.Fail("Please fill out form properly", "400"));
            }

            var result = await _userService.UpdatePasswordAsync(request);
            if (result.Succeeded)
            {
                return StatusCode(200, ApiResponse<bool>.Success(true, "Update password Successful"));
            }
            return StatusCode(500, ApiResponse<string>.Fail("Failed to update the password", "500"));
        }


       
    }
}
