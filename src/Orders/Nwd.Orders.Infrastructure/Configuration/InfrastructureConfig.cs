using Microsoft.Extensions.DependencyInjection;
using Nwd.Orders.Domain.Interfaces;
using Nwd.Orders.Infrastructure.Data.Repositories;

namespace Nwd.Orders.Infrastructure.Configuration
{
    /// <summary>
    ///     Database related configurations
    /// </summary>
    public static class InfrastructureConfig
    {
        /// <summary>
        ///     Setup Cosmos DB
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void SetupInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
        }
    }
}