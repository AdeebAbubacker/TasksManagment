using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManagement.API.Controllers;
using TaskManagement.API.DTOs;
using TaskManagment.Application.Features.Tasks.Commands.CreateTasks;
using TaskManagment.Application.Features.Tasks.Commands.UpdateTasks;
using TaskManagment.Application.Features.Tasks.Commands.UpdateTasksStatus;
using TaskManagment.Application.Features.Tasks.Queries.GetMyTasksList;
using TaskManagment.Application.Features.Tasks.Queries.GetTasksList;
using TaskManagment.Application.Utilities;
using TaskManagment.Application.Utilities.Common;

namespace TaskManagement.Tests.API.Controllers
{
    [TestClass]
    public class TasksControllerTests
    {
        private IMediator mediator;
        private TasksController controller;

        [TestInitialize]
        public void Setup()
        {
            mediator = Substitute.For<IMediator>();
            controller = new TasksController(mediator);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
        }

        // ---------- HELPERS ----------
        private void SetUser(string userId, string role)
        {
            var claims = new List<Claim>
            {
                new Claim("userId", userId),
                new Claim(ClaimTypes.Role, role)
            };

            var identity = new ClaimsIdentity(claims, "TestAuth");
            controller.ControllerContext.HttpContext.User =
                new ClaimsPrincipal(identity);
        }

        // ---------- TESTS ----------

        [TestMethod]
        public async Task Post_UserAuthenticated_ReturnsOk()
        {
            // Arrange
            SetUser("user-123", "User");

            var dto = new CreateTaskDto
            {
                Title = "Task",
                Description = "Desc",
                DueDate = DateTime.UtcNow.AddDays(1)
            };

            mediator.Send(Arg.Any<CreateTasksCommand>())
                .Returns(Guid.NewGuid());

            // Act
            var result = await controller.Post(dto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            await mediator.Received(1).Send(Arg.Any<CreateTasksCommand>());
        }

        [TestMethod]
        public async Task Post_UserNotAuthenticated_ReturnsUnauthorized()
        {
            // Arrange
            var dto = new CreateTaskDto
            {
                Title = "Task"
            };

            // Act
            var result = await controller.Post(dto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));
        }

        [TestMethod]
        public async Task GetUserTasks_ReturnsTasks()
        {
            // Arrange
            SetUser("user-123", "User");

            var query = new GetMyTasksListQuery();

            var response = new PaginatedDTO<TasksListDTO>
            {
                Elements = new List<TasksListDTO>
            {
                new TasksListDTO { Title = "Task 1" }
            },
                TotalAMountOfRecords = 1
            };

            mediator.Send(query).Returns(response);

            // Act
            var result = await controller.GetUserTasks(query);

            // Assert
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(1, result.Value.Count);
            Assert.AreEqual("Task 1", result.Value[0].Title);
        }

        [TestMethod]
        public async Task Put_ValidRequest_ReturnsNoContent()
        {
            // Arrange
            SetUser("user-123", "User");

            var dto = new UpdateTasksDTO
            {
                Title = "Updated",
                Description = "Updated Desc",
                DueDate = null
            };

            // Act
            var result = await controller.Put(Guid.NewGuid(), dto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            await mediator.Received(1).Send(Arg.Any<UpdateTasksCommand>());
        }

        [TestMethod]
        public async Task UpdateTaskStatus_Admin_ReturnsNoContent()
        {
            // Arrange
            SetUser("admin-1", "Admin");

            var dto = new UpdateTasksStatusDTO
            {
                IsCompleted = true
            };

            // Act
            var result = await controller.UpdateTaskStatus(Guid.NewGuid(), dto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            await mediator.Received(1).Send(Arg.Any<UpdateTasksStatusCommand>());
        }
    }
}
