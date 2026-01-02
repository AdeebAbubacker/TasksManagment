using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagment.Application.Utilities;

namespace TaskManagment.Application.Features.Tasks.Commands.UpdateTasks
{
    public class UpdateTasksCommand : IRequest
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }

        // Used to enforce "users can update only their own tasks"
        public string OwnerUserId { get; set; } = null!;
    }
}
