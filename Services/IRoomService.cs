using EasyManagement.API.Dto;
using EasyManagement.API.Models;

namespace EasyManagement.API.Services
{
    public interface IRoomService
    {
        Task<RoomReadDto> CreateRoomAsync(RoomCreateDto request, int ownerId);
        Task<bool> JoinRoom(string roomCode, int userId, string role);
        Task<IEnumerable<RoomReadDto>> GetRooms(int userId);
        Task<RoomReadDto> UpdateRoom(int userId, RoomUpdateDto request);
        Task<RoomReadDto> DeleteRoom(int userId, RoomDeleteDto request);
    }
}
