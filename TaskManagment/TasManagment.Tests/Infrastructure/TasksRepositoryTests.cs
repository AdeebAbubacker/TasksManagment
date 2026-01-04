using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;
using TaskManagement.Infrastructure;
using TaskManagement.Infrastructure.Repositories;
using TaskManagment.Application.Features.Tasks.Queries.GetMyTasksList;
using TaskManagment.Domain.Entities;

namespace TaskManagement.Tests.Infrastructure.Repositories
{
    [TestClass]
    public class TasksRepositoryTests
    {
        private TaskManagmentDbContext context;
        private TasksRepository repository;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<TaskManagmentDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // isolation
                .Options;

            context = new TaskManagmentDbContext(options);
            repository = new TasksRepository(context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [TestMethod]
        public async Task GetUserFilter_ReturnsOnlyUserTasks()
        {
            // Arrange
            var user1 = "user-1";
            var user2 = "user-2";

            context.TaskItems.AddRange(
                new TaskItem("Task A", null, null, user1),
                new TaskItem("Task B", null, null, user1),
                new TaskItem("Task C", null, null, user2)
            );

            await context.SaveChangesAsync();

            var filter = new MyTasksFilterDTO
            {
                Page = 1,
                RecordsPerPage = 10
            };

            // Act
            var result = await repository.GetUserFilter(user1, filter);

            // Assert
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.All(t => t.OwnerUserId == user1));
        }

        [TestMethod]
        public async Task GetUserFilter_DoesNotLeakOtherUsersTasks()
        {
            // Arrange
            context.TaskItems.AddRange(
                new TaskItem("User1 Task", null, null, "user1"),
                new TaskItem("User2 Task", null, null, "user2")
            );

            await context.SaveChangesAsync();

            var filter = new MyTasksFilterDTO
            {
                Page = 1,
                RecordsPerPage = 10
            };

            // Act
            var result = await repository.GetUserFilter("user1", filter);

            // Assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("user1", result.First().OwnerUserId);
        }

        [TestMethod]
        public async Task GetUserFilter_FiltersByTitle()
        {
            // Arrange
            var userId = "user-1";

            context.TaskItems.AddRange(
                new TaskItem("Buy Milk", null, null, userId),
                new TaskItem("Buy Bread", null, null, userId),
                new TaskItem("Clean House", null, null, userId)
            );

            await context.SaveChangesAsync();

            var filter = new MyTasksFilterDTO
            {
                Title = "Buy",
                Page = 1,
                RecordsPerPage = 10
            };

            // Act
            var result = await repository.GetUserFilter(userId, filter);

            // Assert
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.All(t => t.Title.Contains("Buy")));
        }

        [TestMethod]
        public async Task GetUserFilter_AppliesPagination()
        {
            // Arrange
            var userId = "user-1";

            for (int i = 1; i <= 10; i++)
            {
                context.TaskItems.Add(
                    new TaskItem($"Task {i}", null, null, userId)
                );
            }

            await context.SaveChangesAsync();

            var filter = new MyTasksFilterDTO
            {
                Page = 2,
                RecordsPerPage = 3
            };

            // Act
            var result = await repository.GetUserFilter(userId, filter);

            // Assert
            Assert.AreEqual(3, result.Count());
        }
    }
}
