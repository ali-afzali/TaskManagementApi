using TaskManagementApi.BL.Interfaces;
using TaskManagementApi.BL.Interfaces;
using TaskManagementApi.DAL.Interfaces;
using TaskManagementApi.DAL.Models;

namespace TaskManagementApi.BL.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<TaskItem>> GetAllTasksAsync(int userId)
        {
            return await _taskRepository.GetAllTasksAsync(userId);
        }

        public async Task<TaskItem?> GetTaskByIdAsync(int id, int userId)
        {
            return await _taskRepository.GetTaskByIdAsync(id, userId);
        }

        public async Task<TaskItem> CreateTaskAsync(TaskItem task, int userId)
        {
            if (string.IsNullOrWhiteSpace(task.Title))
            {
                throw new ArgumentException("Task title is required", nameof(task.Title));
            }

            return await _taskRepository.CreateTaskAsync(task, userId);
        }

        public async Task<TaskItem?> UpdateTaskAsync(int id, TaskItem task, int userId)
        {
            if (string.IsNullOrWhiteSpace(task.Title))
            {
                throw new ArgumentException("Task title is required", nameof(task.Title));
            }

            task.Id = id;
            return await _taskRepository.UpdateTaskAsync(task, userId);
        }

        public async Task<bool> DeleteTaskAsync(int id, int userId)
        {
            return await _taskRepository.DeleteTaskAsync(id, userId);
        }
    }
}
