using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagment.Application.Utilities;

namespace TaskManagment.Application.Features.Tasks.Commands.CreateTasks
{
    public class CreateTasksCommand : IRequest<Guid>
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }

        public required string OwnerUserId { get; set; }
    }
}
