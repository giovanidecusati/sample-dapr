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
* It simulates a Data-Driven (CRUD) scenario. 
* Consumes Dapr State Service API.
* TODO: Once basket is submitted to checkout it publishes a message in the topic *order-submitted*.

## Inventory microservice
It handles product and inventory operations.
* It simulates a very complext domain. 
* Pusblishes a *ProductCreatedEvent* into a topic *new-product-topic* for every new product created.
* Uses isolation level to deal with concurrency transactions due to stock level operations.

## Orders microservice
It handles orders operations.
* It simulates a CRUD. 
* Subscribes a topic *new-product-topic*.
* Once a order is submitted a state machine (SAGA) starts to reserve the items in the inventory and process payment. 
* TODO: Subscribes a topic *order-submitted*.

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
Open Visual Studion Solition and run *docker-compose*. Solution will start and attach all application with debugging supporr.

## Deployment
* Run *Infrastructure pipeline*.
* Create an SPN to allow containers app get secrets from AKV.
* Add the following values into AKV.  
  * spnDaprClientId
  * spnDaprClientSecret
  * spnDaprObjectId: use below command to obtain spn Id
  ```
  (Get-AzADServicePrincipal -DisplayName 'spn-giodapr-lab-ause').Id
  ```
* Run *build pipeline*.
* Run *dapr componets pipeline*.
* Every new build you can run *create revision pipeline* to update containers to latest version.


## References
1. Getting started - [https:/docs.dapr.io/getting-started/](https://docs.dapr.io/getting-started/)