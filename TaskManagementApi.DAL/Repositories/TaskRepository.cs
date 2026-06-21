using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManagementApi.DAL.Data;
using TaskManagementApi.DAL.Interfaces;
using TaskManagementApi.DAL.Models;

namespace TaskManagementApi.DAL.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskDbContext _context;

        public TaskRepository(TaskDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskItem>> GetAllTasksAsync(int userId)
        {
            // Return all tasks (no filtering by userId for assignment-based system)
            // userId parameter kept for consistency with interface
            return await _context.Tasks.ToListAsync();
        }

        public async Task<TaskItem?> GetTaskByIdAsync(int id, int userId)
        {
            // Return task regardless of userId (assignment-based, not ownership)
            return await _context.Tasks.FindAsync(id);
        }

        public async Task<TaskItem> CreateTaskAsync(TaskItem task, int userId)
        {
            task.CreatedByUserId = userId;
            task.CreatedDate = DateTime.UtcNow;
            // AssigneeUserId should be set by the caller (who the task is assigned to)
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<TaskItem?> UpdateTaskAsync(TaskItem task, int userId)
        {
            var existingTask = await _context.Tasks.FindAsync(task.Id);

            if (existingTask == null)
            {
                return null;
            }

            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.Status = task.Status;
            existingTask.AssigneeUserId = task.AssigneeUserId; // Can reassign tasks
            existingTask.UpdatedDate = DateTime.UtcNow;
            existingTask.UpdatedByUserId = userId;

            await _context.SaveChangesAsync();
            return existingTask;
        }

        public async Task<bool> DeleteTaskAsync(int id, int userId)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return false;
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
