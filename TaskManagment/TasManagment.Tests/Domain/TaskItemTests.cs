using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TaskManagemnt.Domain.Exceptions;
using TaskManagment.Domain.Entities;

namespace TaskManagment.Tests.Domain.Entities
{
    [TestClass]
    public class TaskItemTests
    {
        [TestMethod]
        public void Constructor_ValidData_CreatesTask()
        {
            // Act
            var task = new TaskItem(
                "Test Task",
                "Description",
                DateTime.UtcNow.AddDays(1),
                "user-123"
            );

            // Assert
            Assert.AreNotEqual(Guid.Empty, task.Id);
            Assert.AreEqual("Test Task", task.Title);
            Assert.AreEqual("Description", task.Description);
            Assert.IsFalse(task.IsCompleted);
            Assert.AreEqual("user-123", task.OwnerUserId);
            Assert.IsTrue(task.CreatedAt <= DateTime.UtcNow);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessRuleException))]
        public void Constructor_EmptyTitle_ThrowsException()
        {
            new TaskItem(
                "",
                null,
                null,
                "user-123"
            );
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessRuleException))]
        public void Constructor_EmptyOwner_ThrowsException()
        {
            new TaskItem(
                "Task",
                null,
                null,
                ""
            );
        }

        [TestMethod]
        public void UpdateDetails_ValidData_UpdatesTask()
        {
            // Arrange
            var task = new TaskItem(
                "Old Title",
                "Old Description",
                null,
                "user-123"
            );

            // Act
            task.UpdateDetails(
                "New Title",
                "New Description",
                DateTime.UtcNow.AddDays(5)
            );

            // Assert
            Assert.AreEqual("New Title", task.Title);
            Assert.AreEqual("New Description", task.Description);
            Assert.IsNotNull(task.DueDate);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessRuleException))]
        public void UpdateDetails_EmptyTitle_ThrowsException()
        {
            var task = new TaskItem(
                "Valid",
                null,
                null,
                "user-123"
            );

            task.UpdateDetails("", null, null);
        }

        [TestMethod]
        public void MarkAsCompleted_FirstTime_Succeeds()
        {
            // Arrange
            var task = new TaskItem(
                "Task",
                null,
                null,
                "user-123"
            );

            // Act
            task.MarkAsCompleted();

            // Assert
            Assert.IsTrue(task.IsCompleted);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessRuleException))]
        public void MarkAsCompleted_SecondTime_ThrowsException()
        {
            var task = new TaskItem(
                "Task",
                null,
                null,
                "user-123"
            );

            task.MarkAsCompleted();
            task.MarkAsCompleted();
        }
    }
}
