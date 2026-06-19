using TaskManagementApi.DAL.Models;

namespace TaskManagementApi.DAL.Data
{
    public static class TaskDataSeeder
    {
        public static List<TaskItem> GetSeedData()
        {
            return new List<TaskItem>
            {
                new TaskItem
                {
                    Id = 1,
                    Title = "Complete project documentation",
                    Description = "Write comprehensive documentation for the Task Management API",
                    Status = TaskItemStatus.InProgress,
                    CreatedDate = DateTime.UtcNow.AddDays(-5)
                },
                new TaskItem
                {
                    Id = 2,
                    Title = "Review pull requests",
                    Description = "Review and merge pending pull requests from team members",
                    Status = TaskItemStatus.Completed,
                    CreatedDate = DateTime.UtcNow.AddDays(-3),
                    UpdatedDate = DateTime.UtcNow.AddDays(-1)
                },
                new TaskItem
                {
                    Id = 3,
                    Title = "Setup CI/CD pipeline",
                    Description = "Configure continuous integration and deployment for the project",
                    Status = TaskItemStatus.NotStarted,
                    CreatedDate = DateTime.UtcNow.AddDays(-2)
                },
                new TaskItem
                {
                    Id = 4,
                    Title = "Implement unit tests",
                    Description = "Add unit tests for all service layer methods",
                    Status = TaskItemStatus.InProgress,
                    CreatedDate = DateTime.UtcNow.AddDays(-1)
                },
                new TaskItem
                {
                    Id = 5,
                    Title = "Update dependencies",
                    Description = "Update all NuGet packages to their latest stable versions",
                    Status = TaskItemStatus.Completed,
                    CreatedDate = DateTime.UtcNow.AddDays(-4),
                    UpdatedDate = DateTime.UtcNow.AddDays(-2)
                }
            };
        }
    }
}
