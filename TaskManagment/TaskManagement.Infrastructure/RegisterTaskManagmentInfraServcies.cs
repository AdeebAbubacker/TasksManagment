using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Infrastructure.Repositories;
using TaskManagement.Infrastructure.UnitOfWork;
using TaskManagment.Application.Contracts.Persistance;
using TaskManagment.Application.Contracts.Repositories;

namespace TaskManagement.Infrastructure
{
    public static class RegisterTaskManagmentInfraServcies
    {
        public static IServiceCollection AddInfrastrusturServices(
            this IServiceCollection services)
        {
            services.AddDbContext<TaskManagmentDbContext>(options =>
                options.UseInMemoryDatabase("TaskManagementDb"));

            services.AddScoped<ITasksRepository, TasksRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWorkEfCore>();

            return services;
        }
    }
}
