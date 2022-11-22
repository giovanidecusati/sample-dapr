using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Nwd.Orders.Application.Behaviors;
using System.Reflection;

namespace Nwd.Orders.Application.Configuration
{
    public static class MediatRConfig
    {
        public static void SetupMediatR(this IServiceCollection services)
        {
            // Add Handlers
            services.AddMediatR(Assembly.GetExecutingAssembly());

            // Add Behavior
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        }
    }
}