using Microsoft.Extensions.DependencyInjection;
using Nwd.Orders.Domain.Interfaces;
using Nwd.Orders.Infrastructure.Data.Configuration;
using Nwd.Orders.Infrastructure.Data.Repositories;
using Nwd.Orders.Infrastructure.Extensions;

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
        public static void SetupInfrastructure(this IServiceCollection services, CosmosDbSettings cosmosDbConfig)
        {
            // register CosmosDB client and data repositories
            services.AddCosmosDb(cosmosDbConfig.EndpointUrl,
                                 cosmosDbConfig.PrimaryKey,
                                 cosmosDbConfig.DatabaseName,
                                 cosmosDbConfig.Containers);

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
        }
    }
}