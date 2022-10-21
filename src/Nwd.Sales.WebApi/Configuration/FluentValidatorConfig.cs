using FluentValidation;
using Nwd.Sales.Application.Commands.CreateOrder;

namespace Nwd.Sales.WebApi.Configuration
{
    public static class FluentValidatorConfig
    {
        public static void SetupFluentValidators(this IServiceCollection services)
        {
            // Add Validators
            services.AddValidatorsFromAssemblies(new[]
                    {
                        typeof(OrderCommandHandler).Assembly,
                        typeof(Domain.Orders.Order).Assembly,
                    });
        }
    }
}