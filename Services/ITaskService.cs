using EasyManagement.API.Dto;

namespace EasyManagement.API.Services
{
    public interface ITaskService
    {
        Task<TaskReadDto> CreateTaskAsync(TaskCreateDto request, int userId);
        Task<IEnumerable<TaskReadDto>> GetTasks(string roomCode, int userId);
    }
}
