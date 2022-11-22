using Microsoft.Extensions.DependencyInjection;
using Nwd.Inventory.Domain.Repositories;
using Nwd.Inventory.Infrastructure.Data.Repositories;

namespace Nwd.Inventory.Infrastructure.Configuration
{
    public static class InfrastructureConfig
    {
        public static void SetupInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IInventoryRepository, InventoryRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
        }
    }
}