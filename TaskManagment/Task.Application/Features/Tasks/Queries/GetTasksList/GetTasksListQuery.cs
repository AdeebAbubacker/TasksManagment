using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagment.Application.Utilities;
using TaskManagment.Application.Utilities.Common;

namespace TaskManagment.Application.Features.Tasks.Queries.GetTasksList

{
    public class GetTasksListQuery : TasksFilterDTO, IRequest<PaginatedDTO<TasksListDTO>>
{

}
}
