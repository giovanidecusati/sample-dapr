using MediatR;
using Nwd.Orders.Api.Behaviors;
using Nwd.Orders.Domain.Commands.CreateOrder;

namespace Nwd.Orders.Api.Configuration
{
    public static class MediatRConfig
    {

        public static void SetupMediatR(this IServiceCollection services)
        {
            // Add Handlers
            services.AddMediatR(typeof(CreateOrderCommandHandler).Assembly);

            // Add Behavior
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        }
    }
}