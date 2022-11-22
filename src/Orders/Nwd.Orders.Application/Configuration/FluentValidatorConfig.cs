using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Nwd.Orders.Application.Configuration
{
    public static class FluentValidatorConfig
    {
        public static void SetupFluentValidators(this IServiceCollection services)
        {
            // Add Validators
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}