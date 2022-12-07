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

var containerAppEnvironment = {
  name: 'cae-${solutionName}-${environmentName}-ause'
  location: 'australiaeast'
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

module moduleContainerAppEnvironment './containerAppEnvironment.bicep' = {
  name: 'containerAppEnvironment-${buildId}'
  dependsOn: []
  params: {
    location: containerAppEnvironment.location
    standardTags: standardTags
    containerAppEnvironment: containerAppEnvironment
    logAnalyticsCustomerId: moduleLogAnalyticsWorkspace.outputs.customerId
    logAnalyticsPrimarySharedKey: moduleLogAnalyticsWorkspace.outputs.primarySharedKey
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

// Key-vault secret: cosmosdb_masterKey
module moduleAkvSecret_cosmosdb_masterKey './keyVault.secret.bicep' = {
  name: 'akvSecret_cosmosdb_masterKey-${buildId}'
  dependsOn: [
    moduleKeyVault
  ]
  params: {
    keyVaultName: keyVault.name
    name: 'cosmosdb_masterKey'
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

// Key-vault secret: acr_loginServer
module moduleAkvSecret_acr_loginServer './keyVault.secret.bicep' = {
  name: 'akvSecret_acr_loginServer-${buildId}'
  dependsOn: [
    moduleKeyVault
  ]
  params: {
    keyVaultName: keyVault.name
    name: 'acr_loginServer'
    secretValue: moduleContainerRegistry.outputs.logingServer
    contentType: 'plain/text'
    tags: {
      CredentialId: 'loginServer'
      ProviderAddress: moduleContainerRegistry.outputs.id
      ValidityPeriodDays: -1
    }
  }
}

// Key-vault secret: acr_password
module moduleAkvSecret_acr_password './keyVault.secret.bicep' = {
  name: 'akvSecret_acr_password-${buildId}'
  dependsOn: [
    moduleKeyVault
  ]
  params: {
    keyVaultName: keyVault.name
    name: 'acr_password'
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

// Key-vault secret: acr_username
module moduleAkvSecret_acr_username './keyVault.secret.bicep' = {
  name: 'akvSecret_acr_username-${buildId}'
  dependsOn: [
    moduleKeyVault
  ]
  params: {
    keyVaultName: keyVault.name
    name: 'acr_username'
    secretValue: moduleContainerRegistry.outputs.username
    contentType: 'plain/text'
    tags: {
      CredentialId: 'username'
      ProviderAddress: moduleContainerRegistry.outputs.id
      ValidityPeriodDays: -1
    }
  }
}
