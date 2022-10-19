﻿using FluentValidation;
using Microsoft.Azure.Cosmos;
using NSwag;
using NSwag.Generation.Processors.Security;
using Nwd.Sales.Application.Commands;
using Nwd.Sales.Application.Commands.CreateOrder;
using Nwd.Sales.Commands.CreateOrder;
using Nwd.Sales.Domain.Orders;
using Nwd.Sales.Infrastructure.Data;

namespace Nwd.Sales
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
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

            services.AddHttpContextAccessor();

            // services.AddScoped<ICurrentUser, CurrentUser>();

#if DEBUG
            //services.Configure<ServiceConfig>(config =>
            //{
            //    config.Services = new List<ServiceDescriptor>(services);
            //    config.Path = "/allservices";
            //});

            //services.AddLogging(configure
            //    => configure.AddSeq());
#endif
            return services;
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // CosmosClient
            services.AddTransient(sp =>
            {
                var configuration = sp.GetService<IConfiguration>();
                string account = configuration.GetSection("COSMOS_ENDPOINT").Value;
                string key = configuration.GetSection("COSMOS_KEY").Value;
                return new CosmosClient(account, key);
            });

            // ICustomerRepository
            services.AddTransient<ICustomerRepository>(sp =>
            {
                var cosmosClient = sp.GetService<CosmosClient>();
                var configuration = sp.GetService<IConfiguration>();
                string databaseName = configuration.GetSection("COSMOS_DATABASENAME").Value;
                string containerName = "customers";
                var database = cosmosClient.CreateDatabaseIfNotExistsAsync(databaseName).GetAwaiter().GetResult();
                var container = database.Database.CreateContainerIfNotExistsAsync(containerName, "/id").GetAwaiter().GetResult();
                return new CustomerRepository(container);
            });

            // IProductRepository
            services.AddTransient<IProductRepository>(sp =>
            {
                var cosmosClient = sp.GetService<CosmosClient>();
                var configuration = sp.GetService<IConfiguration>();
                string databaseName = configuration.GetSection("COSMOS_DATABASENAME").Value;
                string containerName = "products";
                var database = cosmosClient.CreateDatabaseIfNotExistsAsync(databaseName).GetAwaiter().GetResult();
                var container = database.Database.CreateContainerIfNotExistsAsync(containerName, "/id").GetAwaiter().GetResult();
                return new ProductRepository(container);
            });

            // IOrderRepository
            services.AddTransient<IOrderRepository>(sp =>
            {
                var cosmosClient = sp.GetService<CosmosClient>();
                var configuration = sp.GetService<IConfiguration>();
                string databaseName = configuration.GetSection("COSMOS_DATABASENAME").Value;
                string containerName = "orders";
                var database = cosmosClient.CreateDatabaseIfNotExistsAsync(databaseName).GetAwaiter().GetResult();
                var container = database.Database.CreateContainerIfNotExistsAsync(containerName, "/customerId").GetAwaiter().GetResult();
                return new OrderRepository(container);
            });

            return services;
        }

        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Validators            
            // :: Commands
            services.AddTransient<IValidator<CreateOrderCommand>, CreateOrderValidator>();
            
            // :: Domain :: Aggegates
            services.AddTransient<IValidator<OrderAgg>, OrderAggValidator>();

            // :: Domain :: Builder
            services.AddTransient<OrderAggBuilder>();

            // Command Handler
            services.AddTransient<OrderCommandHandler>();

            return services;
        }
    }

}