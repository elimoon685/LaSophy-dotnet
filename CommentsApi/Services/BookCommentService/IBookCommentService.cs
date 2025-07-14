using CommentsApi.DTO;
using CommentsApi.DTO.Request;
using CommentsApi.DTO.Response;
using SharedContract.HttpClient;

namespace CommentsApi.Services.BookCommentService
{
    public interface IBookCommentService
    {

        Task <GetBookInfoResponseDto> GetBookInfoByPdfPath(string pdfPath, string? userId);

        Task<List<GetAllBooksInfoResponseDto>> GetAllBooksInfo();

        Task<List<GetCommentsResponseDto>> GetCommentsByBookId(int bookId);

        Task<GetCommentsResponseDto> AddBookComments(CreateCommentsRequestDto bookComments, string userName, string userId);

        Task<bool> AddBookInfo(BookMetaData bookInfo);

        Task <bool> DeleteCommentByIdAsync(int commentId);
    }
}
