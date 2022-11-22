# Inventory


1. Run Inventory microservice
```
dapr run --app-id nwd-inventory-api --components-path .\src\Inventory\components\ --app-port 8002 --dapr-http-port 50002 -- dotnet run --project  .\src\Inventory\Nwd.Inventory.Api\Nwd.Inventory.Api.csproj
```

2. Cleanup
```
dapr stop --app-id nwd-basket-api
```