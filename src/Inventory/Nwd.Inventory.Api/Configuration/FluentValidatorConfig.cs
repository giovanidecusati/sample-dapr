namespace Nwd.Inventory.Api.Configuration
{
    public static class FluentValidatorConfig
    {
        public static void SetupFluentValidators(this IServiceCollection services)
        {
            // Add Validators
            // services.AddValidatorsFromAssembly(typeof(CreateOrderCommandHandler).Assembly);
        }
    }
}