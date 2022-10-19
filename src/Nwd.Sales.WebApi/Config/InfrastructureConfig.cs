using Microsoft.AspNetCore.Identity;
using Nwd.Sales.Domain.Orders;
using Nwd.Sales.Infrastructure.Data.Configuration;
using Nwd.Sales.Infrastructure.Data.Repositories;
using Nwd.Sales.Infrastructure.Extensions;

namespace Nwd.Sales.WebApi.Config
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
        public static void SetupInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Bind database-related bindings
            CosmosDbSettings cosmosDbConfig = configuration.GetSection("ConnectionStrings:CosmosDB").Get<CosmosDbSettings>();
            // register CosmosDB client and data repositories
            services.AddCosmosDb(cosmosDbConfig.EndpointUrl,
                                 cosmosDbConfig.PrimaryKey,
                                 cosmosDbConfig.DatabaseName,
                                 cosmosDbConfig.Containers);

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();

            // Autommaper
            services.AddAutoMapper(typeof(ProductRepository).Assembly);
        }
    }
}