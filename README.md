# Nwd
Nwd stands for Northwind :stuck_out_tongue_winking_eye: This solution's idea is to exercise a few concepts using modern tools, frameworks and libraries. I'm using microservices approach. Each microservice has a different pattern to solve different problems.

Why **Dapr**? It fits this architecture because it solves an essential part of configuring services, such as queue topics, to support publish/subscriber patterns. It avoids a lot of boilerplate code.

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

## Basket microservice
It handles basket operations.
* It simulates a Data-Driven scenario. 

## Inventory microservice
It handles product and inventory operations.
* It simulates a very complext domain. 
* Pusblishes a *ProductCreatedEvent* into a topic *new-product-topic* for every new product created.
* Uses isolation level to deal with concurrency transactions due to stock level operations.

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


## Orders microservice
It handle order operations.
* It simulates a CRUD. 
* Subscribes a topic *new-product-topic*.

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

## Development
Follow the steps below to setup you development enviroment.

### Pre-Requirements
* Install Docker
  * [Install Docker](https://docs.docker.com/desktop/install/windows-install/)
  * Switch to Linux containers.
* Install Azure Cosmos DB Emulator.
  * [Install and use the Azure Cosmos DB Emulator for local development and testing](https://learn.microsoft.com/en-us/azure/cosmos-db/local-emulator?tabs=ssl-netstd21)
* Install Dapr-Cli.
  * [Install the Dapr CLI](https://docs.dapr.io/getting-started/install-dapr-cli/)
  ```
  $Env:DAPR_INSTALL_DIR = "C:\Users\<user>\.dapr\bin"
  powershell -Command "iwr -useb https://raw.githubusercontent.com/dapr/cli/master/install/install.ps1 | iex"
  ```

### Let's have fun
Open your console.
1. Ensure you have started Azure CosmosDB Emulator.
2. Add the following secrets to *Nwd.Orders.Api*:
```
{
  "ConnectionStrings": {
    "CosmosDB": {
      "EndpointUrl": "https://localhost:8081/",
      "PrimaryKey": "<your local cosmos DB Primary key>"
    }
  }
}
```

3. Run the init CLI command.
```
dapr init
```

2. Start Dapr dashboard in a new console.
```
dapr dashboard
```

3. Run Basket microservice. 
```
dapr run --app-id nwd-basket-api --components-path .\src\.components\ --app-port 8003 --dapr-http-port 50001 -- dotnet run --project  .\src\Nwd.Basket.Api\Nwd.Basket.Api.csproj
```

3. Run Orders microservice. 
```
dapr run --app-id nwd-orders-api --components-path .\src\.components\ --app-port 8001 --dapr-http-port 50001 -- dotnet run --project  .\src\Nwd.Orders.Api\Nwd.Orders.Api.csproj
```

4. Run Inventory microservice. 
```
dapr run --app-id nwd-inventory-api --components-path .\src\.components\ --app-port 8002 --dapr-http-port 50001 -- dotnet run --project  .\src\Nwd.Orders.Api\Nwd.Orders.Api.csproj
```

6. Cleanup

```
dapr stop --app-id nwd-orders-api
dapr stop --app-id nwd-inventory-api
```

3. Pub/Sub
```
Invoke-WebRequest 'http://localhost:50001/v1.0/invoke/nwd-orders-api/method/api/product'
```

## References
1. Getting started - [https:/docs.dapr.io/getting-started/](https://docs.dapr.io/getting-started/)