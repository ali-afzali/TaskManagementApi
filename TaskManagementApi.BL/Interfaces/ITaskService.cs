using TaskManagementApi.DAL.Models;
using TaskManagementApi.DAL.Models;

namespace TaskManagementApi.BL.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskItem>> GetAllTasksAsync(int userId);
        Task<TaskItem?> GetTaskByIdAsync(int id, int userId);
        Task<TaskItem> CreateTaskAsync(TaskItem task, int userId);
        Task<TaskItem?> UpdateTaskAsync(int id, TaskItem task, int userId);
        Task<bool> DeleteTaskAsync(int id, int userId);
    }
}
