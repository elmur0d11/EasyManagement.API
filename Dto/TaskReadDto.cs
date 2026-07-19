using EasyManagement.API.Enums;

namespace EasyManagement.API.Dto
{
    public class TaskReadDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskPriority Priority { get; set; }
        public ProjectTaskStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
