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
    }
}
