using CommentsApi.DTO.Response;
using CommentsApi.Models;

namespace CommentsApi.Repository.BookCommentRepository
{
    public interface IBookCommentRepository
    {
        Task<BookInfo> GetBookInfoByPdfPathAsync(string path);

        Task <List<BookInfo>> GetAllBooksInfo();

        Task<BookComments> AddBookCommentsAsync(BookComments bookComments);

        Task<bool> AddBookInfoAsync(BookInfo bookInfo);

        Task<List<BookComments>> GetCommentsByBookId(int bookId);

        Task<BookComments> GetParentCommentAsync(int commentId);

        Task<bool> RemoveCommentAsync(BookComments comment);

        Task <List<BookComments>> GetPotentiallyDeletedCommentTreeAsync(int commentId);

        Task<bool> DeleteComments(List<BookComments> comments);


    }
}
