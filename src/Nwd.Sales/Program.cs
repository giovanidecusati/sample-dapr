using Nwd.Sales;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwagger();
builder.Services.AddInfrastructure();
builder.Services.AddApplication();

builder.Configuration.AddUserSecrets<Program>();

// Add services to the container.
builder.Services.AddControllers();

var app = builder.Build();


// Add OpenAPI/Swagger middlewares
app.UseOpenApi(); // Serves the registered OpenAPI/Swagger documents by default on `/swagger/{documentName}/swagger.json`
app.UseSwaggerUi3(); // Serves the Swagger UI 3 web ui to view the OpenAPI/Swagger documents by default on `/swagger`

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
