# Basket


1. Run Basket microservice
```
dapr run --app-id nwd-basket-api --components-path .\dapr\components --app-port 8001 --dapr-http-port 50001 -- dotnet run --project  .\src\Basket\Nwd.Basket.Api\Nwd.Basket.Api.csproj
```

2. Cleanup
```
dapr stop --app-id nwd-basket-api
```