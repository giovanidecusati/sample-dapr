using FluentValidation;

namespace Nwd.Sales.WebApi.Configuration
{
    public static class FluentValidatorConfig
    {
        public static void SetupFluentValidators(this IServiceCollection services)
        {
            // Add Validators
            services.AddValidatorsFromAssemblies(new[]
                    {
                        typeof(Application.Commands.OrderCommandHandler).Assembly,
                        typeof(Domain.Orders.OrderAgg).Assembly,
                    });
        }
    }
}