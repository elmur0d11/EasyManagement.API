using EasyManagement.API.Dto;
using EasyManagement.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EasyManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [Authorize(Roles = "PM, ProjectManager")]
        [HttpPost("create-task")]
        public async Task<IActionResult> CreateTask(TaskCreateDto request)
        {
            // Get user id from token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");

            // Parse user id
            int userId = int.Parse(userIdClaim.Value);

            // Create task
            var result = await _taskService.CreateTaskAsync(request, userId);
            if (result is null) return BadRequest("Can't create task");

            // Return created task
            return Created("", result);
        }

        [Authorize]
        [HttpGet("get-tasks")]
        public async Task<IActionResult> GetTasks(string roomCode) 
        {
            // Get user id from token
            var userIdClaims = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaims == null)
                return Unauthorized("User ID not found in token.");

            // Parse user id
            int userId = int.Parse(userIdClaims.Value);

            // Get tasks
            var tasks = await _taskService.GetTasks(roomCode, userId);

            // Return tasks
            return Ok(tasks);
        }

        [Authorize(Roles = "PM, ProjectManager")]
        [HttpPut("update-priority")]
        public async Task<IActionResult> UpdateTaskPriority(string roomCode, string taskTitle, TaskPriorityUpdate request)
        {
            // Get user id from token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");

            // Parse user id
            int userId = int.Parse(userIdClaim.Value);

            // Update task priority
            var result = await _taskService.UpdatePriority(userId, taskTitle, roomCode, request);

            // Check if update was successful
            if (result is null) return BadRequest("Can't update task priority");

            // Return success message
            return Ok(result);
        }

        [Authorize(Roles = "PM, ProjectManager")]
        [HttpPut("update-status")]
        public async Task<IActionResult> UpdateTaskStatus(string roomCode, string taskTitle, TaskStatusUpdate request)
        {
            // Get user id from token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");
            // Parse user id
            int userId = int.Parse(userIdClaim.Value);

            // Update task status
            var result = await _taskService.UpdateStatus(userId, taskTitle, roomCode, request);
            if (result is null) return BadRequest("Can't update task status");

            // Return success message
            return Ok(result);
        }

        [Authorize(Roles = "PM, ProjectManager")]
        [HttpPut("update-task")]
        public async Task<IActionResult> UpdateTask(string roomCode, string taskTitle, TaskUpdateDto request)
        {
            // Get user id from token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");
            // Parse user id
            int userId = int.Parse(userIdClaim.Value);

            // Update task
            var result = await _taskService.UpdateTask(userId, taskTitle, roomCode, request);
            if (result is null) return BadRequest("Can't update task");

            // Return success message
            return Ok(result);
        }
           

        }
}
