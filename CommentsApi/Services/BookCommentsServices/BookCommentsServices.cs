using System.Reflection.Metadata.Ecma335;
using AutoMapper;
using CommentsApi.DTO;
using CommentsApi.Exceptions.CustomExceptions;
using CommentsApi.Models;
using CommentsApi.Repository;
using SharedContract.HttpClient;

namespace CommentsApi.Services.BookCommentsServices
{
    public class BookCommentsServices : IBookCommentsService
    {

        private IBookCommentsRepository _bookCommentsRepository;
        private IMapper _mapper;
        public BookCommentsServices(IBookCommentsRepository bookCommentsRepository, IMapper mapper)
        {
            _bookCommentsRepository = bookCommentsRepository;
            _mapper = mapper;
        }

        public async Task<BookComments> AddBookComments(CreateCommentsRequestDto request)
        {

            var bookComments=_mapper.Map<BookComments>(request);    
            var newcomments=await _bookCommentsRepository.AddBookComments(bookComments);
            return newcomments;
        }

        public async Task<bool> AddBookInfo(BookMetaData request)
        {
            bool result_exist = await _bookCommentsRepository.CheckBookExist(request.PdfPath);
            if (result_exist)
            {
                throw new BookMetaDataSavedFailedException("Book already exist");
            }
            var bookInfo=_mapper.Map<BookInfo>(request);

           bool result=await _bookCommentsRepository.AddBookInfo(bookInfo);

            return result;
        }

  

        public async Task<List<CommentsDto>> GetCommentsByBookId(int bookId)
        {
         var comments=await _bookCommentsRepository.GetCommentsByBookId(bookId);

           
            if (comments == null) 
            {
                throw new Exception("no comments");
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

            var commentsDto =_mapper.Map<List<CommentsDto>>(tree);//why unit test failed in this step？？
            return commentsDto;
        } 
      
    }
}
