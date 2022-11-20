namespace Nwd.Orders.Api.Configuration
{
    public static class SwaggerConfig
    {
        public static IServiceCollection SetupNSwag(this IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddOpenApiDocument(configure =>
            {
                configure.Title = "Northwind Sales API";
                configure.Description = "A simple ASP.NET Core web API";
                configure.Version = "v1";
                //configure.AddSecurity("JWT", Enumerable.Empty<string>(),
                //new OpenApiSecurityScheme
                //{
                //    Type = OpenApiSecuritySchemeType.ApiKey,
                //    Name = "Authorization",
                //    In = OpenApiSecurityApiKeyLocation.Header,
                //    Description = "Type into the textbox: Bearer {your JWT token}."
                //});

                //configure.OperationProcessors.Add(
                //    new AspNetCoreOperationSecurityScopeProcessor("JWT"));
            });

            return services;
        }
    }
}