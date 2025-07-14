using CommentsApi.DTO.Response;
using CommentsApi.Models;
using CommentsApi.Repository.ToolRepository;
using Microsoft.EntityFrameworkCore;

namespace CommentsApi.Repository.BookCommentRepository
{
    public class BookCommentRepository : IBookCommentRepository
    {
        private BookCommentsDbContext _context;
        public BookCommentRepository(BookCommentsDbContext context)
        {
            _context = context;
        }

        public async Task<BookInfo> GetBookInfoByPdfPathAsync(string path)
        {   
            var bookInfo=await _context.Books.Where(b=>b.PdfPath==path)
                                             .Include(b=>b.Likes)
                                             .Include(b=>b.Comments)
                                             .Include(b=>b.Collections)
                                             .AsNoTracking()
                                             .FirstOrDefaultAsync();

            return bookInfo;

        }


        public async Task <List<BookInfo>> GetAllBooksInfo()
        {
            return await _context.Books
                            .Include(b => b.Likes)
                            .Include(b => b.Comments)
                            .Include(b => b.Collections)
                            .AsNoTracking()
                            .ToListAsync();
        }

        public async Task<BookComments> AddBookCommentsAsync(BookComments bookComments)
        {
            await _context.Comments.AddAsync(bookComments);
            await _context.SaveChangesAsync();
            return bookComments;

        }

        public async Task<bool> AddBookInfoAsync(BookInfo bookInfo)
        {
            await _context.Books.AddAsync(bookInfo);
            int result = await _context.SaveChangesAsync();

            return result > 0;
        }
        public async Task<List<BookComments>> GetCommentsByBookId(int bookId)
        {
            var comments = await _context.Comments.Where(c => c.BookId == bookId)
                .Include(c=>c.CommentLikes)
                .ToListAsync();
            if (!comments.Any())
                return comments;
            return comments;
        }

        public async Task<BookComments> GetParentCommentAsync(int commentId)
        {
            var parentComment = await _context.Comments.FindAsync(commentId);

            return parentComment;
        }

        public async Task<bool> RemoveCommentAsync(BookComments comment)
        {
            _context.Comments.Remove(comment);
            var result= await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
