using MediatR;
using Nwd.Sales.Application.Commands.CreateOrder;
using Nwd.Sales.Application.Queries.GetOrder;
using Nwd.Sales.WebApi.Behaviors;

namespace Nwd.Sales.WebApi.Configuration
{
    public static class MediatRConfig
    {

        public static void SetupMediatR(this IServiceCollection services)
        {
            // Add Handlers
            services.AddMediatR(typeof(OrderCommandHandler).Assembly, typeof(OrderQueryHandler).Assembly);

            // Add Behavior
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        }
    }
}