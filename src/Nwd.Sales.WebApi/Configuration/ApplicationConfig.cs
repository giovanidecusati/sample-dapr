using Nwd.Sales.Domain.Orders;

namespace Nwd.Sales.WebApi.Configuration
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
            // :: Domain :: Builder
            services.AddTransient<OrderAggBuilder>();
        }
    }
}