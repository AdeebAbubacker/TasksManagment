using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagment.Application.Features.Tasks.Commands.UpdateTasks;

namespace TaskManagment.Application.Features.Tasks.Commands.UpdateTasksStatus

{
    public class UpdateTasksStatusValidator : AbstractValidator<UpdateTasksCommand>
{
    public UpdateTasksStatusValidator()
    {
        RuleFor(p => p.Id).NotEmpty().WithMessage("The field {Id} is required");

    }
}
}
