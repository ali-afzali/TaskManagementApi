using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using TaskManagementApi.BL.Interfaces;
using TaskManagementApi.Controllers;
using TaskManagementApi.DAL.Models;

namespace TaskManagementApi.Tests.Controllers;

public class TasksControllerTests
{
    [Fact]
    public async Task GetTask_ReturnsNotFound_WhenTaskDoesNotExist()
    {
        var taskService = new StubTaskService
        {
            GetTaskByIdResult = null
        };

        var controller = CreateController(taskService, userId: 7);

        var result = await controller.GetTask(99);

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task CreateTask_UsesCurrentUserIdAndReturnsCreatedResult()
    {
        var taskService = new StubTaskService
        {
            CreateTaskResult = new TaskItem
            {
                Id = 42,
                Title = "Finish tests",
                Description = "Create API sample tests",
                Status = TaskItemStatus.NotStarted,
                AssigneeUserId = 1,
                CreatedByUserId = 7,
                CreatedDate = DateTime.UtcNow
            }
        };

        var controller = CreateController(taskService, userId: 7);
        var task = new TaskItem
        {
            Title = "Finish tests",
            Description = "Create API sample tests",
            Status = TaskItemStatus.NotStarted,
            AssigneeUserId = 1
        };

        var result = await controller.CreateTask(task);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var createdTask = Assert.IsType<TaskItem>(createdResult.Value);
        Assert.Equal(42, createdTask.Id);
        Assert.Equal(7, taskService.LastUserId);
    }

    [Fact]
    public async Task GetTasks_ReturnsUnauthorized_WhenUserClaimIsMissing()
    {
        var controller = new TasksController(new StubTaskService(), NullLogger<TasksController>.Instance)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };

        var result = await controller.GetTasks();

        Assert.IsType<UnauthorizedObjectResult>(result.Result);
    }

    private static TasksController CreateController(ITaskService taskService, int userId)
    {
        var controller = new TasksController(taskService, NullLogger<TasksController>.Instance);
        var claims = new[] { new Claim("userId", userId.ToString()) };
        var identity = new ClaimsIdentity(claims, "TestAuth");

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(identity)
            }
        };

        return controller;
    }

    private sealed class StubTaskService : ITaskService
    {
        public IEnumerable<TaskItem> GetAllTasksResult { get; set; } = Array.Empty<TaskItem>();
        public TaskItem? GetTaskByIdResult { get; set; }
        public TaskItem CreateTaskResult { get; set; } = new();
        public TaskItem? UpdateTaskResult { get; set; }
        public bool DeleteTaskResult { get; set; }
        public int LastUserId { get; private set; }

        public Task<IEnumerable<TaskItem>> GetAllTasksAsync(int userId)
        {
            LastUserId = userId;
            return Task.FromResult(GetAllTasksResult);
        }

        public Task<TaskItem?> GetTaskByIdAsync(int id, int userId)
        {
            LastUserId = userId;
            return Task.FromResult(GetTaskByIdResult);
        }

        public Task<TaskItem> CreateTaskAsync(TaskItem task, int userId)
        {
            LastUserId = userId;
            return Task.FromResult(CreateTaskResult);
        }

        public Task<TaskItem?> UpdateTaskAsync(int id, TaskItem task, int userId)
        {
            LastUserId = userId;
            return Task.FromResult(UpdateTaskResult);
        }

        public Task<bool> DeleteTaskAsync(int id, int userId)
        {
            LastUserId = userId;
            return Task.FromResult(DeleteTaskResult);
        }
    }
}
