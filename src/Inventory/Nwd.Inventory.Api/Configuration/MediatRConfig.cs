using MediatR;
using System.Reflection;

namespace Nwd.Inventory.Api.Configuration
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