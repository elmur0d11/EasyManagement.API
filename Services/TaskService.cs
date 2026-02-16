using AutoMapper;
using EasyManagement.API.Data;
using EasyManagement.API.Dto;
using Microsoft.EntityFrameworkCore;

namespace EasyManagement.API.Services
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public TaskService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TaskReadDto> CreateTaskAsync(TaskCreateDto request, int userId)
        {
            // Map the incoming DTO to the Task model
            var tasks = _mapper.Map<Models.Task>(request);

            // Verify that the room exists
            var room = await _context.rooms.FirstOrDefaultAsync(r => r.UniqueCode == request.room_code);
            if (room == null) throw new KeyNotFoundException("Room with the specified code does not exist.");

            // Verify that the user is a member of the room
            var isMember = await _context.roomMembers.AnyAsync(rm => rm.RoomId == room.Id && rm.UserId == userId);
            if (!isMember) throw new UnauthorizedAccessException("User is not a member of the specified room.");

            // Set additional properties
            tasks.room_id = room.Id;
            tasks.user_id = userId;
            tasks.status = "Pending";
            tasks.created_at = DateTime.UtcNow;
            tasks.Room = room;

            // Save the task to the database
            await _context.tasks.AddAsync(tasks);
            await _context.SaveChangesAsync();

            // Map the saved task back to a read DTO and return it
            return _mapper.Map<TaskReadDto>(tasks);
        }

        public async Task<IEnumerable<TaskReadDto>> GetTasks(string roomCode, int userId)
        {
            // Verify that the room exists
            var room = await _context.rooms.FirstOrDefaultAsync(r => r.UniqueCode == roomCode);
            if (room is null) throw new KeyNotFoundException("Room with the specified code does not exist.");

            // Verify that the user is a member of the room
            var isMemeber = await _context.roomMembers.AnyAsync(rm => rm.RoomId == room.Id && rm.UserId == userId);
            if(!isMemeber) throw new UnauthorizedAccessException("User is not a member of the specified room.");

            // Retrieve all tasks associated with the room
            var allTasks = await _context.tasks
                .Where(t => t.room_id == room.Id)
                .ToListAsync();

            // Map the tasks to read DTOs and return them
            return _mapper.Map<IEnumerable<TaskReadDto>>(allTasks);
        }

        public async Task<TaskReadDto> UpdatePriority(int userId, string taskTitle, string roomCode, TaskPriorityUpdate request)
        {
            // Verify that the room exists
            var room = await _context.rooms.FirstOrDefaultAsync(r => r.UniqueCode == roomCode);
            if (room is null) throw new KeyNotFoundException("Room with the specified code does not exist.");

            // Verify that the user is a member of the room
            var isMemeber = await _context.roomMembers.AnyAsync(rm => rm.RoomId == room.Id && rm.UserId == userId);
            if (!isMemeber) throw new UnauthorizedAccessException("User is not a member of the specified room.");

            // Verify that the task exists in the specified room
            var task = await _context.tasks.FirstOrDefaultAsync(t => t.title == taskTitle && t.room_id == room.Id);
            if (task is null) throw new KeyNotFoundException("Task with the specified title does not exist in the specified room.");

            // Update the task's priority
            task.priority = request.Priority;

            // Save the changes to the database
            await _context.SaveChangesAsync();

            // Map the updated task to a read DTO and return it
            return _mapper.Map<TaskReadDto>(task);
        }

        public async Task<TaskReadDto> UpdateStatus(int userId, string taskTitle, string roomCode, TaskStatusUpdate request)
        {
            // Verify that the room exists
            var room = await _context.rooms.FirstOrDefaultAsync(r => r.UniqueCode == roomCode);
            if (room is null) throw new KeyNotFoundException("Room with the specified code does not exist.");

            // Verify that the user is a member of the room
            var isMemeber = await _context.roomMembers.AnyAsync(rm => rm.RoomId == room.Id && rm.UserId == userId);
            if (!isMemeber) throw new UnauthorizedAccessException("User is not a member of the specified room.");

            // Verify that the task exists in the specified room
            var task = await _context.tasks.FirstOrDefaultAsync(t => t.title == taskTitle && t.room_id == room.Id);
            if (task is null) throw new KeyNotFoundException("Task with the specified title does not exist in the specified room.");

            // Update the task's status
            task.status = request.Status;

            // Save the changes to the database
            await _context.SaveChangesAsync();

            // Map the updated task to a read DTO and return it
            return _mapper.Map<TaskReadDto>(task);
        }

    }
}
