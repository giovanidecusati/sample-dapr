param location string
param standardTags object
param containerRegistry object

resource resourceContainerRegistry 'Microsoft.ContainerRegistry/registries@2022-02-01-preview' = {
  name: containerRegistry.name
  location: location
  tags: standardTags
  sku: {
    name: 'Basic'
  }
  properties: {
    adminUserEnabled: true
    publicNetworkAccess: 'Enabled'
    networkRuleBypassOptions: 'AzureServices'
    zoneRedundancy: 'Disabled'
    anonymousPullEnabled: false    
  }
}

output id string = resourceContainerRegistry.id
output logingServer string = resourceContainerRegistry.properties.loginServer
output password string = resourceContainerRegistry.listCredentials().passwords[0].value
output username string = resourceContainerRegistry.listCredentials().username
