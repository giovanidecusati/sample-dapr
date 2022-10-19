using NSwag.Generation.Processors.Security;
using NSwag;
using FluentValidation;
using Nwd.Sales.Application.Commands.CreateOrder;
using Nwd.Sales.Application.Commands;
using Nwd.Sales.Commands.CreateOrder;
using Nwd.Sales.Domain.Orders;

namespace Nwd.Sales.WebApi.Config
{
    /// <summary>
    ///     Configure ApplicationConfig options
    /// </summary>
    public static class ApplicationConfig
    {
        /// <summary>
        ///     Configure controllers
        /// </summary>
        /// <param name="services"></param>
        public static void SetupApplicationLayer(this IServiceCollection services)
        {
            // Validators            
            // :: Commands
            services.AddTransient<IValidator<CreateOrderCommand>, CreateOrderValidator>();

            // :: Domain :: Aggegates
            services.AddTransient<IValidator<OrderAgg>, OrderAggValidator>();

            // :: Domain :: Builder
            services.AddTransient<OrderAggBuilder>();

            // Command Handler
            services.AddTransient<OrderCommandHandler>();
        }
    }
}