using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Nwd.Sales.Domain.Orders;
using Nwd.Sales.Infrastructure.Data.Interfaces;

namespace Nwd.Sales.Infrastructure.Extensions
{
    /// <summary>
    ///     Extension methods for IApplicationBuilder 
    /// </summary>
    public static class IApplicationBuilderExtensions
    {
        /// <summary>
        ///     Ensure Cosmos DB is created
        /// </summary>
        /// <param name="builder"></param>
        public static void EnsureCosmosDbIsCreated(this IApplicationBuilder builder)
        {
            using (IServiceScope serviceScope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                ICosmosDbContainerFactory factory = serviceScope.ServiceProvider.GetService<ICosmosDbContainerFactory>();

                factory.EnsureDbSetupAsync().Wait();
            }
        }

        /// <summary>
        ///     Seed sample data in the Todo container
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static async Task SeedIfEmptyAsync(this IApplicationBuilder builder)
        {
            using (IServiceScope serviceScope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var productRepository = serviceScope.ServiceProvider.GetService<IProductRepository>();
                await productRepository.AddAsync(new Product("Dell XPS 15", 3599.89m, "Laptop"));

                var customerRepository = serviceScope.ServiceProvider.GetService<ICustomerRepository>();
                await customerRepository.AddAsync(new Customer("John Doe", "john.doe@nwdsales.ie"));
            }
        }
    }
}