using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManagment.Application.Contracts.Repositories;
using TaskManagment.Application.Features.Tasks.Queries.GetMyTasksList;
using TaskManagment.Application.Utilities.Common;
using TaskManagment.Domain.Entities;

namespace TaskManagment.Tests.Application.Features.Tasks
{
    [TestClass]
    public class GetMyTasksListQueryHandlerTests
    {
        private ITasksRepository tasksRepository;
        private IHttpContextAccessor httpContextAccessor;
        private GetMyTasksListQueryHandler handler;

        [TestInitialize]
        public void Setup()
        {
            tasksRepository = Substitute.For<ITasksRepository>();
            httpContextAccessor = Substitute.For<IHttpContextAccessor>();

            handler = new GetMyTasksListQueryHandler(
                tasksRepository,
                httpContextAccessor
            );
        }

        [TestMethod]
        public async Task Handle_UserHasTasks_ReturnsPaginatedTasks()
        {
            // Arrange
            var userId = "user-123";

            var claims = new List<Claim>
            {
                new Claim("userId", userId)
            };

            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);

            var httpContext = new DefaultHttpContext
            {
                User = principal
            };

            httpContextAccessor.HttpContext.Returns(httpContext);

            var query = new GetMyTasksListQuery
            {
                Page = 1,
                RecordsPerPage = 10
            };

            var tasks = new List<TaskItem>
            {
                new TaskItem("Task 1", "Desc 1", null, userId),
                new TaskItem("Task 2", null, DateTime.UtcNow.AddDays(1), userId)
            };

            tasksRepository
                .GetUserFilter(userId, query)
                .Returns(tasks);

            // Act
            var result = await handler.Handle(query);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.TotalAMountOfRecords);
            Assert.AreEqual(2, result.Elements.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public async Task Handle_UserIdMissingInToken_ThrowsUnauthorized()
        {
            // Arrange
            var httpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity())
            };

            httpContextAccessor.HttpContext.Returns(httpContext);

            var query = new GetMyTasksListQuery
            {
                Page = 1,
                RecordsPerPage = 10
            };

            // Act
            await handler.Handle(query);
        }
    }
}
