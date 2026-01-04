using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Threading.Tasks;
using TaskManagment.Application.Contracts.Persistance;
using TaskManagment.Application.Contracts.Repositories;
using TaskManagment.Application.Exceptions;
using TaskManagment.Application.Features.Tasks.Commands.UpdateTasks;
using TaskManagment.Domain.Entities;

namespace TaskManagment.Tests.Application.Features.Tasks.Commands
{
    [TestClass]
    public class UpdateTasksCommandHandlerTests
    {
        private ITasksRepository repository;
        private IUnitOfWork unitOfWork;
        private UpdateDentalOfficeCommandHandler handler;

        [TestInitialize]
        public void Setup()
        {
            repository = Substitute.For<ITasksRepository>();
            unitOfWork = Substitute.For<IUnitOfWork>();

            handler = new UpdateDentalOfficeCommandHandler(
                repository,
                unitOfWork
            );
        }

        [TestMethod]
        public async Task Handle_TaskExists_UpdatesAndCommits()
        {
            // Arrange
            var task = new TaskItem(
                "Old Title",
                "Old Description",
                null,
                "user-123"
            );

            var command = new UpdateTasksCommand
            {
                Id = task.Id,
                Title = "New Title",
                Description = "New Description",
                DueDate = DateTime.UtcNow.AddDays(2)
            };

            repository.GetById(task.Id).Returns(task);

            // Act
            await handler.Handle(command);

            // Assert
            Assert.AreEqual("New Title", task.Title);
            Assert.AreEqual("New Description", task.Description);

            await repository.Received(1).Update(task);
            await unitOfWork.Received(1).Commit();
            await unitOfWork.DidNotReceive().Rollback();
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task Handle_TaskDoesNotExist_ThrowsNotFound()
        {
            // Arrange
            var command = new UpdateTasksCommand
            {
                Id = Guid.NewGuid(),
                Title = "Title",
                Description = null,
                DueDate = null
            };

            repository.GetById(command.Id).Returns((TaskItem?)null);

            // Act
            await handler.Handle(command);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task Handle_UpdateFails_RollbackIsCalled()
        {
            // Arrange
            var task = new TaskItem(
                "Old Title",
                null,
                null,
                "user-123"
            );

            var command = new UpdateTasksCommand
            {
                Id = task.Id,
                Title = "Updated",
                Description = null,
                DueDate = null
            };

            repository.GetById(task.Id).Returns(task);

            repository
                .When(x => x.Update(task))
                .Do(_ => throw new Exception("DB error"));

            // Act
            await handler.Handle(command);

            // Assert (verified by ExpectedException)
            await unitOfWork.Received(1).Rollback();
            await unitOfWork.DidNotReceive().Commit();
        }

    }
}
