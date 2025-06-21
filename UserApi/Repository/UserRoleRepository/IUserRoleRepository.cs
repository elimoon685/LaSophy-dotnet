namespace UserApi.Repository.UserRoleRepository
{
    public interface IUserRoleRepository
    {
        Task CreateRoleAsync(string roleName);
    }
}
