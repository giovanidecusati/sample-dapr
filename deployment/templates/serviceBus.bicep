param location string
param standardTags object
param serviceBus object

resource resourceServiceBus 'Microsoft.ServiceBus/namespaces@2022-01-01-preview' = {
  name: serviceBus.name
  location: location
  tags: standardTags
  sku: {    
    name: 'Standard'
    tier: 'Standard'
  }
  properties: {
    minimumTlsVersion: '1.2'
    publicNetworkAccess: 'Enabled'
    zoneRedundant: false
  }
}

var serviceBusEndpoint = '${resourceServiceBus.id}/AuthorizationRules/RootManageSharedAccessKey'
output primaryConnectionString string = listKeys(serviceBusEndpoint, resourceServiceBus.apiVersion).primaryConnectionString
output id string = resourceServiceBus.id
