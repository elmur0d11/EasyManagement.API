using AutoMapper;
using EasyManagement.API.Dto;
using EasyManagement.API.Models;

namespace EasyManagement.API.Profiles
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            CreateMap<RoomCreateDto, Room>();
            CreateMap<Room, RoomReadDto>();
        }
    }
}
