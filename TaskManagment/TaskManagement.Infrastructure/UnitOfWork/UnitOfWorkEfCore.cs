using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagment.Application.Contracts.Persistance;

namespace TaskManagement.Infrastructure.UnitOfWork
{
    public class UnitOfWorkEfCore : IUnitOfWork
    {
        private readonly TaskManagmentDbContext context;

        public UnitOfWorkEfCore(TaskManagmentDbContext context)
        {
            this.context = context;
        }
        public async Task Commit()
        {
            await context.SaveChangesAsync();
        }

        public Task Rollback()
        {
            return Task.CompletedTask;
        }
    }
}
