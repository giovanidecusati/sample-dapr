# Orders

### Orders microservice layers 
* Web Api
  * HealthCheck
  * Monitorig
    * Serilog
  * Swagger
* Domain Layer
  * CQRS using MediaT
  * FluentValidator
* Data Layer
  * CosmosDB
  
1. Run Orders microservice
```
dapr run --app-id nwd-orders-api --config .\dapr\config\nwd-config.yml --components-path .\dapr\components --app-port 8003 --dapr-http-port 50003 -- dotnet run --project  .\src\Orders\Nwd.Orders.Api\Nwd.Orders.Api.csproj
dapr run --app-id nwd-orders-api --components-path .\dapr\components --app-port 8003 --dapr-http-port 50003 -- dotnet run --project  .\src\Orders\Nwd.Orders.Api\Nwd.Orders.Api.csproj
```

2. Cleanup
```
dapr stop --app-id nwd-orders-api
```