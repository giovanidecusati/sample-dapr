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

var containerAppEnvironment = {
  name: 'cae-${solutionName}-${environmentName}-aue'
  location: 'australiaeast'
}

var containerAppBasketApi = {
  name: 'ca-${solutionName}-${environmentName}-${constants.dataCenterCode}-basketapi'
  appId: 'nwd-basket-api'
  image: 'crgiodaprlabause.azurecr.io/nwd-basket-api:latest'
}

var containerAppInventoryApi = {
  name: 'ca-${solutionName}-${environmentName}-${constants.dataCenterCode}-inventoryapi'
  appId: 'nwd-inventory-api'
  image: 'crgiodaprlabause.azurecr.io/nwd-inventory-api:latest'
}

var containerAppOrdersApi = {
  name: 'ca-${solutionName}-${environmentName}-${constants.dataCenterCode}-ordersapi'
  appId: 'nwd-orders-api'
  image: 'crgiodaprlabause.azurecr.io/nwd-orders-api:latest'
}

// ##################################################################
//  Modules
// ##################################################################

resource resourceKeyVault 'Microsoft.KeyVault/vaults@2022-07-01' existing = {
  name: keyVaultName
}

module moduleKeyVaultAccessPolicy './keyVault.accessPolicies.bicep' = {
  name: 'keyVaultAccessPolicy-${buildId}'
  dependsOn: [
    resourceKeyVault
  ]
  params: {
    keyVaultName: keyVaultName
    objectId: resourceKeyVault.getSecret('spnDaprObjectId')
    secrets: [
      'get'
    ]
  }
}

module moduleContainerAppEnvironment './containerAppEnvironment.bicep' = {
  name: 'containerAppEnvironment-${buildId}'
  dependsOn: []
  params: {
    location: containerAppEnvironment.location
    standardTags: standardTags
    containerAppEnvironment: containerAppEnvironment
    logAnalyticsCustomerId: resourceKeyVault.getSecret('logAnalyticsWorkspaceCustomerId')
    logAnalyticsPrimarySharedKey: resourceKeyVault.getSecret('logAnalyticsWorkspacePrimarySharedKey')
    keyvaultName: keyVaultName
    azureClientId: resourceKeyVault.getSecret('spnDaprClientId')
    azureClientSecret: resourceKeyVault.getSecret('spnDaprClientSecret')
  }
}

module moduleContainerAppBasketApi './containerApp.bicep' = {
  name: 'containerAppBasketApi-${buildId}'
  dependsOn: []
  params: {
    location: containerAppEnvironment.location
    standardTags: standardTags
    containerApp: containerAppBasketApi
    acrPassword: resourceKeyVault.getSecret('acrPassword')
    acrServer: resourceKeyVault.getSecret('acrLoginServer')
    acrUserName: resourceKeyVault.getSecret('acrUserName')
    managedEnvironmentId: moduleContainerAppEnvironment.outputs.id
    appInsightsConnectionString: resourceKeyVault.getSecret('appInsightsConnectionString')
  }
}

module moduleContainerAppInventoryApi './containerApp.bicep' = {
  name: 'containerAppInventoryApi-${buildId}'
  dependsOn: []
  params: {
    location: containerAppEnvironment.location
    standardTags: standardTags
    containerApp: containerAppInventoryApi
    acrPassword: resourceKeyVault.getSecret('acrPassword')
    acrServer: resourceKeyVault.getSecret('acrLoginServer')
    acrUserName: resourceKeyVault.getSecret('acrUserName')
    managedEnvironmentId: moduleContainerAppEnvironment.outputs.id
    appInsightsConnectionString: resourceKeyVault.getSecret('appInsightsConnectionString')
  }
}

module moduleContainerAppOrdersApi './containerApp.bicep' = {
  name: 'containerAppOrdersApi-${buildId}'
  dependsOn: []
  params: {
    location: containerAppEnvironment.location
    standardTags: standardTags
    containerApp: containerAppOrdersApi
    acrPassword: resourceKeyVault.getSecret('acrPassword')
    acrServer: resourceKeyVault.getSecret('acrLoginServer')
    acrUserName: resourceKeyVault.getSecret('acrUserName')
    managedEnvironmentId: moduleContainerAppEnvironment.outputs.id
    appInsightsConnectionString: resourceKeyVault.getSecret('appInsightsConnectionString')
  }
}

// Key-vault secret: containerAppNameForOrdersApi
module moduleAkvSecret_containerAppNameForOrdersApi './keyVault.secret.bicep' = {
  name: 'akvSecret_containerAppNameForOrdersApi-${buildId}'
  dependsOn: [
    resourceKeyVault
    moduleContainerAppOrdersApi
  ]
  params: {
    keyVaultName: keyVaultName
    name: 'containerAppNameForOrdersApi'
    secretValue: containerAppOrdersApi.name
    contentType: 'plain/text'
    tags: {}
  }
}

// Key-vault secret: containerAppNameForInventoryApi
module moduleAkvSecret_containerAppNameForInventoryApi './keyVault.secret.bicep' = {
  name: 'akvSecret_containerAppNameForInventoryApi-${buildId}'
  dependsOn: [
    resourceKeyVault
    moduleContainerAppInventoryApi
  ]
  params: {
    keyVaultName: keyVaultName
    name: 'containerAppNameForInventoryApi'
    secretValue: containerAppInventoryApi.name
    contentType: 'plain/text'
    tags: {}
  }
}

// Key-vault secret: containerAppNameForBasketApi
module moduleAkvSecret_containerAppNameForBasketApi './keyVault.secret.bicep' = {
  name: 'akvSecret_containerAppNameForInventoryApi-${buildId}'
  dependsOn: [
    resourceKeyVault
    moduleContainerAppBasketApi
  ]
  params: {
    keyVaultName: keyVaultName
    name: 'containerAppNameForBasketApi'
    secretValue: containerAppBasketApi.name
    contentType: 'plain/text'
    tags: {}
  }
}

output containerAppEnvironment object = {
  id: moduleContainerAppEnvironment.outputs.id
  name: containerAppEnvironment.name
}

output containerAppBasketApi object = {
  id: moduleContainerAppBasketApi.outputs.id
  name: containerAppBasketApi.name
  fqdn: moduleContainerAppBasketApi.outputs.fqdn
}

output containerAppInventoryApi object = {
  id: moduleContainerAppInventoryApi.outputs.id
  name: containerAppInventoryApi.name
  fqdn: moduleContainerAppInventoryApi.outputs.fqdn
}

output containerAppOrdersApi object = {
  id: moduleContainerAppOrdersApi.outputs.id
  name: containerAppOrdersApi.name
  fqdn: moduleContainerAppOrdersApi.outputs.fqdn
}
