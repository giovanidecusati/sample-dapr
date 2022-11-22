# Inventory

### Inventory microservice layers 
* Web Api
  * HealthCheck
  * Monitorig
    * Serilog
  * Swagger
* Application Layer
  * CQRS
* Domain Layer
  * Domain-Driven Design
  * FluentValidator
* Data Layer
  * SQL Server

1. Run Inventory microservice
```
dapr run --app-id nwd-inventory-api --components-path .\src\Inventory\components\ --app-port 8002 --dapr-http-port 50002 -- dotnet run --project  .\src\Inventory\Nwd.Inventory.Api\Nwd.Inventory.Api.csproj
```

2. Cleanup
```
dapr stop --app-id nwd-basket-api
```