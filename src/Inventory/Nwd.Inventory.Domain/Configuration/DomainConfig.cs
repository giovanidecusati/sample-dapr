using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Nwd.Inventory.Domain.Configuration
{
    public static class DomainConfig
    {
        public static void SetupDomain(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
        }
    }
}