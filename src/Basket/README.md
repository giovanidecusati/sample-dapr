# Basket


## Content
1. [Structure](#structure)
2. [Inventory microservice](#inventory-microservice)  
3. [Orders microservice](#orders-microservice-layers)  

## Structure
.  
├── .static :: *support files.*  
├── components :: *dapr components for local development.*  
├── infrastructure :: *Azure infrastructure.*  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;└── templates :: *individual bicep templates that are deployed.*  
├── pipelines :: *azure devops pipeline definitions and templates.*  
├── src :: *dotnet solution and source files.*  
└── README.md



Run Basket microservice. 
```
dapr run --app-id nwd-basket-api --components-path .\src\Basket\components\ --app-port 8003 --dapr-http-port 50001 -- dotnet run --project  .\src\Basket\Nwd.Basket.Api\Nwd.Basket.Api.csproj
```

```
dapr stop --app-id nwd-orders-api
dapr stop --app-id nwd-inventory-api
```