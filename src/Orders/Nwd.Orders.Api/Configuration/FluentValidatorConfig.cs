using FluentValidation;
using Nwd.Orders.Domain.Commands.CreateOrder;

namespace Nwd.Orders.Api.Configuration
{
    public static class FluentValidatorConfig
    {
        public static void SetupFluentValidators(this IServiceCollection services)
        {
            // Add Validators
            services.AddValidatorsFromAssembly(typeof(CreateOrderCommandHandler).Assembly);
        }
    }
}