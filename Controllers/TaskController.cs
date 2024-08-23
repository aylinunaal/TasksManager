
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using taskManager.Models;
using taskManager.Services;

namespace taskManager.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly TaskService _taskService;
        
        public TaskController(TaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            try
            {
                // Check for the correct claim type
                var claim = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
                if (claim == null)
                {
                    // Log available claims for debugging
                    var allClaims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
                    return Unauthorized(new { Message = "User ID claim not found in token", Claims = allClaims });
                }

                var userId = int.Parse(claim.Value);

                var task = await _taskService.GetTaskById(id, userId);

                if (task == null)
                    return NotFound();

                return Ok(task);
            }
            catch (Exception ex)
            {
                // Log the error and return a generic error message
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskModel task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
                task.UserId = userId;

                await _taskService.AddTask(task);
                return Ok(task);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskRequestModel request)
        {
            var userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var existingTask = await _taskService.GetTaskById(id, userId);
            if (existingTask == null)
                return NotFound();

            existingTask.Title = request.Title;
            existingTask.Description = request.Description;
            existingTask.DueDate = request.DueDate;
            existingTask.IsCompleted = request.IsCompleted;
            existingTask.Type = request.Type;

            await _taskService.UpdateTask(existingTask);
            return Ok(existingTask);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                // Retrieve the user ID from the JWT token using the correct claim type
                var claim = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
                if (claim == null)
                    return Unauthorized("User ID claim not found in token");

                var userId = int.Parse(claim.Value);

                // Check if the task exists
                var task = await _taskService.GetTaskById(id, userId);
                if (task == null)
                    return NotFound();

                // Delete the task
                await _taskService.DeleteTask(id);

                return Ok();
            }
            catch (Exception ex)
            {
                // Log the error and return a generic error message
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
    }
}
