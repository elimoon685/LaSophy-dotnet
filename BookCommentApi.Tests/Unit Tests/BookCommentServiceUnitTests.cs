using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Azure.Messaging.ServiceBus;
using CommentsApi.Repository.BookCommentRepository;
using CommentsApi.Repository.ToolRepository;
using CommentsApi.Services.BookCommentService;
using CommentsApi;
using Moq;
using SharedContract.HttpClient;
using Azure.Core;
using CommentsApi.Exceptions.CustomExceptions;
using CommentsApi.Models;

namespace BookCommentApi.Tests.Unit_Tests
{
    public class BookCommentServiceUnitTests
    {

        private readonly Mock<IBookCommentRepository> _bookCommentRepository=new();
        private readonly Mock<IToolRepository> _toolRepository=new();
        private readonly Mock<IMapper> _mapper=new();
        private readonly Mock<ServiceBusSender> _serviceBusSender=new();

        private readonly BookCommentService _bookCommentService;

        public BookCommentServiceUnitTests()
        {
            _bookCommentService = new BookCommentService(_bookCommentRepository.Object, _mapper.Object, _toolRepository.Object, _serviceBusSender.Object);


        }
        [Fact]
        public async Task AddBookInfo_WhenBookExists_ShouldThrowException()
        {
            var bookMetaDate = new BookMetaData
            {
                Title = "Tungking",
                Author = "Mesny William",
                Year = "1883",
                PdfPath = "tungking00mesnrich.pdf",
                ImgPath = "tungking00mesnrich.png"
            };
            _toolRepository.Setup(r => r.CheckBookExist(bookMetaDate.PdfPath))
                           .ReturnsAsync(true);

            var ex = await Assert.ThrowsAsync<BookMetaDataSavedFailedException>(() =>
       _bookCommentService.AddBookInfo(bookMetaDate));

            Assert.Equal("Book already exist", ex.Message);
        }

        [Fact] 
        public async Task AddBookInfo_WhenBookNotExists_ShouldReturnAddResult()


        {
            var bookMetaDate = new BookMetaData
            {
                Title = "Tungking",
                Author = "Mesny William",
                Year = "1883",
                PdfPath = "tungking00mesnrich.pdf",
                ImgPath = "tungking00mesnrich.png"
            };
            var mappedBook = new BookInfo
            {
                Title = "Test Book",
                Author = "Tester",
                Year = "2000",
                PdfPath = "dummy.pdf",
                ImgPath = "dummy.png",
                Likes = [],
                Collections = [],
                Comments = []
            };

            _toolRepository.Setup(r => r.CheckBookExist(bookMetaDate.PdfPath))
                           .ReturnsAsync(false);

            _mapper.Setup(m => m.Map<BookInfo>(bookMetaDate))
                   .Returns(mappedBook);

            _bookCommentRepository.Setup(r => r.AddBookInfoAsync(mappedBook))
                                  .ReturnsAsync(true);

            // Act
            var result = await _bookCommentService.AddBookInfo(bookMetaDate);

            // Assert
            Assert.True(result);
        }
    }
}
