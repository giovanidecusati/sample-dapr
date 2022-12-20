@allowed([
  'prd'
  'uat'
  'dev'
  'lab'
])
param environmentName string
param solutionName string
param buildId string
param location string = resourceGroup().location
param baseTime string = utcNow('u')

var standardTags = {
  environment: environmentName
  solutionName: solutionName
}

var appConstants = {
  nonprod: [
    'dev'
    'lab'
  ]
  diagnosticSettingName: 'defaultDiagnosticSettings'
  dataCenterCode: 'aue'
}

var logAnalyticsWorkspace = {
  name: 'log-${solutionName}-${environmentName}-${appConstants.dataCenterCode}'
  logRetentionDays: contains(appConstants.nonprod, environmentName) ? 30 : 90
}

var appInsights = {
  name: 'appi-${solutionName}-${environmentName}-${appConstants.dataCenterCode}'
}

var containerRegistry = {
  name: 'cr${solutionName}${environmentName}${appConstants.dataCenterCode}'
}

var keyVault = {
  name: 'kv-${solutionName}-${environmentName}-${appConstants.dataCenterCode}'
}

var cosmosDb = {
  name: 'cosmos-${solutionName}-${environmentName}-${appConstants.dataCenterCode}'
  totalThroughputLimit: 4000
  locations: [
    {
      locationName: location
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
  name: 'sb-${solutionName}-${environmentName}-${appConstants.dataCenterCode}'
}

var containerAppEnvironment = {
  name: 'cae-${solutionName}-${environmentName}-${appConstants.dataCenterCode}'
}

var containerAppEnvUserManagedIdentity = {
  name: 'id-ca-${solutionName}-${environmentName}-${appConstants.dataCenterCode}'
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
  dependsOn: []
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
    constants: appConstants
    keyVault: keyVault
    logAnalyticsWorkspaceId: moduleLogAnalyticsWorkspace.outputs.id
  }
}

module moduleUserManagedIdentity './userManagedIdentity.bicep' = {
  name: 'moduleUserManagedIdentity-${buildId}'
  dependsOn: [
    moduleKeyVault
  ]
  params: {
    location: location
    managedIdentity: containerAppEnvUserManagedIdentity
    standardTags: standardTags
  }
}

module moduleKeyVaultUserManagedIdentity './keyVault.accessPolicies.bicep' = {
  name: 'keyVaultUserManagedIdentity-${buildId}'
  dependsOn: [
    moduleKeyVault
  ]
  params: {
    keyVaultName: keyVault.name
    objectId: moduleUserManagedIdentity.outputs.principalId
    secrets: [
      'get'
    ]
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

module moduleContainerAppEnvironment './containerAppEnvironment.bicep' = {
  name: 'containerAppEnvironment-${buildId}'
  dependsOn: []
  params: {
    location: location
    standardTags: standardTags
    containerAppEnvironment: containerAppEnvironment
    logAnalyticsCustomerId: moduleLogAnalyticsWorkspace.outputs.customerId
    logAnalyticsPrimarySharedKey: moduleLogAnalyticsWorkspace.outputs.primarySharedKey
    keyvaultName: keyVault.name
    applicationInsightsConnectionString: moduleAppInsights.outputs.ConnectionString
  }
}

// Key-vault secret: appInsightsConnectionString
module moduleAkvSecret_appInsightsConnectionString './keyVault.secret.bicep' = {
  name: 'akvSecret_appInsightsConnectionString-${buildId}'
  dependsOn: [
    moduleKeyVault
    moduleContainerRegistry
  ]
  params: {
    keyVaultName: keyVault.name
    name: 'appInsightsConnectionString'
    secretValue: moduleAppInsights.outputs.ConnectionString
    contentType: 'plain/text'
    tags: {
      CredentialId: 'connectionString'
      ProviderAddress: moduleAppInsights.outputs.id
      ValidityPeriodDays: -1
    }
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

// Key-vault secret: servicebusConnectionString
module moduleAkvSecret_servicebusConnectionString './keyVault.secret.bicep' = {
  name: 'akvSecret_servicebusConnectionString-${buildId}'
  dependsOn: [
    moduleKeyVault
    moduleCosmosDb
  ]
  params: {
    keyVaultName: keyVault.name
    name: 'servicebusConnectionString'
    secretValue: moduleServiceBus.outputs.primaryConnectionString
    contentType: 'plain/text'
    tags: {
      CredentialId: 'primaryConnectionString'
      ProviderAddress: moduleServiceBus.outputs.id
      ValidityPeriodDays: 365
    }
    expiryDate: '${dateTimeToEpoch(dateTimeAdd(baseTime, 'P1Y'))}'
  }
}

output logAnalyticsWorkspaceId string = moduleLogAnalyticsWorkspace.outputs.id
output logAnalyticsWorkspaceName string = logAnalyticsWorkspace.name
output keyVaultId string = moduleKeyVault.outputs.id
output keyVaultName string = keyVault.name
output containerRegistryId string = moduleContainerRegistry.outputs.id
output containerRegistryName string = containerRegistry.name
output containerRegistryLoginServer string = moduleContainerRegistry.outputs.logingServer
output cosmosDbId string = moduleCosmosDb.outputs.id
output cosmosDbName string = cosmosDb.name
output serviceBusId string = moduleServiceBus.outputs.id
output serviceBusName string = serviceBus.name
output containerAppEnvironmentId string = moduleContainerAppEnvironment.outputs.id
output containerAppEnvironmentName string = containerAppEnvironment.name
output userManagedIdentityPrincipalId string = moduleUserManagedIdentity.outputs.principalId
