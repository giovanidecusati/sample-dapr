using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Nwd.Sales.Infrastructure.Configuration;
using Nwd.Sales.Infrastructure.Data.Configuration;
using Nwd.Sales.Infrastructure.Extensions;
using Nwd.Sales.WebApi.Configuration;
using Nwd.Sales.WebApi.Services;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up...");

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(ctx.Configuration));

// Setup Infrastructure
builder.Services.SetupInfrastructure(builder.Configuration.GetSection("ConnectionStrings:CosmosDB").Get<CosmosDbSettings>());

// Setup ApplicationLayer
builder.Services.SetupApplicationLayer();

// Setup Swagger
builder.Services.SetupNSwag();

// Setup Controllers
builder.Services.SetupControllers();

// Setup FluentValidators
builder.Services.SetupFluentValidators();

// Setup MediatR
builder.Services.SetupMediatR();

// HttpContext
builder.Services.AddHttpContextAccessor();

// HealthChecks
builder.Services.AddHealthChecks()
    .AddCheck<HealthCheck>("System");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.EnsureCosmosDbIsCreated();

    await app.SeedIfEmptyAsync();

    // Add OpenAPI/Swagger middlewares
    // Serves the registered OpenAPI/Swagger documents by default on `/swagger/{documentName}/swagger.json`
    app.UseOpenApi();

    // Serves the Swagger UI 3 web ui to view the OpenAPI/Swagger documents by default on `/swagger`
    app.UseSwaggerUi3();

    app.UseReDoc();
}

// MapHealthChecks
var healthCheckOptions = new HealthCheckOptions();
healthCheckOptions.ResponseWriter = async (c, r) =>
{
    c.Response.ContentType = "application/json";
    var result = JsonConvert.SerializeObject(r);
    await c.Response.WriteAsync(result);
};

app.MapHealthChecks("/health", healthCheckOptions);

// UseSerilogRequestLogging
app.UseSerilogRequestLogging();

app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseHttpsRedirection();

// app.UseAuthorization();

// app.UseAuthentication();

app.UseRouting();

app.UseEndpoints(endpoints => endpoints.MapControllers());

Log.Information("Middleware configuration completed.");

try
{
    Log.Information("Starting up.");
    app.Run();
    Log.Information("Shutting down.");
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly.");
}
finally
{
    Log.Information("Shutdown completed.");
    Log.CloseAndFlush();
}