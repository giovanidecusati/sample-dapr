param location string
param standardTags object
param containerAppEnvironment object
@secure()
param logAnalyticsCustomerId string
@secure()
param logAnalyticsPrimarySharedKey string

resource resourceContainerAppEnvironment 'Microsoft.App/managedEnvironments@2022-06-01-preview' = {
  name: containerAppEnvironment.name
  location: location
  tags: standardTags
  sku: {
    name: 'Consumption'
  }
  properties: {
    appLogsConfiguration: {
      destination: 'log-analytics'
      logAnalyticsConfiguration: {
        customerId: logAnalyticsCustomerId
        sharedKey: logAnalyticsPrimarySharedKey
      }
    }
    zoneRedundant: false
  }
}

output id string = resourceContainerAppEnvironment.id
