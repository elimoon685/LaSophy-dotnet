using CommentsApi.Models;

namespace CommentsApi.Repository.ToolRepository
{
    public interface IToolRepository
    {
        Task<bool> CheckBookExist(string PdfPath);
        Task<bool> CheckBookExistById(int bookId);

        Task<BookComments> GetCommentById(int commentId);
    }
}
