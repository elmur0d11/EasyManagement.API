using AutoMapper;
using EasyManagement.API.Dto;
using EasyManagement.API.Models;

namespace EasyManagement.API.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserCreateDto, User>()
                .ForMember(dest => dest.PasswordHash,
                opt => opt.MapFrom(src => src.Password));
            CreateMap<User, UserReadDto>();
            CreateMap<UserLoginDto, User>()
                .ForMember(dest => dest.PasswordHash,
                opt => opt.MapFrom(src => src.Password));
        }
    }
}
