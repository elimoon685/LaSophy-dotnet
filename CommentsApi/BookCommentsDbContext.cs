using CommentsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CommentsApi
{
    public class BookCommentsDbContext:DbContext
    {
        public BookCommentsDbContext(DbContextOptions<BookCommentsDbContext> options):base(options) { }

        public DbSet<BookInfo> Books { get; set; }
        public DbSet<BookComments> Comments { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<BookComments>()
           .HasOne(c => c.ParentComment)
           .WithMany(c => c.Replies)
           .HasForeignKey(c => c.ParentCommentId)
           .OnDelete(DeleteBehavior.Restrict);
            //why i change setnull to restrict
        }
    }
}
