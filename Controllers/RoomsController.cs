using EasyManagement.API.Dto;
using EasyManagement.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EasyManagement.API.Controllers
{
    [Authorize]
    [Route("api/v1/room")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly ILogger<RoomsController> _logger;

        public RoomsController(IRoomService roomService, ILogger<RoomsController> logger)
        {
            _roomService = roomService;
            _logger = logger;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateRoom(RoomCreateDto request)
        {
            _logger.LogInformation("Creating a new room. Name: {RoomName}", request.RoomName);
            // Extract user ID from JWT token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            // Parse user ID
            int userId = int.Parse(userIdClaim.Value);
            // Call the service to create the room
            var result = await _roomService.CreateRoomAsync(request, userId);
            _logger.LogInformation("Room created successfully. RoomCode: {RoomCode}", result.UniqueCode);
            // Return the result
            return Ok(result);
        }

        [Authorize]
        [HttpPost("join")]
        public async Task<IActionResult> JoinRoom(JoinRoomDto request)
        {
            _logger.LogInformation("User attempting to join room. RoomCode: {RoomCode}", request.RoomCode);
            // Extract user ID from JWT token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            // Extract user role from JWT token
            var userRoleClaim = User.FindFirst(ClaimTypes.Role);
            // Parse user ID
            int userId = int.Parse(userIdClaim.Value);
            // Get user role
            var userRole = userRoleClaim.Value;
            // Call the service to join the room
            var success = await _roomService.JoinRoom(request.RoomCode ,userId, userRole);
            _logger.LogInformation("User joined room successfully. RoomCode: {RoomCode}, UserId: {UserId}", request.RoomCode, userId);
            // Return success message
            return Ok(new { message = "You Joined the room successfully" });
        }

        [Authorize]
        [HttpPut("rename")]
        public async Task<IActionResult> RenameRoom(RoomUpdateDto request)
            {
                _logger.LogInformation("Renaming room. RoomCode: {RoomCode}, NewName: {NewName}", request.UniqueCode, request.RoomName);
                var userIdClaims = User.FindFirst(ClaimTypes.NameIdentifier);
                // Parse user ID
                int userId = int.Parse(userIdClaims.Value);
                var rooms = await _roomService.UpdateRoom(userId, request);
                _logger.LogInformation("Room renamed successfully. RoomCode: {RoomCode}, NewName: {NewName}", request.UniqueCode, request.RoomName);
                return Ok(rooms);
            }

        [Authorize]
        [HttpGet("rooms")]
        public async Task<IActionResult> GetMyRooms()
        {
            _logger.LogInformation("Fetching rooms for user.");
            // Extract user ID from JWT token
            var userIdClaims = User.FindFirst(ClaimTypes.NameIdentifier);
            // Parse user ID
            int userId = int.Parse(userIdClaims.Value);
            // Call the service to get user's rooms
            var rooms = await _roomService.GetRooms(userId);
            _logger.LogInformation("Rooms fetched successfully for user. UserId: {userId}", userId);
            // Return the list of rooms
            return Ok(rooms);
        }

        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteRoom(RoomDeleteDto request)
        {
            _logger.LogInformation("Deleting room. RoomCode: {RoomCode}", request.RoomCode);
            // Extract user ID from JWT token
            var userIdClaims = User.FindFirst(ClaimTypes.NameIdentifier);
            // Parse user ID
            int userId = int.Parse(userIdClaims.Value);
            var room = await _roomService.DeleteRoom(userId, request);
            _logger.LogInformation("Room deleted successfully. RoomCode: {RoomCode}", request.RoomCode);
            return NoContent();
        }
    }
}
