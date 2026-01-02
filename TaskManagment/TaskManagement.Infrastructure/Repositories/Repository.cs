using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagment.Application.Contracts.Repositories;

namespace TaskManagement.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly TaskManagmentDbContext context;

        public Repository(TaskManagmentDbContext context)
        {
            this.context = context;
        }
        public Task<T> Add(T entity)
        {
            context.Add(entity);
            return Task.FromResult(entity);
        }

        public Task Delete(T entity)
        {
            context.Remove(entity);
            return Task.FromResult(entity);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await context.Set<T>().ToListAsync();
        }

        public async Task<T?> GetById(Guid id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        public async Task<int> GetTotalAmpountofRecords()
        {
            return await context.Set<T>().CountAsync();
        }

        public Task Update(T entity)
        {
            context.Update(entity);
            return Task.CompletedTask;
        }
    }
}
