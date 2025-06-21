
using UserApi.Repository.UserRoleRepository;

namespace UserApi.Service.UserRoleService
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;

        public UserRoleService(IUserRoleRepository userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
        }

        public async Task CreateRoleAsync(string roleName)
        {
            await _userRoleRepository.CreateRoleAsync(roleName);
        }
    }
}
