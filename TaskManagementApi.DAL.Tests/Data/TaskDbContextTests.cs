using Microsoft.EntityFrameworkCore;
using TaskManagementApi.DAL.Data;

namespace TaskManagementApi.DAL.Tests.Data;

public class TaskDbContextTests
{
    [Fact]
    public async Task EnsureCreated_SeedsDefaultUserAndTasks()
    {
        await using var context = CreateContext();

        var userCount = await context.Users.CountAsync();
        var taskCount = await context.Tasks.CountAsync();
        var adminUser = await context.Users.SingleOrDefaultAsync(user => user.Username == "admin");

        Assert.Equal(1, userCount);
        Assert.Equal(5, taskCount);
        Assert.NotNull(adminUser);
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
