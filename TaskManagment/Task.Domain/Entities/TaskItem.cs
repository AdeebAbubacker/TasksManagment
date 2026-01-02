using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TaskManagemnt.Domain.Exceptions;

namespace TaskManagment.Domain.Entities
{
    public class TaskItem
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; } = null!;
        public string? Description { get; private set; }
        public bool IsCompleted { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? DueDate { get; private set; }
        public string OwnerUserId { get; private set; } = null!;

        // EF Core requirement
        private TaskItem() { }

        public TaskItem(
            string title,
            string? description,
            DateTime? dueDate,
            string ownerUserId)
        {
            EnforceTitleBusinessRule(title);
            EnforceOwnerBusinessRule(ownerUserId);

            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            DueDate = dueDate;
            OwnerUserId = ownerUserId;

            CreatedAt = DateTime.UtcNow;
            IsCompleted = false;
        }

        public void UpdateDetails(string title, string? description, DateTime? dueDate)
        {
            EnforceTitleBusinessRule(title);

            Title = title;
            Description = description;
            DueDate = dueDate;
        }

        public void MarkAsCompleted()
        {
            if (IsCompleted)
            {
                throw new BusinessRuleException("Task is already completed.");
            }

            IsCompleted = true;
        }

        private void EnforceTitleBusinessRule(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new BusinessRuleException("Task title cannot be empty.");
            }
        }

        private void EnforceOwnerBusinessRule(string ownerUserId)
        {
            if (string.IsNullOrWhiteSpace(ownerUserId))
            {
                throw new BusinessRuleException("Task must have an owner.");
            }
        }
    }
}