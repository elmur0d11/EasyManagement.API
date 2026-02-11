namespace EasyManagement.API.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public string UniqueCode { get; set; } = string.Empty;

        public int OwnerId { get; set; }
        public User Owner { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
