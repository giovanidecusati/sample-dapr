using Nwd.Orders.Application.Configuration;
using Nwd.Orders.Application.Queries.GetSingleOrder;
using Nwd.Orders.Application.Queries.ListOrder;
using Nwd.Orders.Application.Queries.Repositories;

namespace Nwd.Orders.Api.Configuration
{
    public static class ApplicationConfig
    {
        public static void SetupApplicationConfig(this IServiceCollection services)
        {
            // Add Validators
            services.SetupFluentValidators();

            // MediatR
            services.SetupMediatR();

            // ReadOnly Repositories
            services.AddScoped<IOrderReadOnlyRepository, OrderReadOnlyRepository>();
            services.AddScoped<IListOrderReadOnlyRepository, OrderReadOnlyRepository>();
        }
    }
}