
* Domain-Driven Design
* CQRS using MediatR
* FluentValidator
* HealthCheck
* Monitorig
  * Serilog


## Tye
* https://github.com/dotnet/tye/blob/main/docs/getting_started.md

## Dapr

1. Dashboard
```
$\src\>dapr dashboard
```

2. Order API
```
$\src> dapr run --app-id nwd-orders-api --components-path .\components\ --app-port 8001 --dapr-http-port 50001 -- dotnet run --project  .\Nwd.Orders.Api\Nwd.Orders.Api.csproj
```

3. Pub/Sub
```
Invoke-WebRequest 'http://localhost:50001/v1.0/invoke/nwd-orders-api/method/api/product'
```