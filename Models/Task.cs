using System.ComponentModel.DataAnnotations.Schema;

namespace EasyManagement.API.Models
{
    public class Task
    {
        public int id { get; set; }
        public string title { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string priority { get; set; } = "Low";
        public string status { get; set; } = "In Progress";
        public string room_code { get; set; } = string.Empty;
        public DateTime created_at { get; set; } = DateTime.UtcNow;

        [ForeignKey("User")]
        public int user_id { get; set; }
        public User User { get; set; } = null!;

        [ForeignKey("Room")]
        public int room_id { get; set; }    
        public Room Room { get; set; } = null!;
    }
}
