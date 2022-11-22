# Orders


1. Run Orders microservice
```
dapr run --app-id nwd-orders-api --components-path .\src\Orders\components\ --app-port 8003 --dapr-http-port 50003 -- dotnet run --project  .\src\Orders\Nwd.Orders.Api\Nwd.Orders.Api.csproj
```

2. Cleanup
```
dapr stop --app-id nwd-orders-api
```