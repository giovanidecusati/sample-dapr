using Nwd.Sales.Infrastructure.Extensions;
using Nwd.Sales.WebApi.Config;

var builder = WebApplication.CreateBuilder(args);

// SetupInfrastructure
builder.Services.SetupInfrastructure(builder.Configuration);

// SetupApplicationLayer
builder.Services.SetupApplicationLayer();

// Swagger
builder.Services.SetupSwagger();

// API controllers
builder.Services.SetupControllers();

// HttpContext
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.EnsureCosmosDbIsCreated();

    await app.SeedIfEmptyAsync();
}

if (app.Environment.IsDevelopment())
{
    // Add OpenAPI/Swagger middlewares
    // Serves the registered OpenAPI/Swagger documents by default on `/swagger/{documentName}/swagger.json`
    app.UseOpenApi();

    // Serves the Swagger UI 3 web ui to view the OpenAPI/Swagger documents by default on `/swagger`
    app.UseSwaggerUi3();
}

app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseAuthentication();

app.UseRouting();

app.UseEndpoints(endpoints => endpoints.MapControllers());

app.Run();