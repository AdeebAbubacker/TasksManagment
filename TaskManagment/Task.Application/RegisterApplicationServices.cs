using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagment.Application.Utilities;

namespace TaskManagment.Application
{
    public static class RegisterApplicationServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IMediator, SimpleMediator>();

            //scanning all classes in application layer and adding to di system instead of below manual method
            //for this we use package called Scrutor
            services.Scan(scan => scan.FromAssembliesOf(typeof(RegisterApplicationServices)).AddClasses(c => c.AssignableTo(
                typeof(IRequestHandler<>))).AsImplementedInterfaces().WithScopedLifetime().AddClasses(c => c.AssignableTo(
                    typeof(IRequestHandler<,>))).AsImplementedInterfaces().WithScopedLifetime());

            return services;
        }
    }
}
