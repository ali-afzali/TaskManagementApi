using Microsoft.EntityFrameworkCore;
using TaskManagementApi.DAL.Data;
using TaskManagementApi.DAL.Models;
using TaskManagementApi.DAL.Repositories;

namespace TaskManagementApi.DAL.Tests.Repositories;

public class TaskRepositoryTests
{
    [Fact]
    public async Task CreateTaskAsync_SetsAuditFieldsAndPersistsTask()
    {
        await using var context = CreateContext();
        var repository = new TaskRepository(context);
        var task = new TaskItem
        {
            Title = "Sample task",
            Description = "Repository test",
            Status = TaskItemStatus.NotStarted,
            AssigneeUserId = 1
        };

        var result = await repository.CreateTaskAsync(task, userId: 9);

        Assert.Equal(9, result.CreatedByUserId);
        Assert.True(result.CreatedDate <= DateTime.UtcNow);
        Assert.Contains(await context.Tasks.ToListAsync(), t => t.Id == result.Id);
    }

    [Fact]
    public async Task UpdateTaskAsync_UpdatesTaskFieldsAndAuditData()
    {
        await using var context = CreateContext();
        var repository = new TaskRepository(context);
        var existingTask = new TaskItem
        {
            Title = "Before update",
            Description = "Old description",
            Status = TaskItemStatus.NotStarted,
            AssigneeUserId = 1
        };

        await repository.CreateTaskAsync(existingTask, userId: 1);

        var update = new TaskItem
        {
            Id = existingTask.Id,
            Title = "After update",
            Description = "New description",
            Status = TaskItemStatus.Completed,
            AssigneeUserId = 2
        };

        var result = await repository.UpdateTaskAsync(update, userId: 4);

        Assert.NotNull(result);
        Assert.Equal("After update", result!.Title);
        Assert.Equal(TaskItemStatus.Completed, result.Status);
        Assert.Equal(2, result.AssigneeUserId);
        Assert.Equal(4, result.UpdatedByUserId);
        Assert.NotNull(result.UpdatedDate);
    }

    [Fact]
    public async Task DeleteTaskAsync_ReturnsFalse_WhenTaskDoesNotExist()
    {
        await using var context = CreateContext();
        var repository = new TaskRepository(context);

        var result = await repository.DeleteTaskAsync(999, userId: 1);

        Assert.False(result);
    }

    private static TaskDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<TaskDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new TaskDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }
}
