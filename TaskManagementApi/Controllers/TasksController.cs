using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TaskManagementApi.BL.Interfaces;
using TaskManagementApi.DAL.Models;

namespace TaskManagementApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ILogger<TasksController> _logger;

        public TasksController(ITaskService taskService, ILogger<TasksController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        [HttpGet(Name = "GetTasks")]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks()
        {
            try
            {
                var tasks = await _taskService.GetAllTasksAsync();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving tasks");
                return StatusCode(500, "An error occurred while retrieving tasks");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> GetTask(int id)
        {
            try
            {
                var task = await _taskService.GetTaskByIdAsync(id);
                if (task == null)
                {
                    return NotFound($"Task with ID {id} not found");
                }
                return Ok(task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving task {TaskId}", id);
                return StatusCode(500, "An error occurred while retrieving the task");
            }
        }

        [HttpPost(Name = "CreateTask")]
        public async Task<ActionResult<TaskItem>> CreateTask([FromBody] TaskItem task)
        {
            try
            {
                var createdTask = await _taskService.CreateTaskAsync(task);
                return CreatedAtAction(nameof(GetTask), new { id = createdTask.Id }, createdTask);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid task data");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating task");
                return StatusCode(500, "An error occurred while creating the task");
            }
        }

        [HttpPut("{id}", Name = "UpdateTask")]
        public async Task<ActionResult<TaskItem>> UpdateTask(int id, [FromBody] TaskItem task)
        {
            try
            {
                var updatedTask = await _taskService.UpdateTaskAsync(id, task);
                if (updatedTask == null)
                {
                    return NotFound($"Task with ID {id} not found");
                }
                return Ok(updatedTask);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid task data");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating task {TaskId}", id);
                return StatusCode(500, "An error occurred while updating the task");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTask(int id)
        {
            try
            {
                var result = await _taskService.DeleteTaskAsync(id);
                if (!result)
                {
                    return NotFound($"Task with ID {id} not found");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting task {TaskId}", id);
                return StatusCode(500, "An error occurred while deleting the task");
            }
        }
    }
}
