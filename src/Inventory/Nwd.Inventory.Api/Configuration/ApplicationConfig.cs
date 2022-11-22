using MediatR;
using Nwd.Inventory.Application.Configuration;
using Nwd.Inventory.Application.Queries.GetSingleCategory;
using Nwd.Inventory.Application.Queries.Repositories;
using System.Reflection;

namespace Nwd.Inventory.Api.Configuration
{
    public static class ApplicationConfig
    {
        public static void SetupApplication(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddScoped<IGetSingleCategoryReadOnlyRepository, CategoryReadOnlyRepository>();

            // Setup EventHandlers
            services.SetupMediatR();
        }
    }
}