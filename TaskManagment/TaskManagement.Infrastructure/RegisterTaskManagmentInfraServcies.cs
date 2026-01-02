using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Infrastructure.Repositories;
using TaskManagement.Infrastructure.UnitOfWork;
using TaskManagment.Application.Contracts.Persistance;
using TaskManagment.Application.Contracts.Repositories;

namespace TaskManagement.Infrastructure
{

    public static class RegisterTaskManagmentInfraServcies
    {
        public static IServiceCollection AddInfrastrusturServices(this IServiceCollection services)
        {
            services.AddDbContext<TaskManagmentDbContext>(options =>
                options.UseSqlServer("Server=ADEEB\\SQLEXPRESS;Database=TaskManagmentDB;Trusted_Connection=True;TrustServerCertificate=True"));
            services.AddScoped<ITasksRepository, TasksRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWorkEfCore>();
            return services;
        }
    }

    public class TaskManagmentDbContextFactory
       : IDesignTimeDbContextFactory<TaskManagmentDbContext>
    {
        public TaskManagmentDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder =
                new DbContextOptionsBuilder<TaskManagmentDbContext>();

            optionsBuilder.UseSqlServer(
                "Server=ADEEB\\SQLEXPRESS;Database=TaskManagmentDB;Trusted_Connection=True;TrustServerCertificate=True");

            return new TaskManagmentDbContext(optionsBuilder.Options);
        }
    }
}
