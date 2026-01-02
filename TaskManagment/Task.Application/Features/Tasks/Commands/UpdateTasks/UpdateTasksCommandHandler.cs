using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagment.Application.Contracts.Persistance;
using TaskManagment.Application.Contracts.Repositories;
using TaskManagment.Application.Exceptions;
using TaskManagment.Application.Utilities;

namespace TaskManagment.Application.Features.Tasks.Commands.UpdateTasks

{
    public class UpdateDentalOfficeCommandHandler : IRequestHandler<UpdateTasksCommand>
{
    private readonly ITasksRepository dentalOfficeRepository;
    private readonly IUnitOfWork unitofWork;

    public UpdateDentalOfficeCommandHandler(ITasksRepository dentalOfficeRepository, IUnitOfWork unitofWork)
    {
        this.dentalOfficeRepository = dentalOfficeRepository;
        this.unitofWork = unitofWork;
    }
    public async Task Handle(UpdateTasksCommand request)
    {
        var dentalOffice = await dentalOfficeRepository.GetById(request.Id);
        if (dentalOffice == null)
        {
            throw new NotFoundException();
        }
        dentalOffice.UpdateDetails(request.Title,request.Description,request.DueDate);
        try
        {
            await dentalOfficeRepository.Update(dentalOffice);
            await unitofWork.Commit();
        }
        catch (Exception)
        {
            await unitofWork.Rollback();
            throw;
        }
    }
}
}
