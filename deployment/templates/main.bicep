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

// Key-vault secret: adonetSQLServerConnectionString
// module moduleKeyVaultSecretADONETSqlServerConnectionString './keyVault.secret.bicep' = {
//   name: 'akvADONETSqlServerConnectionStringDeploy'
//   dependsOn: [
//     moduleKeyVault
//   ]
//   params: {
//     keyVaultName: keyVault.name
//     name: 'adonetSQLServerConnectionString'
//     secretValue: moduleSqlServerDatabases.outputs.adonetSQLServerConnectionString
//     contentType: 'plain/text'
//     tags: {
//       CredentialId: 'connectionString'
//       ProviderAddress: moduleSqlServers.outputs.sqlServerId
//       ValidityPeriodDays: 365
//     }
//     expiryDate: '${dateTimeToEpoch(dateTimeAdd(baseTime, 'P1Y'))}'
//   }
// }
