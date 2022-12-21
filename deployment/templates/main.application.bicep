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
param containerRegistryLoginserver string
param containerAppEnvName string
param containerAppEnvUsrMngtIdName string
param buildId string
param location string = resourceGroup().location

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
  image: '${containerRegistryLoginserver}/nwd-basket-api:${imageVersion}'
}

var containerAppInventoryApi = {
  name: 'ca-${solutionName}-${environmentName}-${appConstants.dataCenterCode}-inventoryapi'
  appId: 'nwd-inventory-api'
  image: '${containerRegistryLoginserver}/nwd-inventory-api:${imageVersion}'
}

var containerAppOrdersApi = {
  name: 'ca-${solutionName}-${environmentName}-${appConstants.dataCenterCode}-ordersapi'
  appId: 'nwd-orders-api'
  image: '${containerRegistryLoginserver}/nwd-orders-api:${imageVersion}'
}

// ##################################################################
//  Modules
// ##################################################################

resource resourceContainerAppEnvironment 'Microsoft.App/managedEnvironments@2022-06-01-preview' existing = {
  name: containerAppEnvName
}

resource moduleUserManagedIdentity 'Microsoft.ManagedIdentity/userAssignedIdentities@2022-01-31-preview' existing = {
  name: containerAppEnvUsrMngtIdName
}

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
    acrServer: resourceKeyVault.getSecret('acrLoginServer')
    managedEnvironmentId: resourceContainerAppEnvironment.id
    managedEnvironmentIdentityId: moduleUserManagedIdentity.id
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
    acrServer: resourceKeyVault.getSecret('acrLoginServer')
    managedEnvironmentId: resourceContainerAppEnvironment.id
    managedEnvironmentIdentityId: moduleUserManagedIdentity.id
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
    acrServer: resourceKeyVault.getSecret('acrLoginServer')
    managedEnvironmentId: resourceContainerAppEnvironment.id
    managedEnvironmentIdentityId: moduleUserManagedIdentity.id
    appInsightsConnectionString: resourceKeyVault.getSecret('appInsightsConnectionString')
  }
}

output containerAppBasketApiId string = moduleContainerAppBasketApi.outputs.id
output containerAppBasketApiName string = containerAppBasketApi.name
output containerAppBasketApiFqdn string = moduleContainerAppBasketApi.outputs.fqdn

output containerAppInventoryApiId string = moduleContainerAppInventoryApi.outputs.id
output containerAppInventoryApiName string = containerAppInventoryApi.name
output containerAppInventoryApiFqdn string = moduleContainerAppInventoryApi.outputs.fqdn

output containerAppOrdersApiId string = moduleContainerAppOrdersApi.outputs.id
output containerAppOrdersApiName string = containerAppOrdersApi.name
output containerAppOrdersApiFqdn string = moduleContainerAppOrdersApi.outputs.fqdn
