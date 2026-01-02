using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagment.Application.Utilities;

namespace TaskManagment.Application.Features.Tasks.Commands.UpdateTasksStatus

{
    public class UpdateTasksStatusCommand : IRequest
    {
        public Guid Id { get; set; }

        public bool IsCompleted { get; set; }
    }
}

