using AutoMapper;
using CommentsApi.DTO;
using CommentsApi.Models;
using SharedContract.HttpClient;

namespace CommentsApi
{
    public class MappingProfile:Profile
    {

        public MappingProfile() 
        {
            CreateMap<BookMetaData, BookInfo>();
            CreateMap<CreateCommentsRequestDto, BookComments>();
            CreateMap<BookComments, CommentsDto>();
        
        }
    }
}
