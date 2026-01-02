using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagment.Application.Contracts.Persistance;
using TaskManagment.Application.Contracts.Repositories;
using TaskManagment.Application.Utilities;
using TaskManagment.Domain.Entities;

namespace TaskManagment.Application.Features.Tasks.Commands.CreateTasks

{
    public class CreateTasksCommandHandler : IRequestHandler<CreateTasksCommand, Guid>
{
    private readonly ITasksRepository repository;
    private readonly IUnitOfWork unitOfWork;


    public CreateTasksCommandHandler(ITasksRepository dentalrepository, IUnitOfWork unitOfWork)
    {
        this.repository = dentalrepository;
        this.unitOfWork = unitOfWork;

    }
        public async Task<Guid> Handle(CreateTasksCommand command)
        {
            var taskItem = new TaskItem(
                command.Title,
                command.Description,
                command.DueDate,
                command.OwnerUserId
            );

            try
            {
                var result = await repository.Add(taskItem);
                await unitOfWork.Commit();
                return result.Id;
            }
            catch
            {
                await unitOfWork.Rollback();
                throw;
            }
        }

    }
}
