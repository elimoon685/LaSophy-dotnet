using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserApi.Models;

namespace UserApi.Repository.UserRepository
{
    public class UserRepository : IUserRepository
    { 

        private readonly UserManager<User> _userManager;
        private readonly LaSophyDbContext _laSophyDbContext;

        public UserRepository(UserManager<User> userManager, LaSophyDbContext laSophyDbContext )
        {
            _userManager = userManager;
            _laSophyDbContext = laSophyDbContext;
        }

        public async Task AddUserToRoleAsync(User user, string role)
        {
            await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
             return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IdentityResult> CreateUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _laSophyDbContext.User.AnyAsync(u => u.Email==email);
        }

        public async Task<User?> FindByEmailAsync(string email)
        {
           return await _userManager.FindByEmailAsync(email);
        }

        public async Task<string> GenerateToken(User user)
        {
            var token=await _userManager.GeneratePasswordResetTokenAsync(user);
            return token;
        }

        public async Task<User?> GetUserByEmailAsync(string eamil)
        {
            return await _laSophyDbContext.User.FirstOrDefaultAsync(u => u.Email==eamil);
        }

        public async Task<string> GetUserRolesAsync(User user)
        {
            var roles= await _userManager.GetRolesAsync(user);
            return roles.FirstOrDefault();
        }
    }
}
