using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using TaskManagment.Domain.Entities;

namespace TaskManagement.Infrastructure
{

    public class TaskManagmentDbContext : DbContext
    {
        public TaskManagmentDbContext(DbContextOptions options) : base(options)
        {
        }

        protected TaskManagmentDbContext()
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TaskManagmentDbContext).Assembly);
        }

        public DbSet<TaskItem> TaskItems { get; set; }

    }
}
