using CommentsApi.Models;

namespace CommentsApi.Repository.InteractiveRepository
{
    public interface IInteractiveRepository
    {

        Task AddLikeAsync(BookLike bookLike);

        Task RemoveLikeAsync(BookLike booklike);

        Task AddCollectAsync (BookCollection bookCollect);

        Task RemoveCollectAsync (BookCollection bookCollect);

        Task AddCommentLikeAsync(CommentLike commentLike);

        Task RemoveCommentLikeAsync(CommentLike commentLike);


        Task<BookCollection?> GetCollectByUserAndBook(string userId, int bookId);

        Task<BookLike?> GetLikeByUserAndBook(string userId, int bookId);

        Task<CommentLike?> GetCommentLikeByUserAndComment(string userId, int commentId);

        Task<int> CountCollectAsync(int bookId);

        Task<int> CountLikeAsync(int bookId);

        Task <int> CountCommentlike(int commentId);

    }
}
