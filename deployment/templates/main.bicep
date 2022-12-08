@allowed([
  'prd'
  'uat'
  'dev'
  'lab'
])
param environmentName string = 'dev'
param solutionName string = 'dapr'
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

var logAnalyticsWorkspace = {
  name: 'log-${solutionName}-${environmentName}-${constants.dataCenterCode}'
  logRetentionDays: contains(constants.nonprod, environmentName) ? 30 : 90
}

var appInsights = {
  name: 'appi-${solutionName}-${environmentName}-${constants.dataCenterCode}'
}

var containerRegistry = {
  name: 'cr${solutionName}${environmentName}${constants.dataCenterCode}'
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

var keyVault = {
  name: 'kv-${solutionName}-${environmentName}-${constants.dataCenterCode}'
}

var serviceBus = {
  name: 'sb-${solutionName}-${environmentName}-${constants.dataCenterCode}'
}

// ##################################################################
//  Modules
// ##################################################################

module moduleLogAnalyticsWorkspace './logAnalyticsWorkspace.bicep' = {
  name: 'logAnalyticsWorkspace-${buildId}'
  params: {
    location: location
    standardTags: standardTags
    logAnalyticsWorkspace: logAnalyticsWorkspace
  }
}

module moduleAppInsights './appInsights.bicep' = {
  name: 'appInsights-${buildId}'
  dependsOn: [
    moduleLogAnalyticsWorkspace
  ]
  params: {
    location: location
    standardTags: standardTags
    appInsights: appInsights
    logAnalyticsWorkspaceId: moduleLogAnalyticsWorkspace.outputs.id
  }
}

module moduleKeyVault './keyVault.bicep' = {
  name: 'keyVault-${buildId}'
  dependsOn: [
    moduleLogAnalyticsWorkspace
  ]
  params: {
    location: location
    standardTags: standardTags
    constants: constants
    keyVault: keyVault
    logAnalyticsWorkspaceId: moduleLogAnalyticsWorkspace.outputs.id
  }
}

module moduleContainerRegistry './containerRegistry.bicep' = {
  name: 'containerRegistry-${buildId}'
  dependsOn: []
  params: {
    location: location
    standardTags: standardTags
    containerRegistry: containerRegistry
  }
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

// Key-vault secret: cosmosdbMasterKey
module moduleAkvSecret_cosmosdbMasterKey './keyVault.secret.bicep' = {
  name: 'akvSecret_cosmosdbMasterKey-${buildId}'
  dependsOn: [
    moduleKeyVault
    moduleCosmosDb
  ]
  params: {
    keyVaultName: keyVault.name
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
    moduleKeyVault
    moduleCosmosDb
  ]
  params: {
    keyVaultName: keyVault.name
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

// Key-vault secret: acrLoginServer
module moduleAkvSecret_acrLoginServer './keyVault.secret.bicep' = {
  name: 'akvSecret_acrLoginServer-${buildId}'
  dependsOn: [
    moduleKeyVault
    moduleContainerRegistry
  ]
  params: {
    keyVaultName: keyVault.name
    name: 'acrLoginServer'
    secretValue: moduleContainerRegistry.outputs.logingServer
    contentType: 'plain/text'
    tags: {
      CredentialId: 'loginServer'
      ProviderAddress: moduleContainerRegistry.outputs.id
      ValidityPeriodDays: -1
    }
  }
}

// Key-vault secret: acrPassword
module moduleAkvSecret_acrPassword './keyVault.secret.bicep' = {
  name: 'akvSecret_acrPassword-${buildId}'
  dependsOn: [
    moduleKeyVault
    moduleContainerRegistry
  ]
  params: {
    keyVaultName: keyVault.name
    name: 'acrPassword'
    secretValue: moduleContainerRegistry.outputs.password
    contentType: 'plain/text'
    tags: {
      CredentialId: 'password'
      ProviderAddress: moduleContainerRegistry.outputs.id
      ValidityPeriodDays: 365
    }
    expiryDate: '${dateTimeToEpoch(dateTimeAdd(baseTime, 'P1Y'))}'
  }
}

// Key-vault secret: acrUserName
module moduleAkvSecret_acrUserName './keyVault.secret.bicep' = {
  name: 'akvSecret_acrUserName-${buildId}'
  dependsOn: [
    moduleKeyVault
    moduleContainerRegistry
  ]
  params: {
    keyVaultName: keyVault.name
    name: 'acrUserName'
    secretValue: moduleContainerRegistry.outputs.username
    contentType: 'plain/text'
    tags: {
      CredentialId: 'username'
      ProviderAddress: moduleContainerRegistry.outputs.id
      ValidityPeriodDays: -1
    }
  }
}

// Key-vault secret: servicebusConnectionString
module moduleAkvSecret_servicebus_connectionString './keyVault.secret.bicep' = {
  name: 'akvSecret_servicebusConnectionString-${buildId}'
  dependsOn: [
    moduleKeyVault
    moduleContainerRegistry
  ]
  params: {
    keyVaultName: keyVault.name
    name: 'servicebusConnectionString'
    secretValue: moduleServiceBus.outputs.primaryConnectionString
    contentType: 'plain/text'
    tags: {
      CredentialId: 'connectionString'
      ProviderAddress: moduleServiceBus.outputs.id
      ValidityPeriodDays: -1
    }
  }
}

// Key-vault secret: logAnalyticsWorkspaceCustomerId
module moduleAkvSecret_logAnalyticsWorkspaceCustomerId './keyVault.secret.bicep' = {
  name: 'akvSecret_logAnalyticsWorkspaceCustomerId-${buildId}'
  dependsOn: [
    moduleKeyVault
    moduleContainerRegistry
  ]
  params: {
    keyVaultName: keyVault.name
    name: 'logAnalyticsWorkspaceCustomerId'
    secretValue: moduleLogAnalyticsWorkspace.outputs.customerId
    contentType: 'plain/text'
    tags: {
      CredentialId: 'customerId'
      ProviderAddress: moduleLogAnalyticsWorkspace.outputs.customerId
      ValidityPeriodDays: -1
    }
  }
}

// Key-vault secret: logAnalyticsWorkspacePrimarySharedKey
module moduleAkvSecret_logAnalyticsWorkspacePrimarySharedKey './keyVault.secret.bicep' = {
  name: 'akvSecret_logAnalyticsWorkspacePrimarySharedKey-${buildId}'
  dependsOn: [
    moduleKeyVault
    moduleContainerRegistry
  ]
  params: {
    keyVaultName: keyVault.name
    name: 'logAnalyticsWorkspacePrimarySharedKey'
    secretValue: moduleLogAnalyticsWorkspace.outputs.primarySharedKey
    contentType: 'plain/text'
    tags: {
      CredentialId: 'primarySharedKey'
      ProviderAddress: moduleLogAnalyticsWorkspace.outputs.primarySharedKey
      ValidityPeriodDays: -1
    }
  }
}

output appInsights object = {
  id: moduleAppInsights.outputs.id
  name: appInsights.name
}

output keyVault object = {
  id: moduleKeyVault.outputs.id
  name: keyVault.name
}

output containerRegistry object = {
  id: moduleContainerRegistry.outputs.id
  name: containerRegistry.name
}

output cosmosDb object = {
  id: moduleCosmosDb.outputs.id
  name: cosmosDb.name
}

output serviceBus object = {
  id: moduleServiceBus.outputs.id
  name: serviceBus.name
}
