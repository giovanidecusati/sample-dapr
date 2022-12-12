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

module moduleContainerAppEnvironment './containerAppEnvironment.bicep' = {
  name: 'containerAppEnvironment-${buildId}'
  dependsOn: []
  params: {
    location: containerAppEnvironment.location
    standardTags: standardTags
    containerAppEnvironment: containerAppEnvironment
    logAnalyticsCustomerId: resourceKeyVault.getSecret('logAnalyticsWorkspaceCustomerId')
    logAnalyticsPrimarySharedKey: resourceKeyVault.getSecret('logAnalyticsWorkspacePrimarySharedKey')
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
