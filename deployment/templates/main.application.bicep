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

var managedEnvironmentId = resourceId('Microsoft.App/managedEnvironments', containerAppEnvName)
var containerAppEnvUserManagedIdentityId = resourceId('Microsoft.ManagedIdentity/userAssignedIdentities', containerAppEnvUsrMngtIdName)

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
    managedEnvironmentId: managedEnvironmentId
    managedEnvironmentIdentityId: containerAppEnvUserManagedIdentityId
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
    managedEnvironmentId: managedEnvironmentId
    managedEnvironmentIdentityId: containerAppEnvUserManagedIdentityId
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
    managedEnvironmentId: managedEnvironmentId
    managedEnvironmentIdentityId: containerAppEnvUserManagedIdentityId
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
