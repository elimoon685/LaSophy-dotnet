using CommentsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CommentsApi
{
    public class BookCommentsDbContext:DbContext
    {
        public BookCommentsDbContext(DbContextOptions<BookCommentsDbContext> options):base(options) { }

        public DbSet<BookInfo> Books { get; set; }
        public DbSet<BookComments> Comments { get; set; }

        public DbSet<BookCollection> BookCollects { get; set; }
        public DbSet<BookLike> BookLike { get; set; }
        public DbSet<CommentLike> CommentLike { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<BookComments>()
           .HasOne(c => c.ParentComment)
           .WithMany(c => c.Replies)
           .HasForeignKey(c => c.ParentCommentId)
           .OnDelete(DeleteBehavior.Restrict);
            //why i change setnull to restrict
            //why i change setnull to restrict
            //why i change setnull to restrict

            modelBuilder.Entity<BookLike>()
           .HasOne(bl => bl.Book)
           .WithMany(b=>b.Likes) // or WithMany(b => b.BookLikes) if reverse nav
           .HasForeignKey(bl => bl.BookId)
           .OnDelete(DeleteBehavior.Cascade); // 

            modelBuilder.Entity<BookCollection>()
           .HasOne(bl => bl.Book)
           .WithMany(b=>b.Collections) // or WithMany(b => b.BookLikes) if reverse nav
           .HasForeignKey(bl => bl.BookId)
           .OnDelete(DeleteBehavior.Cascade); // o

            modelBuilder.Entity<CommentLike>()
            .HasOne(bl => bl.Comment)
            .WithMany(b=>b.CommentLikes)
            .HasForeignKey(b=>b.CommentId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }

        
    

}

