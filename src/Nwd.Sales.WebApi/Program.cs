using Nwd.Sales.Infrastructure.Extensions;
using Nwd.Sales.WebApi.Config;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up...");

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(ctx.Configuration));

// Setup Infrastructure
builder.Services.SetupInfrastructure(builder.Configuration);

// Setup ApplicationLayer
builder.Services.SetupApplicationLayer();

// Setup Swagger
builder.Services.SetupSwagger();

// Setup Controllers
builder.Services.SetupControllers();

// Setup FluentValidators
builder.Services.SetupFluentValidators();

// HttpContext
builder.Services.AddHttpContextAccessor();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.EnsureCosmosDbIsCreated();

    await app.SeedIfEmptyAsync();

    // Add OpenAPI/Swagger middlewares
    // Serves the registered OpenAPI/Swagger documents by default on `/swagger/{documentName}/swagger.json`
    app.UseOpenApi();

    // Serves the Swagger UI 3 web ui to view the OpenAPI/Swagger documents by default on `/swagger`
    app.UseSwaggerUi3();
}

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
    Log.Information("Starting app up.");
    app.Run();
    Log.Information("Shutting app down.");
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