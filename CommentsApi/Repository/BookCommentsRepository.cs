using CommentsApi.DTO;
using CommentsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CommentsApi.Repository
{
    public class BookCommentsRepository : IBookCommentsRepository
    {

        private BookCommentsDbContext _context;
        public BookCommentsRepository(BookCommentsDbContext context)
        {
            _context = context;
        }

        public async Task<BookComments> AddBookComments(BookComments bookComments)
        {
            _context.Comments.Add(bookComments);
            _context.SaveChanges();
            return bookComments;
            
        }

        public async Task<bool> AddBookInfo(BookInfo bookInfo)
        {
            await _context.Books.AddAsync(bookInfo);
           int result= await _context.SaveChangesAsync();

            return result>0;
        }

        public async Task<bool> CheckBookExist(string PdfPath)
        {
           return await _context.Books.AnyAsync(p=>p.PdfPath == PdfPath);
        }

        public async Task<List<BookComments>> GetCommentsByBookId(int bookId)
        {
           var comments=await _context.Comments.Where(c=>c.BookId==bookId).ToListAsync();
            
            return comments;
        }
    }
}
