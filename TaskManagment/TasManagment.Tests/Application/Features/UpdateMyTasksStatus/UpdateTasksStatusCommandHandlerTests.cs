using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Threading.Tasks;
using TaskManagemnt.Domain.Exceptions;
using TaskManagment.Application.Contracts.Persistance;
using TaskManagment.Application.Contracts.Repositories;
using TaskManagment.Application.Exceptions;
using TaskManagment.Application.Features.Tasks.Commands.UpdateTasksStatus;
using TaskManagment.Domain.Entities;

namespace TaskManagment.Tests.Application.Features.Tasks.Commands
{
    [TestClass]
    public class UpdateTasksStatusCommandHandlerTests
    {
        private ITasksRepository repository;
        private IUnitOfWork unitOfWork;
        private UpdateTasksStatusCommandHandler handler;

        [TestInitialize]
        public void Setup()
        {
            repository = Substitute.For<ITasksRepository>();
            unitOfWork = Substitute.For<IUnitOfWork>();

            handler = new UpdateTasksStatusCommandHandler(
                repository,
                unitOfWork
            );
        }

        [TestMethod]
        public async Task Handle_TaskNotCompleted_MarksAsCompletedAndCommits()
        {
            // Arrange
            var task = new TaskItem(
                "Task Title",
                "Description",
                null,
                "user-123"
            );

            var command = new UpdateTasksStatusCommand
            {
                Id = task.Id,
                IsCompleted = true
            };

            repository.GetById(task.Id).Returns(task);

            // Act
            await handler.Handle(command);

            // Assert
            Assert.IsTrue(task.IsCompleted);
            await unitOfWork.Received(1).Commit();
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task Handle_TaskDoesNotExist_ThrowsNotFound()
        {
            // Arrange
            var command = new UpdateTasksStatusCommand
            {
                Id = Guid.NewGuid(),
                IsCompleted = true
            };

            repository.GetById(command.Id).Returns((TaskItem?)null);

            // Act
            await handler.Handle(command);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessRuleException))]
        public async Task Handle_TaskAlreadyCompleted_ThrowsBusinessRuleException()
        {
            // Arrange
            var task = new TaskItem(
                "Completed Task",
                null,
                null,
                "user-123"
            );

            // Mark task as completed first
            task.MarkAsCompleted();

            var command = new UpdateTasksStatusCommand
            {
                Id = task.Id,
                IsCompleted = true
            };

            repository.GetById(task.Id).Returns(task);

            // Act
            await handler.Handle(command);
        }
    }
}
