using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagment.Application.Features.Tasks.Queries.GetMyTasksList;
using TaskManagment.Application.Features.Tasks.Queries.GetTasksList;
using TaskManagment.Domain.Entities;

namespace TaskManagment.Application.Repositories


{
    public interface ITasksRepository : IRepository<TaskItem>
{
    Task<IEnumerable<TaskItem>> GetFiltered(TasksFilterDTO filter);

    Task<IEnumerable<TaskItem>> GetUserFilter(MyTasksFilterDTO filter);
    }
}

