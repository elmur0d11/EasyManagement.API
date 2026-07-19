using AutoMapper;
using EasyManagement.API.Data;
using EasyManagement.API.Dto;
using EasyManagement.API.Exceptions;
using EasyManagement.API.Models;
using Microsoft.EntityFrameworkCore;
using KeyNotFoundException = EasyManagement.API.Exceptions.KeyNotFoundException;
using UnauthorizedAccessException = EasyManagement.API.Exceptions.UnauthorizedAccessException;

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
            await _context.Rooms.AddAsync(room);

            // Add owner as a member with PM role
            await _context.RoomMembers.AddAsync(new RoomMember
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
            var room = await _context.Rooms.FirstOrDefaultAsync(u => u.UniqueCode == roomCode);
            if (room is null) throw new KeyNotFoundException("Room not found"); ;

            // Check if user is already a member
            var isMember = await _context.RoomMembers.AnyAsync(rm => rm.RoomId == room.Id && rm.UserId == userId);
            if (isMember) throw new Exception("You already joined this room!"); ;

            // Add user as a member with his role
            var membership = new RoomMember
            {
                UserId = userId,
                RoomId = room.Id,
                Role = role,
                JoinedAt = DateTime.UtcNow,
            };

            // Save membership to database and commit changes
            await _context.RoomMembers.AddAsync(membership);
            await _context.SaveChangesAsync();
            // Return success
            return true;
        }

        public async Task<IEnumerable<RoomReadDto>> GetRooms(int userId)
        {
            // Retrieve rooms where the user is a member
            var userRooms = await _context.RoomMembers
                .Where(rm => rm.UserId == userId)
                .Include(rm => rm.Room)
                .Select(rm => rm.Room)
                .ToListAsync();

            return _mapper.Map<IEnumerable<RoomReadDto>>(userRooms);
        }

        public async Task<RoomReadDto> UpdateRoom(int userId, RoomUpdateDto request)
        {
            var room = _mapper.Map<Room>(request);

            var rooms = await _context.Rooms.FirstOrDefaultAsync(r => r.UniqueCode == request.UniqueCode);
            if (rooms is null) throw new KeyNotFoundException("Room with the specified code does not exist.");

            if (rooms.OwnerId != userId)
                throw new UnauthorizedAccessException("Only the room owner can rename the room.");

            rooms.RoomName = room.RoomName;

            await _context.SaveChangesAsync();

            return _mapper.Map<RoomReadDto>(rooms);
        }

        public async Task<RoomReadDto> DeleteRoom(int userId, RoomDeleteDto request)
        {
            var rooms = await _context.Rooms.FirstOrDefaultAsync(r => r.UniqueCode == request.RoomCode);
            if (rooms is null) throw new KeyNotFoundException("Room with the specified code does not exist.");

            if (request.RoomNameReply != rooms.RoomName) throw new BadRequestException("Please reply title of the task.");

            if (rooms.OwnerId != userId)
                throw new UnauthorizedAccessException("Only the room owner can delete the room.");

            _context.Rooms.Remove(rooms);
            await _context.SaveChangesAsync();

            return null!;
        }

        // Generate a unique code for the room
        private string GenerateUniqueCode() => Guid.NewGuid().ToString().Substring(0, 8).ToUpper();

        
    }
}
