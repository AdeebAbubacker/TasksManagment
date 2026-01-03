using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagment.Application.Contracts.Repositories;
using TaskManagment.Application.Features.Tasks.Queries.GetTasksList;
using TaskManagment.Application.Utilities;
using TaskManagment.Application.Utilities.Common;
using Microsoft.AspNetCore.Http;

namespace TaskManagment.Application.Features.Tasks.Queries.GetMyTasksList


{
    public class GetMyTasksListQueryHandler : IRequestHandler<GetMyTasksListQuery, PaginatedDTO<TasksListDTO>>
{
        private readonly ITasksRepository tasksRepository;
        private readonly IHttpContextAccessor httpContextAccessor;

        public GetMyTasksListQueryHandler(ITasksRepository tasksRepository, IHttpContextAccessor httpContextAccessor)
        {
            this.tasksRepository = tasksRepository;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<PaginatedDTO<TasksListDTO>> Handle(
        GetMyTasksListQuery request
        )
        {
            // ✅ 1. Read userId from JWT (custom claim)
            var userId = httpContextAccessor.HttpContext?
                .User.FindFirst("userId")?.Value;

            // ✅ 2. Security check
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("UserId not found in token");

            // ✅ 4. Query only user's tasks
            var tasks = await tasksRepository.GetUserFilter(userId, request);

            // ⚠️ OPTIONAL: this should ideally count only USER tasks
            var totalAmountOfRecords = tasks.Count();

            var taskDTO = tasks.Select(task => task.ToDto()).ToList();

            return new PaginatedDTO<TasksListDTO>
            {
                Elements = taskDTO,
                TotalAMountOfRecords = totalAmountOfRecords
            };
        }
    }
}
