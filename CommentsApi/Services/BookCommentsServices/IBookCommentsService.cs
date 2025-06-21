using CommentsApi.DTO;
using CommentsApi.Models;
using SharedContract.HttpClient;

namespace CommentsApi.Services.BookCommentsServices
{
    public interface IBookCommentsService
    {
        Task<List<CommentsDto>> GetCommentsByBookId(int bookId);

        Task<BookComments> AddBookComments( CreateCommentsRequestDto bookComments);

        Task<bool> AddBookInfo(BookMetaData bookInfo);

    }

}

