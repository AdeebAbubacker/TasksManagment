using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Threading.Tasks;
using TaskManagment.Application.Contracts.Persistance;
using TaskManagment.Application.Contracts.Repositories;
using TaskManagment.Application.Features.Tasks.Commands.CreateTasks;
using TaskManagment.Domain.Entities;

namespace TaskManagment.Tests.Application.Features.Tasks
{
    [TestClass]
    public class CreateTasksCommandHandlerTests
    {
        private ITasksRepository repository;
        private IUnitOfWork unitOfWork;
        private CreateTasksCommandHandler handler;

        [TestInitialize]
        public void Setup()
        {
            repository = Substitute.For<ITasksRepository>();
            unitOfWork = Substitute.For<IUnitOfWork>();

            handler = new CreateTasksCommandHandler(repository, unitOfWork);
        }

        [TestMethod]
        public async Task Handle_ValidCommand_CreatesTaskAndCommits()
        {
            // Arrange
            var command = new CreateTasksCommand
            {
                Title = "New Task",
                Description = "Task description",
                DueDate = DateTime.UtcNow.AddDays(2),
                OwnerUserId = "user-123"
            };

            repository
                .Add(Arg.Any<TaskItem>())
                .Returns(callInfo => callInfo.Arg<TaskItem>());

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.AreNotEqual(Guid.Empty, result);

            await repository.Received(1).Add(Arg.Any<TaskItem>());
            await unitOfWork.Received(1).Commit();
            await unitOfWork.DidNotReceive().Rollback();
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task Handle_RepositoryThrows_RollbackIsCalled()
        {
            // Arrange
            var command = new CreateTasksCommand
            {
                Title = "Failing Task",
                Description = null,
                DueDate = null,
                OwnerUserId = "user-123"
            };

            repository
                .Add(Arg.Any<TaskItem>())
                .Returns<TaskItem>(_ => throw new Exception("DB error"));

            // Act
            await handler.Handle(command);

            // Assert (implicit due to ExpectedException)
            await unitOfWork.Received(1).Rollback();
            await unitOfWork.DidNotReceive().Commit();
        }
    }
}
