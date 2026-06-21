using TaskManagementApi.BL.Services;
using TaskManagementApi.DAL.Interfaces;
using TaskManagementApi.DAL.Models;

namespace TaskManagementApi.BL.Tests.Services;

public class TaskServiceTests
{
    [Fact]
    public async Task CreateTaskAsync_ThrowsArgumentException_WhenTitleIsMissing()
    {
        var service = new TaskService(new StubTaskRepository());
        var task = new TaskItem { Title = "   " };

        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateTaskAsync(task, userId: 1));
    }

    [Fact]
    public async Task UpdateTaskAsync_SetsTaskIdBeforeCallingRepository()
    {
        var repository = new StubTaskRepository
        {
            UpdateResult = new TaskItem { Id = 15, Title = "Updated title" }
        };

        var service = new TaskService(repository);
        var task = new TaskItem
        {
            Title = "Updated title",
            Description = "Updated description",
            Status = TaskItemStatus.InProgress,
            AssigneeUserId = 2
        };

        var result = await service.UpdateTaskAsync(15, task, userId: 8);

        Assert.NotNull(result);
        Assert.Equal(15, repository.LastUpdatedTask?.Id);
    }

    private sealed class StubTaskRepository : ITaskRepository
    {
        public TaskItem? LastUpdatedTask { get; private set; }
        public TaskItem UpdateResult { get; set; } = new();

        public Task<IEnumerable<TaskItem>> GetAllTasksAsync(int userId)
        {
            return Task.FromResult<IEnumerable<TaskItem>>(Array.Empty<TaskItem>());
        }

        public Task<TaskItem?> GetTaskByIdAsync(int id, int userId)
        {
            return Task.FromResult<TaskItem?>(null);
        }

        public Task<TaskItem> CreateTaskAsync(TaskItem task, int userId)
        {
            return Task.FromResult(task);
        }

        public Task<TaskItem?> UpdateTaskAsync(TaskItem task, int userId)
        {
            LastUpdatedTask = task;
            return Task.FromResult<TaskItem?>(UpdateResult);
        }

        public Task<bool> DeleteTaskAsync(int id, int userId)
        {
            return Task.FromResult(false);
        }
    }
}
