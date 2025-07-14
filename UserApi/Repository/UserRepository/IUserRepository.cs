using Microsoft.AspNetCore.Identity;
using UserApi.Models;

namespace UserApi.Repository.UserRepository
{
    public interface IUserRepository
    {
        Task<bool> EmailExistsAsync(string email);
        Task<IdentityResult> CreateUserAsync(User user,  string password);

        Task<bool> CheckPasswordAsync(User user, string newPassword);
        Task<User?> GetUserByEmailAsync(string eamil);
        Task<string> GetUserRolesAsync(User user);
        Task AddUserToRoleAsync(User user, string role);
        Task <User?>FindByEmailAsync(string email);
        Task<string> GenerateToken(User user);

        Task<User> GetUserInfoByIdAsync(string id);   
        Task<IdentityResult> ResetPasswordAsync(User user, string token, string newPassword);

        Task UpdateAsync(User user);
    }
}
