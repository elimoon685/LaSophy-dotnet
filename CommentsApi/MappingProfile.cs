using AutoMapper;
using CommentsApi.DTO;
using CommentsApi.DTO.Request;
using CommentsApi.DTO.Response;
using CommentsApi.Models;
using SharedContract.HttpClient;

namespace CommentsApi
{
    public class MappingProfile:Profile
    {

        public MappingProfile() 
        {
            CreateMap<BookMetaData, BookInfo>();
            CreateMap<CreateCommentsRequestDto, BookComments>();//reques to entity
            CreateMap<BookComments, GetCommentsResponseDto>()
            .ForMember(dest => dest.CommentLikesCount, opt => opt.MapFrom(src => src.CommentLikes != null ? src.CommentLikes.Count : 0));
            CreateMap<BookInfo, BookInfoDto>();
            CreateMap<ToggleBookLikeCollectRequestDto, BookCollection>();
            CreateMap<ToggleBookLikeCollectRequestDto, BookLike>();
            CreateMap<ToggleBookCommentLikeRequestDto,CommentLike>();
        }
    }
}
