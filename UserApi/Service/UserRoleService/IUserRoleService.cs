namespace UserApi.Service.UserRoleService
{
    public interface IUserRoleService
    {

        Task CreateRoleAsync(string roleName);
    }
}
