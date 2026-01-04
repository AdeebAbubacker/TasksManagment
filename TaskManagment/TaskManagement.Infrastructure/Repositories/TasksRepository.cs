using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Infrastructure.Utilities;
using TaskManagment.Application.Contracts.Repositories;
using TaskManagment.Application.Features.Tasks.Queries.GetMyTasksList;
using TaskManagment.Application.Features.Tasks.Queries.GetTasksList;
using TaskManagment.Domain.Entities;

namespace TaskManagement.Infrastructure.Repositories
{

    public class TasksRepository : Repository<TaskItem>, ITasksRepository
    {
        private readonly TaskManagmentDbContext context;

        public TasksRepository(TaskManagmentDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<TaskItem>> GetFiltered(TasksFilterDTO filter)
        {
            var queryable = context.TaskItems.AsQueryable();
            if (!string.IsNullOrWhiteSpace(filter.Title))
            {
                queryable = queryable.Where(x => x.Title.Contains(filter.Title));
            }

            return await queryable.OrderBy(x => x.Title).Paginate(filter.Page, filter.RecordsPerPage).ToListAsync();
        }


        public async Task<IEnumerable<TaskItem>> GetUserFilter(string ownerUserId, MyTasksFilterDTO filter)
        {
            var queryable = context.TaskItems.AsQueryable();
            queryable = queryable.Where(x => x.OwnerUserId == ownerUserId);

            if (!string.IsNullOrWhiteSpace(filter.Title))
                queryable = queryable.Where(x => x.Title.Contains(filter.Title));

            return await queryable
                .OrderBy(x => x.Title)
                .Paginate(filter.Page, filter.RecordsPerPage)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetUserTasks(
        string ownerUserId,
        MyTasksFilterDTO filter)
        {
            var queryable = context.TaskItems.AsQueryable();
            queryable = queryable.Where(x => x.OwnerUserId == ownerUserId);

            if (!string.IsNullOrWhiteSpace(filter.Title))
                queryable = queryable.Where(x => x.Title.Contains(filter.Title));

            return await queryable
                .OrderBy(x => x.Title)
                .Paginate(filter.Page, filter.RecordsPerPage)
                .ToListAsync();
        }
    }
}
