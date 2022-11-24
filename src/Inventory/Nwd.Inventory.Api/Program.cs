using Nwd.Inventory.Api.Configuration;
using Nwd.Inventory.Api.Services;
using Nwd.Inventory.Application.Configuration;
using Nwd.Inventory.Infrastructure.Configuration;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up...");

var builder = WebApplication.CreateBuilder(args);

// Serilog
builder.Host.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(ctx.Configuration));

// Setup Swagger
builder.Services.SetupNSwag();

// Setup Controllers
builder.Services.SetupControllers();

// HttpContext
builder.Services.AddHttpContextAccessor();

// HealthChecks
builder.Services.AddHealthChecks()
    .AddCheck<HealthCheck>("System");

// Setup Infrastructure
builder.Services.SetupInfrastructure();

// Setup Application
builder.Services.SetupApplication();

var app = builder.Build();

// TODO: replace Erro API Filter to global error handler ??
// app.UseMiddleware<ErrorHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    // Add OpenAPI/Swagger middlewares
    // Serves the registered OpenAPI/Swagger documents by default on `/swagger/{documentName}/swagger.json`
    app.UseOpenApi();

    // Serves the Swagger UI 3 web ui to view the OpenAPI/Swagger documents by default on `/swagger`
    app.UseSwaggerUi3();

    app.UseReDoc();
}

// UseRouting
app.UseRouting();

// UseDapr
app.UseDapr();

// MapAppHealthChecks
app.MapAppHealthChecks();

// UseSerilogRequestLogging
app.UseSerilogRequestLogging();

// UseCors
app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// UseHttpsRedirection
// Dapr Actors doesn't support UseHttpsRedirection  
//app.UseHttpsRedirection();

// app.UseAuthorization();

// app.UseAuthentication();

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