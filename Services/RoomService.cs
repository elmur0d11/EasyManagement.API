using AutoMapper;
using EasyManagement.API.Data;
using EasyManagement.API.Dto;
using EasyManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EasyManagement.API.Services
{
    public class RoomService : IRoomService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public RoomService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<RoomReadDto> CreateRoomAsync(RoomCreateDto request ,int ownerId)
        {
            // Map DTO to Room entity
            var room = _mapper.Map<Room>(request);

            // Set additional properties
            room.UniqueCode = GenerateUniqueCode();
            room.OwnerId = ownerId;

            // Save to database
            await _context.rooms.AddAsync(room);

            // Add owner as a member with PM role
            await _context.roomMembers.AddAsync(new RoomMember
            {
                UserId = ownerId,
                Room = room,
                Role = "PM",
                JoinedAt = DateTime.UtcNow,
            });

            // Commit changes
            await _context.SaveChangesAsync();

            // Map back to Read DTO and return
            return _mapper.Map<RoomReadDto>(room);
        }

        public async Task<bool> JoinRoom(string roomCode, int userId, string role)
        {
            // Find room by unique code
            var room = await _context.rooms.FirstOrDefaultAsync(u => u.UniqueCode == roomCode);
            if (room is null) return false;

            // Check if user is already a member
            var isMember = await _context.roomMembers.AnyAsync(rm => rm.RoomId == room.Id && rm.UserId == userId);
            if (isMember) return false;

            // Add user as a member with his role
            var membership = new RoomMember
            {
                UserId = userId,
                RoomId = room.Id,
                Role = role,
                JoinedAt = DateTime.UtcNow,
            };

            // Save membership to database and commit changes
            await _context.roomMembers.AddAsync(membership);
            await _context.SaveChangesAsync();
            // Return success
            return true;
        }
        public async Task<IEnumerable<RoomReadDto>> GetRooms(int userId)
        {
            // Retrieve rooms where the user is a member
            var userRooms = await _context.roomMembers
                .Where(rm => rm.UserId == userId)
                .Include(rm => rm.Room)
                .Select(rm => rm.Room)
                .ToListAsync();

            return _mapper.Map<IEnumerable<RoomReadDto>>(userRooms);
        }

        public async Task<RoomReadDto> UpdateRoom(int userID, RoomUpdateDto request)
        {
            var room = _mapper.Map<Room>(request);

            var rooms = await _context.rooms.FirstOrDefaultAsync(r => r.UniqueCode == request.UniqueCode);
            if (rooms is null) throw new KeyNotFoundException("Room with the specified code does not exist.");

            var isMemeber = await _context.roomMembers.AnyAsync(rm => rm.RoomId == rooms.Id && rm.UserId == userID);
            if (!isMemeber) throw new UnauthorizedAccessException("User is not a member of the specified room.");

            rooms.RoomName = room.RoomName;

            await _context.SaveChangesAsync();

            return _mapper.Map<RoomReadDto>(rooms);
        }

        // Generate a unique code for the room
        private string GenerateUniqueCode() => Guid.NewGuid().ToString().Substring(0, 8).ToUpper();

        
    }
}
