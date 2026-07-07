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
        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [Authorize(Roles = "PM, ProjectManager")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateTask(TaskCreateDto request)
        {
            // Get user id from token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            // Parse user id
            int userId = int.Parse(userIdClaim.Value);

            // Create task
            var result = await _taskService.CreateTaskAsync(request, userId);

            // Return created task
            return Created("", result);
        }

        [Authorize]
        [HttpGet("tasks")]
        public async Task<IActionResult> GetTasks(string roomCode) 
        {
            // Get user id from token
            var userIdClaims = User.FindFirst(ClaimTypes.NameIdentifier);

            // Parse user id
            int userId = int.Parse(userIdClaims.Value);

            // Get tasks
            var tasks = await _taskService.GetTasks(roomCode, userId);

            // Return tasks
            return Ok(tasks);
        }

        [Authorize(Roles = "PM, ProjectManager")]
        [HttpPut("updatePriority")]
        public async Task<IActionResult> UpdateTaskPriority(string roomCode, string taskTitle, TaskPriorityUpdate request)
        {
            // Get user id from token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            // Parse user id
            int userId = int.Parse(userIdClaim.Value);

            // Update task priority
            var result = await _taskService.UpdatePriority(userId, taskTitle, roomCode, request);

            // Return success message
            return Ok(result);
        }

        [Authorize(Roles = "PM, ProjectManager")]
        [HttpPut("updateStatus")]
        public async Task<IActionResult> UpdateTaskStatus(string roomCode, string taskTitle, TaskStatusUpdate request)
        {
            // Get user id from token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
           
            // Parse user id
            int userId = int.Parse(userIdClaim.Value);

            // Update task status
            var result = await _taskService.UpdateStatus(userId, taskTitle, roomCode, request);

            // Return success message
            return Ok(result);
        }

        [Authorize(Roles = "PM, ProjectManager")]
        [HttpPut("updateTask")]
        public async Task<IActionResult> UpdateTask(string roomCode, string taskTitle, TaskUpdateDto request)
        {
            // Get user id from token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
           
            // Parse user id
            int userId = int.Parse(userIdClaim.Value);

            // Update task
            var result = await _taskService.UpdateTask(userId, taskTitle, roomCode, request);

            // Return success message
            return Ok(result);
        }

        [Authorize(Roles = "PM, ProjectManager")]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteTask(TaskDeleteDto request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            int userId = int.Parse(userIdClaim.Value);

            await _taskService.DeleteTask(userId, request);

            return NoContent();
        }

    }
}
