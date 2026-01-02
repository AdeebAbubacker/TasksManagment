using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagment.Application.Features.Tasks.Commands.UpdateTasks


{
    public class UpdateTasksCommandValidator : AbstractValidator<UpdateTasksCommand>
{
    public UpdateTasksCommandValidator()
    {
        RuleFor(p => p.Title).NotEmpty().WithMessage("The field {Title} is required");

    }
}
}
