using AutoMapper;
using EasyManagement.API.Dto;

namespace EasyManagement.API.Profiles
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<TaskCreateDto, Models.Task>();
            CreateMap<Models.Task, TaskReadDto>();
        }
    }
}
