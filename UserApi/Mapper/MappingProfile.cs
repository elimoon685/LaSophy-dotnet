using AutoMapper;
using UserApi.DTO;
using UserApi.Models;

namespace UserApi.Mapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile() 
        { 
            CreateMap<User, UserInfoResponseDto>(); 
        }
    }
}
