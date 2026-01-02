using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagment.Application.Features.Tasks.Queries.GetTasksList
{
    public class TasksFilterDTO
    {
        public int Page { get; set; } = 1;
        public int RecordsPerPage { get; set; } = 10;
        public string? Title { get; set; } = null;

        public string? OwnerUserId { get; set; } = null;

    }
}
