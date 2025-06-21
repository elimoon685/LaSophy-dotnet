using CommentsApi.DTO;
using CommentsApi.Models;

namespace CommentsApi.Repository
{
    public interface IBookCommentsRepository
    {

        Task <List<BookComments>> GetCommentsByBookId (int bookId);

        Task <BookComments> AddBookComments(BookComments bookComments);

        Task <bool> AddBookInfo (BookInfo bookInfo);

        Task<bool> CheckBookExist(string PdfPath);
    }
}
