using Microsoft.AspNetCore.Identity;
using UserApi.DTO;

namespace UserApi.Service.UserService
{
    public interface IUserService
    {

        Task<IdentityResult> RegisterAdminAsync(RegisterRequestDto model);
        Task<IdentityResult> RegisterUserAsync(RegisterRequestDto model);
        Task<string?> AuthenticateUserAsync(LoginRequestDto model);
        Task<bool> EmailExistsAsync(string email);
        Task <bool>SendEmail(string eamil);
        Task <UserInfoResponseDto> GetUserInfoByIdAsync(string id); 

        Task<UserInfoResponseDto> UpdateUserInfoByIdAsync(string userId, UpdateUserInfoDto model);

        Task<IdentityResult> UpdatePasswordAsync(ResetPasswordRequestDto model);

    }
}
