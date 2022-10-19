using Nwd.Sales.Infrastructure.Extensions;
using Nwd.Sales.WebApi.Config;

var builder = WebApplication.CreateBuilder(args);

// Cosmos DB for application data
builder.Services.SetupInfrastructure(builder.Configuration);

// Swagger
builder.Services.SetupSwagger();

// SetupApplicationLayer
builder.Services.SetupApplicationLayer();

// API controllers
builder.Services.SetupControllers();

// HttpContext
builder.Services.AddHttpContextAccessor();

// TODO: Only in Developement
builder.Configuration.AddUserSecrets<Program>();

var app = builder.Build();

// TODO: Only in Developement
app.EnsureCosmosDbIsCreated();

await app.SeedIfEmptyAsync();

// Add OpenAPI/Swagger middlewares
// Serves the registered OpenAPI/Swagger documents by default on `/swagger/{documentName}/swagger.json`
app.UseOpenApi();

// Serves the Swagger UI 3 web ui to view the OpenAPI/Swagger documents by default on `/swagger`
app.UseSwaggerUi3();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();