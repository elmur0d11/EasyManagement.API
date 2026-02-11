using EasyManagement.API.Models;

namespace EasyManagement.API.Dto
{
    public class RoomReadDto
    {
        public string RoomName { get; set; } = string.Empty;
        public string UniqueCode { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
