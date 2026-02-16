using EasyManagement.API.Dto;

namespace EasyManagement.API.Services
{
    public interface ITaskService
    {
        Task<TaskReadDto> CreateTaskAsync(TaskCreateDto request, int userId);
        Task<IEnumerable<TaskReadDto>> GetTasks(string roomCode, int userId);
        Task<TaskReadDto> UpdatePriority(int userId, string taskTitle, string roomCode, TaskPriorityUpdate request);
        Task<TaskReadDto> UpdateStatus(int userId, string taskTitle, string roomCode, TaskStatusUpdate request);
        Task<TaskReadDto> UpdateTask(int userId, string taskTitle, string roomCode, TaskUpdateDto request);
    }
}
