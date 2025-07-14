using CommentsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CommentsApi.Repository.ToolRepository
{
    public class ToolRepository:IToolRepository
    {
        private readonly BookCommentsDbContext _context;
        public ToolRepository(BookCommentsDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckBookExist(string PdfPath)
        {
            return await _context.Books.AnyAsync(p => p.PdfPath == PdfPath);
        }

        public async Task<bool> CheckBookExistById(int bookId)
        {
            return await _context.Books.AnyAsync(b => b.Id == bookId);
        }

        public async Task<BookComments> GetCommentById(int commentId)
        {
            return await _context.Comments.FindAsync(commentId);
        }
    }
}
