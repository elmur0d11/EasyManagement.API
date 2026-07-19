using EasyManagement.API.Dto;
using EasyManagement.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EasyManagement.API.Controllers
{
    [Route("api/v1/task")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ILogger<TasksController> _logger;
        public TasksController(ITaskService taskService, ILogger<TasksController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        [Authorize(Roles = "PM, ProjectManager")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateTask(TaskCreateDto request)
        {
            _logger.LogInformation("Creating task. Title: {title}, Room: {room_code}", request.Title, request.RoomCode);
            // Get user id from token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            // Parse user id
            int userId = int.Parse(userIdClaim.Value);
            // Create task
            var result = await _taskService.CreateTaskAsync(request, userId);
            _logger.LogInformation("Task created successfully. Title: {title}, Room: {room_code}", request.Title, request.RoomCode);
            // Return created task
            return Created("", result);
        }

        [Authorize]
        [HttpGet("tasks")]
        public async Task<IActionResult> GetTasks(string roomCode) 
        {
            _logger.LogInformation("Fetching tasks. Room: {room_code}", roomCode);
            // Get user id from token
            var userIdClaims = User.FindFirst(ClaimTypes.NameIdentifier);
            // Parse user id
            int userId = int.Parse(userIdClaims.Value);
            // Get tasks
            var tasks = await _taskService.GetTasks(roomCode, userId);
            _logger.LogInformation("Tasks fetched successfully. Room: {room_code}", roomCode);
            // Return tasks
            return Ok(tasks);
        }

        [Authorize(Roles = "PM, ProjectManager")]
        [HttpPut("updatePriority")]
        public async Task<IActionResult> UpdateTaskPriority(string roomCode, string taskTitle, TaskPriorityUpdate request)
        {
            _logger.LogInformation("Updating task priority. Room: {roomCode}, Task: {taskTitle}", roomCode, taskTitle);
            // Get user id from token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            // Parse user id
            int userId = int.Parse(userIdClaim.Value);
            // Update task priority
            var result = await _taskService.UpdatePriority(userId, taskTitle, roomCode, request);
            _logger.LogInformation("Task priority updated successfully. Room: {roomCode}, Task: {taskTitle}", roomCode, taskTitle);
            // Return success message
            return Ok(result);
        }

        [Authorize(Roles = "PM, ProjectManager")]
        [HttpPut("updateStatus")]
        public async Task<IActionResult> UpdateTaskStatus(string roomCode, string taskTitle, TaskStatusUpdate request)
        {
            _logger.LogInformation("Updating task status. Room: {roomCode}, Task: {taskTitle}", roomCode, taskTitle);
            // Get user id from token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            // Parse user id
            int userId = int.Parse(userIdClaim.Value);
            // Update task status
            var result = await _taskService.UpdateStatus(userId, taskTitle, roomCode, request);
            _logger.LogInformation("Task status updated successfully. Room: {roomCode}, Task: {taskTitle}", roomCode, taskTitle);
            // Return success message
            return Ok(result);
        }

        [Authorize(Roles = "PM, ProjectManager")]
        [HttpPut("updateTask")]
        public async Task<IActionResult> UpdateTask(string roomCode, string taskTitle, TaskUpdateDto request)
        {
            _logger.LogInformation("Updating task. Room: {roomCode}, Task: {taskTitle}", roomCode, taskTitle);
            // Get user id from token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            // Parse user id
            int userId = int.Parse(userIdClaim.Value);
            // Update task
            var result = await _taskService.UpdateTask(userId, taskTitle, roomCode, request);
            _logger.LogInformation("Task updated successfully. Room: {roomCode}, Task: {taskTitle}", roomCode, taskTitle);
            // Return success message
            return Ok(result);
        }

        [Authorize(Roles = "PM, ProjectManager")]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteTask(TaskDeleteDto request)
        {
            _logger.LogInformation("Deleting task. Room: {roomCode}, Task: {taskTitle}", request.RoomCode, request.TaskTitle);
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            int userId = int.Parse(userIdClaim.Value);
            await _taskService.DeleteTask(userId, request);
            _logger.LogInformation("Task deleted successfully. Room: {roomCode}, Task: {taskTitle}", request.RoomCode, request.TaskTitle);
            return NoContent();
        }

    }
}
