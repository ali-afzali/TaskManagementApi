using TaskManagementApi.DAL.Models;
using TaskManagementApi.DAL.Models;

namespace TaskManagementApi.DAL.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskItem>> GetAllTasksAsync(int userId);
        Task<TaskItem?> GetTaskByIdAsync(int id, int userId);
        Task<TaskItem> CreateTaskAsync(TaskItem task, int userId);
        Task<TaskItem?> UpdateTaskAsync(TaskItem task, int userId);
        Task<bool> DeleteTaskAsync(int id, int userId);
    }
}
