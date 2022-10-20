using FluentValidation;
using MediatR;
using NSwag;
using NSwag.Generation.Processors.Security;
using Nwd.Sales.WebApi.Behaviors;
using System.Reflection;

namespace Nwd.Sales.WebApi.Config
{
    /// <summary>
    ///     Configure MVC options
    /// </summary>
    public static class MvcConfig
    {
        /// <summary>
        ///     Configure controllers
        /// </summary>
        /// <param name="services"></param>
        public static void SetupControllers(this IServiceCollection services)
        {
            // API controllers
            services.AddControllers();
        }

        public static void SetupFluentValidators(this IServiceCollection services)
        {
            // Add Validators
            services.AddValidatorsFromAssemblies(new[]
                    {
                        typeof(Application.Commands.OrderCommandHandler).Assembly,
                        typeof(Domain.Orders.OrderAgg).Assembly,
                    });
        }

        public static void SetupMediatR(this IServiceCollection services)
        {
            // Add Validators
            services.AddMediatR(typeof(Application.Commands.OrderCommandHandler).Assembly);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        }

        public static IServiceCollection SetupSwagger(this IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddOpenApiDocument(configure =>
            {
                configure.Title = "Nwd Sales API";
                configure.AddSecurity("JWT", Enumerable.Empty<string>(),
                    new OpenApiSecurityScheme
                    {
                        Type = OpenApiSecuritySchemeType.ApiKey,
                        Name = "Authorization",
                        In = OpenApiSecurityApiKeyLocation.Header,
                        Description = "Type into the textbox: Bearer {your JWT token}."
                    });

                configure.OperationProcessors.Add(
                    new AspNetCoreOperationSecurityScopeProcessor("JWT"));
            });

            return services;
        }
    }
}