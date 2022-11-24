using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Nwd.Orders.Application.Behaviors;
using Nwd.Orders.Application.Configuration;
using Nwd.Orders.Application.Queries.GetSingleOrder;
using Nwd.Orders.Application.Queries.ListOrder;
using Nwd.Orders.Application.Queries.Repositories;
using Nws.BuildingBlocks.Events;
using System.Reflection;

namespace Nwd.Orders.Application.Configuration
{
    public static class ApplicationConfig
    {
        public static void SetupApplication(this IServiceCollection services)
        {
            // Add Validators
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // MediatR :: Add Handlers
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddMediatR(typeof(ProductCreatedEvent).Assembly);

            // MediatR :: Add Behavior
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

            // ReadOnly Repositories
            services.AddScoped<IOrderReadOnlyRepository, OrderReadOnlyRepository>();
            services.AddScoped<IListOrderReadOnlyRepository, OrderReadOnlyRepository>();
        }
    }
}