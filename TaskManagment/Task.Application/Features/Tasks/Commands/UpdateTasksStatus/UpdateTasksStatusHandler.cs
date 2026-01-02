using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagemnt.Domain.Exceptions;
using TaskManagment.Application.Contracts.Persistance;
using TaskManagment.Application.Contracts.Repositories;
using TaskManagment.Application.Exceptions;
using TaskManagment.Application.Features.Tasks.Commands.UpdateTasks;
using TaskManagment.Application.Utilities;

namespace TaskManagment.Application.Features.Tasks.Commands.UpdateTasksStatus
{
    public class UpdateTasksStatusCommandHandler : IRequestHandler<UpdateTasksStatusCommand>
    {
        private readonly ITasksRepository repository;
        private readonly IUnitOfWork unitOfWork;

        public UpdateTasksStatusCommandHandler(ITasksRepository repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateTasksStatusCommand command)
        {
            var task = await repository.GetById(command.Id)
                ?? throw new NotFoundException();

            if (task.IsCompleted && command.IsCompleted)
            {
                throw new BusinessRuleException("Task is already completed.");
            }

            task.MarkAsCompleted(); // Task domain already handles business rules

            await unitOfWork.Commit();
        }
    }
}
