using EasyManagement.API.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyManagement.API.Models
{
    public class Task
    {
        public int id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskPriority Priority { get; set; } = TaskPriority.Low;
        public ProjectTaskStatus Status { get; set; } = ProjectTaskStatus.Todo;
        public string RoomCode { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        [ForeignKey("Room")]
        public int RoomId { get; set; }    
        public Room Room { get; set; } = null!;
    }
}
