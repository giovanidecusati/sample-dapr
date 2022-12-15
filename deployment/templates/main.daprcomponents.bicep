@allowed([
  'prd'
  'uat'
  'dev'
  'lab'
])
param environmentName string
param solutionName string
param keyVaultName string
param imageVersion string
param containerRegistryName string
param containerAppManagedEnvironmentId string
param location string = resourceGroup().location
param buildId string
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

var containerAppBasketApi = {
  name: 'ca-${solutionName}-${environmentName}-${appConstants.dataCenterCode}-basketapi'
  appId: 'nwd-basket-api'
  image: '${containerRegistryName}/nwd-basket-api:${imageVersion}'
}

var containerAppInventoryApi = {
  name: 'ca-${solutionName}-${environmentName}-${appConstants.dataCenterCode}-inventoryapi'
  appId: 'nwd-inventory-api'
  image: '${containerRegistryName}/nwd-inventory-api:${imageVersion}'
}

var containerAppOrdersApi = {
  name: 'ca-${solutionName}-${environmentName}-${appConstants.dataCenterCode}-ordersapi'
  appId: 'nwd-orders-api'
  image: '${containerRegistryName}/nwd-orders-api:${imageVersion}'
}

// ##################################################################
//  Modules
// ##################################################################

resource resourceKeyVault 'Microsoft.KeyVault/vaults@2022-07-01' existing = {
  name: keyVaultName
}

module moduleContainerAppBasketApi './containerApp.bicep' = {
  name: 'containerAppBasketApi-${buildId}'
  dependsOn: []
  params: {
    location: location
    standardTags: standardTags
    containerApp: containerAppBasketApi
    acrPassword: resourceKeyVault.getSecret('acrPassword')
    acrServer: resourceKeyVault.getSecret('acrLoginServer')
    acrUserName: resourceKeyVault.getSecret('acrUserName')
    managedEnvironmentId: containerAppManagedEnvironmentId
    appInsightsConnectionString: resourceKeyVault.getSecret('appInsightsConnectionString')
  }
}

module moduleContainerAppInventoryApi './containerApp.bicep' = {
  name: 'containerAppInventoryApi-${buildId}'
  dependsOn: []
  params: {
    location: location
    standardTags: standardTags
    containerApp: containerAppInventoryApi
    acrPassword: resourceKeyVault.getSecret('acrPassword')
    acrServer: resourceKeyVault.getSecret('acrLoginServer')
    acrUserName: resourceKeyVault.getSecret('acrUserName')
    managedEnvironmentId: location
    appInsightsConnectionString: resourceKeyVault.getSecret('appInsightsConnectionString')
  }
}

module moduleContainerAppOrdersApi './containerApp.bicep' = {
  name: 'containerAppOrdersApi-${buildId}'
  dependsOn: []
  params: {
    location: location
    standardTags: standardTags
    containerApp: containerAppOrdersApi
    acrPassword: resourceKeyVault.getSecret('acrPassword')
    acrServer: resourceKeyVault.getSecret('acrLoginServer')
    acrUserName: resourceKeyVault.getSecret('acrUserName')
    managedEnvironmentId: location
    appInsightsConnectionString: resourceKeyVault.getSecret('appInsightsConnectionString')
  }
}

module moduleKeyVaultAccessPolicy './keyVault.accessPolicies.bicep' = {
  name: 'keyVaultAccessPolicy-${buildId}'
  dependsOn: [
    resourceKeyVault
  ]
  params: {
    keyVaultName: keyVaultName
    objectId: moduleContainerAppBasketApi.outputs.principalId
    secrets: [
      'get'
    ]
  }
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
