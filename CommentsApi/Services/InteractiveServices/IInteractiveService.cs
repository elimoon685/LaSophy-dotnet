using CommentsApi.DTO;
using CommentsApi.DTO.Request;

namespace CommentsApi.Services.InteractiveServices
{
    public interface IInteractiveService
    {
        Task<int> ToggleBookLikeAsync(string userId, ToggleBookLikeCollectRequestDto request);

        Task<int> ToggleBookCollectAsync(string userId, ToggleBookLikeCollectRequestDto request);

        Task <int> ToggleBookCommentLikeAsync(string userId, ToggleBookCommentLikeRequestDto request);
    }
}
