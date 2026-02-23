using EasyManagement.API.Dto;
using EasyManagement.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EasyManagement.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomsController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateRoom(RoomCreateDto request)
        {
            // Extract user ID from JWT token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");

            // Parse user ID
            int userId = int.Parse(userIdClaim.Value);

            // Call the service to create the room
            var result = await _roomService.CreateRoomAsync(request, userId);

            // Return the result
            return Ok(result);
        }

        [Authorize]
        [HttpPost("join")]
        public async Task<IActionResult> JoinRoom(JoinRoomDto request)
        {
            // Extract user ID from JWT token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");

            // Extract user role from JWT token
            var userRoleClaim = User.FindFirst(ClaimTypes.Role);
            if (userRoleClaim == null)
                return BadRequest("Can't dedect role, please select your role before joining");

            // Parse user ID
            int userId = int.Parse(userIdClaim.Value);
            // Get user role
            var userRole = userRoleClaim.Value;

            // Call the service to join the room
            var success = await _roomService.JoinRoom(request.RoomCode ,userId, userRole);

            // Return the result
            if (!success)
                return BadRequest("Failed to join the room. Please check the room code and try again.");

            // Return success message
            return Ok(new { message = "You Joined the room successfully" });
        }

        [Authorize(Roles = "PM, ProjectManager")]
        [HttpPut("rename-room")]
        public async Task<IActionResult> RenameRoom(RoomUpdateDto request)
        {
            var userIdClaims = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaims == null)
                return Unauthorized("User ID not found in token.");
            // Parse user ID
            int userId = int.Parse(userIdClaims.Value);

            var rooms = await _roomService.UpdateRoom(userId, request);

            return Ok(rooms);
        }

        [Authorize]
        [HttpGet("my-rooms")]
        public async Task<IActionResult> GetMyRooms()
        {
            // Extract user ID from JWT token
            var userIdClaims = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaims == null)
                return Unauthorized("User ID not found in token.");
            // Parse user ID
            int userId = int.Parse(userIdClaims.Value);

            // Call the service to get user's rooms
            var rooms = await _roomService.GetRooms(userId);

            // Return the list of rooms
            return Ok(rooms);
        }

    }
}
