using AutoMapper;
using Azure.Messaging.ServiceBus;
using CommentsApi;
using CommentsApi.DTO.Request;
using CommentsApi.DTO.Response;
using CommentsApi.DTO;
using CommentsApi.Models;
using CommentsApi.Repository.BookCommentRepository;
using CommentsApi.Repository.ToolRepository;
using CommentsApi.Services.BookCommentService;
using Microsoft.EntityFrameworkCore;
using Moq;
using SharedContract.HttpClient;

namespace BookCommentApi.Tests
{
    public class BookCommentServiceTests
    {

        private readonly IBookCommentService _bookCommentService;
        private readonly IBookCommentRepository _bookCommentRepository;
        private readonly IToolRepository _toolRepository;
        private readonly IMapper _mapper;
        private readonly Mock<ServiceBusSender> _serviceBusSender;
        private readonly BookCommentsDbContext _bookCommentsDbContext;

        public BookCommentServiceTests()
        {
            var options = new DbContextOptionsBuilder<BookCommentsDbContext>().UseInMemoryDatabase("TestDb_" + Guid.NewGuid().ToString()).Options;
            //return builder
            _bookCommentsDbContext = new BookCommentsDbContext(options);

            _bookCommentRepository = new BookCommentRepository(_bookCommentsDbContext);
            _serviceBusSender = new Mock<ServiceBusSender>();
            _toolRepository = new ToolRepository(_bookCommentsDbContext);

            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BookMetaData, BookInfo>();
                cfg.CreateMap<CreateCommentsRequestDto, BookComments>();//reques to entity
                cfg.CreateMap<BookComments, GetCommentsResponseDto>();//entity to request
                cfg.CreateMap<BookInfo, BookInfoDto>();


            });
            _mapper= mappingConfig.CreateMapper();
            _bookCommentService =new BookCommentService(_bookCommentRepository,_mapper, _toolRepository, _serviceBusSender.Object);
        }
            
            [Fact]
        public async Task AddBookcomment_WhenPropsAreVaild_ShouldRetunrnNewComment()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString();
            var userName = "Eli";
            var bookComment = new CreateCommentsRequestDto
            {
                BookId = 2,
                Content="I like this book",
                ParentCommentId=null
            };
            //Act
            var result = await _bookCommentService.AddBookComments(bookComment, userId, userName);

            //Assert
            Assert.NotNull(result);

        }
        [Fact]
        public async Task AddBookInfo_WhenPropsIsValid_ShouldReturnBoolen()
        {
            //Arrange
            var bookMetaDate = new BookMetaData
            {
                Title = "Tungking",
                Author = "Mesny William",
                Year = "1883",
                PdfPath = "tungking00mesnrich.pdf",
                ImgPath = "tungking00mesnrich.png"
            };
            // Act
            var addResult = await _bookCommentService.AddBookInfo(bookMetaDate);
            //Assert

            Assert.True(addResult);

        }
    }
}