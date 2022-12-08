@allowed([
  'prd'
  'uat'
  'dev'
  'lab'
])
param environmentName string = 'dev'
param solutionName string = 'dapr'
param keyVaultName string = 'kv-giodapr-lab-ause'
param location string = resourceGroup().location
param buildId string
param baseTime string = utcNow('u')

var standardTags = {
  environment: environmentName
  solutionName: solutionName
  department: 'IT'
  businessOwner: 'giovani'
  technicalOwner: 'giovani'
}

var constants = {
  nonprod: [
    'dev'
    'lab'
  ]
  diagnosticSettingName: 'defaultDiagnosticSettings'
  dataCenterCode: 'ause'
}

var cosmosDb = {
  name: 'cosmos-${solutionName}-${environmentName}-${constants.dataCenterCode}'
  totalThroughputLimit: 4000
  locations: [
    {
      locationName: 'australiasoutheast'
    }
  ]
  databases: [
    {
      name: 'BasketDB'
      containers: [ 'Basket' ]
    }
    {
      name: 'InventoryDB'
      containers: [ 'Category', 'Inventory', 'Product', 'Transaction' ]
    }
    {
      name: 'OrdersDB'
      containers: [ 'Customer', 'Order', 'orderProcessorActor', 'Product' ]
    }
  ]
}

var serviceBus = {
  name: 'sb-${solutionName}-${environmentName}-${constants.dataCenterCode}'
}

var containerAppEnvironment = {
  name: 'cae-${solutionName}-${environmentName}-aue'
  location: 'australiaeast'
}

// ##################################################################
//  Modules
// ##################################################################

resource resourceKeyVault 'Microsoft.KeyVault/vaults@2022-07-01' existing = {
  name: keyVaultName
}

module moduleCosmosDb './cosmosdb.bicep' = {
  name: 'cosmosdb-${buildId}'
  dependsOn: []
  params: {
    location: location
    standardTags: standardTags
    cosmosDb: cosmosDb
  }
}

module moduleCosmosDbContainer './cosmosdbContainer.bicep' = [for database in cosmosDb.databases: {
  name: 'cosmosdb${database.name}Container-${buildId}'
  dependsOn: [
    moduleCosmosDb
  ]
  params: {
    location: location
    standardTags: standardTags
    cosmosDbName: cosmosDb.name
    databaseName: database.name
    containerNames: database.containers
  }
}]

module moduleServiceBus './serviceBus.bicep' = {
  name: 'serviceBus-${buildId}'
  dependsOn: []
  params: {
    location: location
    standardTags: standardTags
    serviceBus: serviceBus
  }
}

module moduleContainerAppEnvironment './containerAppEnvironment.bicep' = {
  name: 'containerAppEnvironment-${buildId}'
  dependsOn: []
  params: {
    location: containerAppEnvironment.location
    standardTags: standardTags
    containerAppEnvironment: containerAppEnvironment
    logAnalyticsCustomerId: resourceKeyVault.getSecret('logAnalyticsCustomerId')
    logAnalyticsPrimarySharedKey: resourceKeyVault.getSecret('logAnalyticsPrimarySharedKey')
  }
}

// Key-vault secret: cosmosdbMasterKey
module moduleAkvSecret_cosmosdbMasterKey './keyVault.secret.bicep' = {
  name: 'akvSecret_cosmosdbMasterKey-${buildId}'
  dependsOn: [
    moduleCosmosDb
  ]
  params: {
    keyVaultName: keyVaultName
    name: 'cosmosdbMasterKey'
    secretValue: moduleCosmosDb.outputs.primaryMasterKey
    contentType: 'plain/text'
    tags: {
      CredentialId: 'primaryMasterKey'
      ProviderAddress: moduleCosmosDb.outputs.id
      ValidityPeriodDays: 365
    }
    expiryDate: '${dateTimeToEpoch(dateTimeAdd(baseTime, 'P1Y'))}'
  }
}

// Key-vault secret: cosmosdbDocumentEndpoint
module moduleAkvSecret_cosmosdbDocumentEndpoint './keyVault.secret.bicep' = {
  name: 'akvSecret_cosmosdbDocumentEndpoint-${buildId}'
  dependsOn: [
    moduleCosmosDb
  ]
  params: {
    keyVaultName: keyVaultName
    name: 'cosmosdbDocumentEndpoint'
    secretValue: moduleCosmosDb.outputs.documentEndpoint
    contentType: 'plain/text'
    tags: {
      CredentialId: 'documentEndpoint'
      ProviderAddress: moduleCosmosDb.outputs.id
      ValidityPeriodDays: 365
    }
    expiryDate: '${dateTimeToEpoch(dateTimeAdd(baseTime, 'P1Y'))}'
  }
}

output cosmosDb object = {
  id: moduleCosmosDb.outputs.id
  name: cosmosDb.name
}

output serviceBus object = {
  id: moduleServiceBus.outputs.id
  name: serviceBus.name
}

output containerAppEnvironment object = {
  id: moduleContainerAppEnvironment.outputs.id
  name: containerAppEnvironment.name
}
