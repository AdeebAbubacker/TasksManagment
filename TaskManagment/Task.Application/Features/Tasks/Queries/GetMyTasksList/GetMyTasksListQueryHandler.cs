using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagment.Application.Contracts.Repositories;
using TaskManagment.Application.Features.Tasks.Queries.GetTasksList;
using TaskManagment.Application.Utilities;
using TaskManagment.Application.Utilities.Common;

namespace TaskManagment.Application.Features.Tasks.Queries.GetMyTasksList
//{
//    internal class GetMyTasksListQueryHandler
//    {
//    }
//}


{
    public class GetMyTasksListQueryHandler : IRequestHandler<GetMyTasksListQuery, PaginatedDTO<TasksListDTO>>
{
    private readonly ITasksRepository tasksRepository;

    public GetMyTasksListQueryHandler(ITasksRepository tasksRepository)
    {
        this.tasksRepository = tasksRepository;
    }
    public async Task<PaginatedDTO<TasksListDTO>> Handle(GetMyTasksListQuery request)
    {

        var tasks = await tasksRepository.GetUserFilter(request);
        var totalAmountOfRecords = await tasksRepository.GetTotalAmpountofRecords();
        var taskDTO = tasks.Select(task => task.ToDto()).ToList();
        var paginatedDTO = new PaginatedDTO<TasksListDTO>
        {
            Elements = taskDTO,
            TotalAMountOfRecords = totalAmountOfRecords,
        };
        return paginatedDTO;
    }
}
}
