using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Nwd.Inventory.Application.Queries.GetSingleCategory;
using Nwd.Inventory.Application.Queries.Repositories;
using Nws.BuildingBlocks.Events;
using System.Reflection;

namespace Nwd.Inventory.Application.Configuration
{
    public static class ApplicationConfig
    {
        public static void SetupApplication(this IServiceCollection services)
        {
            // MediatR from Application
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddMediatR(typeof(ProductCreatedEvent).Assembly);

            // Query Repositories
            services.AddScoped<IGetSingleCategoryReadOnlyRepository, CategoryReadOnlyRepository>();

        }
    }
}