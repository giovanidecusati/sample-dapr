using NSwag.Generation.Processors.Security;
using NSwag;
using Serilog;

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