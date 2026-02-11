namespace EasyManagement.API.Models
{
    public class RoomMember
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public int RoomId { get; set; }
        public Room Room { get; set; } = null!;
        public string Role { get; set; } = "User";
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    }
}
