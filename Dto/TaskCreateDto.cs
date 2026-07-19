using EasyManagement.API.Enums;

namespace EasyManagement.API.Dto
{
    public class TaskCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskPriority Priority { get; set; }
        public ProjectTaskStatus Status { get; set; }
        public string RoomCode { get; set; } = string.Empty;
    }
}
