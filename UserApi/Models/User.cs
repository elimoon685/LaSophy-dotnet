using Microsoft.AspNetCore.Identity;

namespace UserApi.Models
{
    public class User:IdentityUser

    {
        public DateTime CreatedAt = DateTime.UtcNow;
    }
}
