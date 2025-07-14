using AutoMapper;
using Azure.Core;
using CommentsApi.DTO.Request;
using CommentsApi.Models;
using CommentsApi.Repository.InteractiveRepository;

namespace CommentsApi.Services.InteractiveServices
{
    public class InteractiveService:IInteractiveService
    {
        private readonly IInteractiveRepository _interactiveRepository;
        private readonly IMapper _mapper;
        public InteractiveService(IInteractiveRepository interactiveRepository, IMapper mapper)
        {

            _interactiveRepository = interactiveRepository;
            _mapper = mapper;
        }

        public async Task<int> ToggleBookLikeAsync(string userId, ToggleBookLikeCollectRequestDto request)
        {
            BookLike bookLike = _mapper.Map<BookLike>(request);

            bookLike.UserId = userId;

            var existing = await _interactiveRepository.GetLikeByUserAndBook(bookLike.UserId, bookLike.BookId);

            if (existing != null)
            {
                await _interactiveRepository.RemoveLikeAsync(existing);
            }
            else
            {
                await _interactiveRepository.AddLikeAsync(bookLike);
            }

            return await _interactiveRepository.CountLikeAsync(bookLike.BookId);

        }

        public async Task<int> ToggleBookCollectAsync(string userId, ToggleBookLikeCollectRequestDto request)
        {
            BookCollection bookCollects = _mapper.Map<BookCollection>(request);

            bookCollects.UserId = userId;

            var existing = await _interactiveRepository.GetCollectByUserAndBook(bookCollects.UserId, bookCollects.BookId);

            if (existing != null)
            {
                await _interactiveRepository.RemoveCollectAsync(existing);
            }
            else
            {
                await _interactiveRepository.AddCollectAsync(bookCollects);
            }

            return await _interactiveRepository.CountCollectAsync(bookCollects.BookId);
        }

        public async Task<int> ToggleBookCommentLikeAsync(string userId, ToggleBookCommentLikeRequestDto request)
        {

            CommentLike commentLike = _mapper.Map<CommentLike>(request);

            commentLike.UserId = userId;
             
            var existing = await _interactiveRepository.GetCommentLikeByUserAndComment(commentLike.UserId, commentLike.CommentId);

            if (existing != null)
            {
                await _interactiveRepository.RemoveCommentLikeAsync(existing);
            }
            else
            {
                await _interactiveRepository.AddCommentLikeAsync(commentLike);
            }

            return await _interactiveRepository.CountCommentlike(commentLike.CommentId);

        }
    }
}
