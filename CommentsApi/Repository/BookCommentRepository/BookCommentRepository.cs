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
            try
            {
                _context.Comments.Remove(comment);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex) {

                throw new Exception();
            }
        }

        public async Task<List<BookComments>> GetPotentiallyDeletedCommentTreeAsync(int commentId)
        {
            var rootComment=await _context.Comments.FirstOrDefaultAsync(c=>c.CommentsId == commentId);

            var allCommentsByBookId=await _context.Comments.Where(c=>c.BookId==rootComment.BookId).ToListAsync();

            var result = new List<BookComments>();

            void CollectChildren(int parentId)
            {
                var children=allCommentsByBookId.Where(c=>c.ParentCommentId==parentId).ToList();

                foreach (var child in children)
                {
                    result.Add(child);
                    CollectChildren(child.CommentsId);
                }
            }

            if (rootComment != null)
            {
                result.Add(rootComment);
                CollectChildren(commentId);
            }

            return result;
        }

        public async Task<bool> DeleteComments(List<BookComments> comments)
        {
            _context.Comments.RemoveRange(comments);
           var result= await _context.SaveChangesAsync();

            return result > 0;
        }
        
    }
}
