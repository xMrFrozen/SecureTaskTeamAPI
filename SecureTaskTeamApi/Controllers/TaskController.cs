using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SecureTaskTeamApi.Data;
using SecureTaskTeamApi.Models;
using SecureTaskTeamApi.Controllers;
using Microsoft.EntityFrameworkCore;

namespace SecureTaskTeamApi.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly AppDBContext _context;
        public TaskController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks()
        {

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);

            return await _context.Tasks
                .Where(t => t.UserID == userId)
                .OrderBy(t => t.deadLine)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<TaskItem>> CreateTask(TaskItem taskItem)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            taskItem.UserID = int.Parse(userIdClaim.Value);
            taskItem.User = null;

            _context.Tasks.Add(taskItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTasks), new
            {
                id = taskItem.Id
            },
            taskItem);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            int userId = int.Parse(userIdClaim!.Value);

            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserID == userId);

            if (task == null) return NotFound("Task not found or you do not have permission to delete it.");

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, TaskItem updatedTask)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            int userId = int.Parse(userIdClaim!.Value);

            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserID == userId);

            if (task == null) return NotFound("Task not found or you do not have permission to modify it.");

            if (!string.IsNullOrWhiteSpace(updatedTask.Title)) task.Title = updatedTask.Title;
            if (!string.IsNullOrWhiteSpace(updatedTask.Description)) task.Description = updatedTask.Description;
            task.IsCompleted = updatedTask.IsCompleted;

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
