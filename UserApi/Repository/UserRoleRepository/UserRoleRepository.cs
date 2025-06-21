
using Microsoft.AspNetCore.Identity;
using UserApi.Models;

namespace UserApi.Repository.UserRoleRepository
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly IUserRoleRepository _userRoleRepository;

        private readonly RoleManager<Role> _roleManager;

        public UserRoleRepository(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }

        public async Task CreateRoleAsync(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new Role { Name = roleName });
            }
        }
    }
    
}
