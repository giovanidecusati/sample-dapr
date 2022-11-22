using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Nwd.Inventory.Application.Configuration
{
    public static class MediatRConfig
    {
        public static void SetupMediatR(this IServiceCollection services)
        {
            // Add Handlers
            services.AddMediatR(Assembly.GetExecutingAssembly());
        }
    }
}