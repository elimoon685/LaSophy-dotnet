using CommentsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CommentsApi.Repository.InteractiveRepository
{
    public class InteractiveReposotory : IInteractiveRepository
    {
        private BookCommentsDbContext _context;
        public InteractiveReposotory(BookCommentsDbContext context)
        {
            _context = context;
        }
        public async Task AddCollectAsync(BookCollection bookCollect)
        {
            try
            {
                await _context.BookCollects.AddAsync(bookCollect);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) {
                throw new Exception();
            }
        }

        public async Task AddLikeAsync(BookLike bookLike)
        {
            try
            {
                await _context.BookLike.AddAsync(bookLike);
                await _context.SaveChangesAsync();
            }catch(Exception err)
            {
                throw new Exception();
            }
        }

        public async Task<int> CountCollectAsync(int bookId)
        {
            return await _context.BookCollects.CountAsync(l => l.BookId == bookId);
        }

        public async Task<int> CountLikeAsync(int bookId)
        {
            return await _context.BookLike.CountAsync(l => l.BookId == bookId);
        }

        public async Task<BookLike?> GetLikeByUserAndBook(string userId, int bookId)
        {
            return await _context.BookLike.FirstOrDefaultAsync(l => l.UserId == userId && l.BookId == bookId);
        }

        public async Task<BookCollection?> GetCollectByUserAndBook(string userId, int bookId)
        {
            return await _context.BookCollects.FirstOrDefaultAsync(l => l.UserId == userId && l.BookId == bookId);
        }

        public async Task RemoveCollectAsync(BookCollection bookCollect)
        {
            _context.BookCollects.Remove(bookCollect);

            await _context.SaveChangesAsync();
        }

        public async Task RemoveLikeAsync(BookLike bookLike)
        {
            _context.BookLike.Remove(bookLike);

            await _context.SaveChangesAsync();
        }

        public async Task AddCommentLikeAsync(CommentLike commentLike)
        {
            await _context.CommentLike.AddAsync(commentLike);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveCommentLikeAsync(CommentLike commentLike)
        {
            _context.CommentLike.Remove(commentLike);
            await _context.SaveChangesAsync();
        }

        public async Task<CommentLike?> GetCommentLikeByUserAndComment(string userId, int commentId)
        {
            return await _context.CommentLike.FirstOrDefaultAsync(c => c.CommentId == commentId && c.UserId == userId);
        }

        public async Task<int> CountCommentlike(int commentId)
        {
            return await _context.CommentLike.CountAsync(c=>c.CommentId == commentId);
        }
    }
}
