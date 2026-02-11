using AutoMapper;
using EasyManagement.API.Dto;
using EasyManagement.API.Models;

namespace EasyManagement.API.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserCreateDto, User>();
            CreateMap<User, UserReadDto>();
            CreateMap<UserLoginDto, User>();
        }
    }
}
