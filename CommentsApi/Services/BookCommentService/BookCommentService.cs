using Azure.Core;
using Azure.Messaging.ServiceBus;
using System.Text.Json;
using CommentsApi.DTO;
using CommentsApi.DTO.Response;
using CommentsApi.Models;
using CommentsApi.Repository.BookCommentRepository;
using SharedContract.Event;
using SharedContract.HttpClient;
using AutoMapper;
using CommentsApi.Exceptions.CustomExceptions;
using CommentsApi.Repository.ToolRepository;
using CommentsApi.DTO.Request;

namespace CommentsApi.Services.BookCommentService
{
    public class BookCommentService : IBookCommentService
    {
        private readonly IBookCommentRepository _bookCommentRepository;
        private readonly IToolRepository _toolRepository;
        private readonly IMapper _mapper;
        private readonly ServiceBusSender _serviceBusSender;

        public BookCommentService(IBookCommentRepository bookCommentRepository, IMapper mapper,IToolRepository toolRepository ,ServiceBusSender serviceBusSender)
        {
            _bookCommentRepository = bookCommentRepository;
            _mapper = mapper;
            _toolRepository = toolRepository;   
            _serviceBusSender = serviceBusSender;
        }

        public async Task<GetCommentsResponseDto> AddBookComments(CreateCommentsRequestDto request, string userName, string userId)
        {
            var bookComment = _mapper.Map<BookComments>(request);
            bookComment.CreatedBy = userName;
            bookComment.UserId = userId;
            var addedResult = await _bookCommentRepository.AddBookCommentsAsync(bookComment);

            if (bookComment.ParentCommentId.HasValue)
            {
                BookComments parentComment = await _bookCommentRepository.GetParentCommentAsync(bookComment.ParentCommentId.Value);

                var replyInfo = new CreateNewReplyEvent
                {
                    BookId = bookComment.BookId,
                    Content = bookComment.Content,
                    ParentCommentContent = parentComment.Content,
                    CreatedAt = bookComment.CreatedAt,
                    SendUserId = parentComment.UserId,
                    ReplyUserId = bookComment.UserId,
                    ReplyUserName = bookComment.CreatedBy
                };
                var jsonBody = JsonSerializer.Serialize(replyInfo);
                var message = new ServiceBusMessage(jsonBody)
                {
                    ContentType = "application/json",
                    Subject = "Send notification to reply receiver" // Optional metadata
                };
                await _serviceBusSender.SendMessageAsync(message);


            }

            GetCommentsResponseDto addedComment = _mapper.Map<GetCommentsResponseDto>(addedResult);


            return addedComment;
        }

        public async Task<bool> AddBookInfo(BookMetaData request)
        {
            bool result_exist = await _toolRepository.CheckBookExist(request.PdfPath);
            if (result_exist)
            {
                throw new BookMetaDataSavedFailedException("Book already exist");
            }
            var bookInfo = _mapper.Map<BookInfo>(request);
            bool result = await _bookCommentRepository.AddBookInfoAsync(bookInfo);
            return result;
        }

        public async Task<bool> DeleteCommentByIdAsync(int commentId)
        {
            var comment = await _toolRepository.GetCommentById(commentId);

            var result= await _bookCommentRepository.RemoveCommentAsync(comment);

            return result;
        }

        public async Task<List<GetAllBooksInfoResponseDto>> GetAllBooksInfo()
        { 
            
            var allBooksInfo=await _bookCommentRepository.GetAllBooksInfo();
            return allBooksInfo.Select(b => new GetAllBooksInfoResponseDto
            {
                Id = b.Id,
                Title = b.Title,
                Year = b.Year,
                Author = b.Author,
                PdfPath = b.PdfPath,
                ImgPath = b.ImgPath,
                LikeCount = b.Likes.Count,
                CollectCount = b.Collections.Count,
                CommentCount = b.Comments.Count,

            }).ToList();
            
        }

        public async Task<GetBookInfoResponseDto> GetBookInfoByPdfPath(string pdfPath, string? userId)
        {
            var bookInfo=await _bookCommentRepository.GetBookInfoByPdfPathAsync(pdfPath);

            return new GetBookInfoResponseDto
            {
                Id = bookInfo.Id,
                Title = bookInfo.Title,
                Year = bookInfo.Year,
                Author = bookInfo.Author,
                CurrentUserLike = userId == null ? false : bookInfo.Likes.Any(l => l.UserId == userId),
                CurrentUserCollect = userId == null ? false : bookInfo.Collections.Any(l => l.UserId == userId),
                LikeCount = bookInfo.Likes.Count,
                CollectCount = bookInfo.Collections.Count,
                CommentCount = bookInfo.Comments.Count,
            };
        }

        public async Task<List<GetCommentsResponseDto>> GetCommentsByBookId(int bookId)
        {
            bool bookExists = await _toolRepository.CheckBookExistById(bookId);

            if (!bookExists)
            {
                throw new BookNotExistException("Book not exist"); // 204: Book exists, but no comments
            }
            var comments = await _bookCommentRepository.GetCommentsByBookId(bookId);



            if (comments == null || comments.Count == 0)
            {
                return new List<GetCommentsResponseDto>(); // or return null if controller will handle
            }

            var lookup = comments.ToLookup(c => c.ParentCommentId);

            List<BookComments> BuildTree(int? parentId)
            {
                return lookup[parentId]
                    .Select(c =>
                    {
                        c.Replies = BuildTree(c.CommentsId);
                        return c;

                    })
                    .ToList();
            }
            var tree = BuildTree(null);

            var commentsDto = _mapper.Map<List<GetCommentsResponseDto>>(tree);//why unit test failed in this step？？
            return commentsDto;
        }
    }
}
