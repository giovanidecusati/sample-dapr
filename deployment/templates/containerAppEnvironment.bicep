param location string
param standardTags object
param containerAppEnvironment object
param keyvaultName string
@secure()
param logAnalyticsCustomerId string
@secure()
param logAnalyticsPrimarySharedKey string

resource resourceContainerAppEnvironment 'Microsoft.App/managedEnvironments@2022-06-01-preview' = {
  name: containerAppEnvironment.name
  location: location
  tags: standardTags
  sku: {
    name: 'Consumption'
  }
  properties: {
    appLogsConfiguration: {
      destination: 'log-analytics'
      logAnalyticsConfiguration: {
        customerId: logAnalyticsCustomerId
        sharedKey: logAnalyticsPrimarySharedKey
      }
    }
    zoneRedundant: false
  }
  resource resourceSecretStore 'daprComponents@2022-06-01-preview' = {
    name: 'nwd-appsecretstore'
    properties: {
      componentType: 'secretstores.azure.keyvault'
      version: 'v1'
      metadata: [
        {
          name: 'vaultName'
          value: keyvaultName
        }
        {
          name: 'azureTenantId'
          value: tenant().tenantId
        }
        {
          name: 'azureClientId'
          value: 'd172df0b-9009-4e2b-a82e-866c63730b8e'
        }
        {
          name: 'azureClientSecret'
          value: 'fcz8Q~q~W4GtjYl3yzXsLao0YdxgjadsB5.gHaOh'
        }
      ]
      scopes: [
        'nwd-inventory-api'
        'nwd-orders-api'
        'nwd-basket-api'
      ]
    }
  }
  resource resourceAppservicebus 'daprComponents@2022-06-01-preview' = {
    name: 'nwd-appservicebus'
    dependsOn: [
      resourceSecretStore
    ]
    properties: {
      componentType: 'pubsub.azure.servicebus'
      version: 'v1'
      secretStoreComponent: 'nwd-appsecretstore'
      metadata: [
        {
          name: 'connectionString'
          secretRef: 'servicebusConnectionString'
        }
      ]
      scopes: [
        'nwd-inventory-api'
        'nwd-orders-api'
        'nwd-basket-api'
      ]
    }
  }
  resource resourceNwdBasketBasket 'daprComponents@2022-06-01-preview' = {
    name: 'nwd-basket-basket'
    dependsOn: [
      resourceSecretStore
    ]
    properties: {
      componentType: 'state.azure.cosmosdb'
      version: 'v1'
      secretStoreComponent: 'nwd-appsecretstore'
      metadata: [
        {
          name: 'url'
          secretRef: 'cosmosdbDocumentEndpoint'
        }
        {
          name: 'masterKey'
          secretRef: 'cosmosdbMasterKey'
        }
        {
          name: 'database'
          value: 'BasketDB'
        }
        {
          name: 'collection'
          value: 'Basket'
        }
        {
          name: 'partitionKey'
          value: 'id'
        }
      ]
      scopes: [
        'nwd-inventory-api'
        'nwd-orders-api'
        'nwd-basket-api'
      ]
    }
  }
  resource resourceNwdInventoryCategory 'daprComponents@2022-06-01-preview' = {
    name: 'nwd-inventory-category'
    dependsOn: [
      resourceSecretStore
    ]
    properties: {
      componentType: 'state.azure.cosmosdb'
      version: 'v1'
      secretStoreComponent: 'nwd-appsecretstore'
      metadata: [
        {
          name: 'url'
          secretRef: 'cosmosdbDocumentEndpoint'
        }
        {
          name: 'masterKey'
          secretRef: 'cosmosdbMasterKey'
        }
        {
          name: 'database'
          value: 'InventoryDB'
        }
        {
          name: 'collection'
          value: 'Category'
        }
        {
          name: 'partitionKey'
          value: 'id'
        }
      ]
      scopes: [
        'nwd-inventory-api'
        'nwd-orders-api'
        'nwd-basket-api'
      ]
    }
  }
  resource resourceNwdInventoryInventory 'daprComponents@2022-06-01-preview' = {
    name: 'nwd-inventory-inventory'
    dependsOn: [
      resourceSecretStore
    ]
    properties: {
      componentType: 'state.azure.cosmosdb'
      version: 'v1'
      secretStoreComponent: 'nwd-appsecretstore'
      metadata: [
        {
          name: 'url'
          secretRef: 'cosmosdbDocumentEndpoint'
        }
        {
          name: 'masterKey'
          secretRef: 'cosmosdbMasterKey'
        }
        {
          name: 'database'
          value: 'InventoryDB'
        }
        {
          name: 'collection'
          value: 'Inventory'
        }
        {
          name: 'partitionKey'
          value: 'id'
        }
      ]
      scopes: [
        'nwd-inventory-api'
        'nwd-orders-api'
        'nwd-basket-api'
      ]
    }
  }
  resource resourceNwdInventoryProduct 'daprComponents@2022-06-01-preview' = {
    name: 'nwd-inventory-product'
    dependsOn: [
      resourceSecretStore
    ]
    properties: {
      componentType: 'state.azure.cosmosdb'
      version: 'v1'
      secretStoreComponent: 'nwd-appsecretstore'
      metadata: [
        {
          name: 'url'
          secretRef: 'cosmosdbDocumentEndpoint'
        }
        {
          name: 'masterKey'
          secretRef: 'cosmosdbMasterKey'
        }
        {
          name: 'database'
          value: 'InventoryDB'
        }
        {
          name: 'collection'
          value: 'Product'
        }
        {
          name: 'partitionKey'
          value: 'id'
        }
      ]
      scopes: [
        'nwd-inventory-api'
        'nwd-orders-api'
        'nwd-basket-api'
      ]
    }
  }
  resource resourceNwdInventoryTransaction 'daprComponents@2022-06-01-preview' = {
    name: 'nwd-inventory-transaction'
    dependsOn: [
      resourceSecretStore
    ]
    properties: {
      componentType: 'state.azure.cosmosdb'
      version: 'v1'
      secretStoreComponent: 'nwd-appsecretstore'
      metadata: [
        {
          name: 'url'
          secretRef: 'cosmosdbDocumentEndpoint'
        }
        {
          name: 'masterKey'
          secretRef: 'cosmosdbMasterKey'
        }
        {
          name: 'database'
          value: 'InventoryDB'
        }
        {
          name: 'collection'
          value: 'Transaction'
        }
        {
          name: 'partitionKey'
          value: 'id'
        }
      ]
      scopes: [
        'nwd-inventory-api'
        'nwd-orders-api'
        'nwd-basket-api'
      ]
    }
  }
  resource resourceNwdOrdersCustomer 'daprComponents@2022-06-01-preview' = {
    name: 'nwd-orders-customer'
    dependsOn: [
      resourceSecretStore
    ]
    properties: {
      componentType: 'state.azure.cosmosdb'
      version: 'v1'
      secretStoreComponent: 'nwd-appsecretstore'
      metadata: [
        {
          name: 'url'
          secretRef: 'cosmosdbDocumentEndpoint'
        }
        {
          name: 'masterKey'
          secretRef: 'cosmosdbMasterKey'
        }
        {
          name: 'database'
          value: 'OrdersDB'
        }
        {
          name: 'collection'
          value: 'Customer'
        }
        {
          name: 'partitionKey'
          value: 'id'
        }
      ]
      scopes: [
        'nwd-inventory-api'
        'nwd-orders-api'
        'nwd-basket-api'
      ]
    }
  }
  resource resourceNwdOrdersOrder 'daprComponents@2022-06-01-preview' = {
    name: 'nwd-orders-order'
    dependsOn: [
      resourceSecretStore
    ]
    properties: {
      componentType: 'state.azure.cosmosdb'
      version: 'v1'
      secretStoreComponent: 'nwd-appsecretstore'
      metadata: [
        {
          name: 'url'
          secretRef: 'cosmosdbDocumentEndpoint'
        }
        {
          name: 'masterKey'
          secretRef: 'cosmosdbMasterKey'
        }
        {
          name: 'database'
          value: 'OrdersDB'
        }
        {
          name: 'collection'
          value: 'Order'
        }
        {
          name: 'partitionKey'
          value: 'id'
        }
      ]
      scopes: [
        'nwd-inventory-api'
        'nwd-orders-api'
        'nwd-basket-api'
      ]
    }
  }
  resource resourceNwdOrdersOrderProcessorActor 'daprComponents@2022-06-01-preview' = {
    name: 'nwd-orders-orderprocessoractor'
    dependsOn: [
      resourceSecretStore
    ]
    properties: {
      componentType: 'state.azure.cosmosdb'
      version: 'v1'
      secretStoreComponent: 'nwd-appsecretstore'
      metadata: [
        {
          name: 'url'
          secretRef: 'cosmosdbDocumentEndpoint'
        }
        {
          name: 'masterKey'
          secretRef: 'cosmosdbMasterKey'
        }
        {
          name: 'database'
          value: 'OrdersDB'
        }
        {
          name: 'collection'
          value: 'orderProcessorActor'
        }
        {
          name: 'partitionKey'
          value: 'id'
        }
      ]
      scopes: [
        'nwd-inventory-api'
        'nwd-orders-api'
        'nwd-basket-api'
      ]
    }
  }
  resource resourceNwdOrdersProduct 'daprComponents@2022-06-01-preview' = {
    name: 'nwd-orders-product'
    dependsOn: [
      resourceSecretStore
    ]
    properties: {
      componentType: 'state.azure.cosmosdb'
      version: 'v1'
      secretStoreComponent: 'nwd-appsecretstore'
      metadata: [
        {
          name: 'url'
          secretRef: 'cosmosdbDocumentEndpoint'
        }
        {
          name: 'masterKey'
          secretRef: 'cosmosdbMasterKey'
        }
        {
          name: 'database'
          value: 'OrdersDB'
        }
        {
          name: 'collection'
          value: 'Product'
        }
        {
          name: 'partitionKey'
          value: 'id'
        }
      ]
      scopes: [
        'nwd-inventory-api'
        'nwd-orders-api'
        'nwd-basket-api'
      ]
    }
  }
}

output id string = resourceContainerAppEnvironment.id
