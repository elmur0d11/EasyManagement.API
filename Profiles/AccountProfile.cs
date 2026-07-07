using AutoMapper;
using EasyManagement.API.Dto;
using EasyManagement.API.Models;

namespace EasyManagement.API.Profiles
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<User, AccountReadDto>();
        }
    }
}
