using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserApi.Models;

namespace UserApi
{
    public class LaSophyDbContext: IdentityDbContext<User,Role,string>
    {

        public LaSophyDbContext(DbContextOptions<LaSophyDbContext> options) : base(options) { }

        public DbSet<User> User { get; set; }

        public DbSet<UserEvent> Event { get; set; }

    }
}
