using MediatR;
using Nwd.Sales.WebApi.Behaviors;

namespace Nwd.Sales.WebApi.Configuration
{
    public static class MediatRConfig
    {

        public static void SetupMediatR(this IServiceCollection services)
        {
            // Add Handlers
            services.AddMediatR(typeof(Application.Commands.OrderCommandHandler).Assembly);

            // Add Behavior
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        }
    }
}