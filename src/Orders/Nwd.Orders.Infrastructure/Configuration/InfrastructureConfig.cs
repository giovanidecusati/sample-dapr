using Microsoft.AspNetCore.Builder;
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

        public static async Task SeedIfEmptyAsync(this IApplicationBuilder app)
        {
            using (IServiceScope serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var customerRepository = serviceScope.ServiceProvider.GetService<ICustomerRepository>();
                var customer = await customerRepository.GetByIdAsync("3cb8ed37-10c3-4bf0-9905-194c03f40434");
                if (customer == null)
                {
                    customer = new Domain.Entities.Customer()
                    {
                        Email = "john.doe@email.com",
                        Id = "3cb8ed37-10c3-4bf0-9905-194c03f40434",
                        Name = "John Doe"
                    };

                    await customerRepository.UpdateAsync(customer);
                }
            }

        }
    }
}