param location string
param standardTags object
param logAnalyticsWorkspace object

resource resourceLogAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2021-12-01-preview' = {
  location: location
  tags: standardTags
  name: logAnalyticsWorkspace.name
  properties: {
    sku: {
      name: 'PerGB2018'
    }
    retentionInDays: logAnalyticsWorkspace.logRetentionDays
    features: {
      enableLogAccessUsingOnlyResourcePermissions: true
    }
    workspaceCapping: {
      dailyQuotaGb: -1
    }
    publicNetworkAccessForIngestion: 'Enabled'
    publicNetworkAccessForQuery: 'Enabled'
  }
}

output id string = resourceLogAnalyticsWorkspace.id
output customerId string =  resourceLogAnalyticsWorkspace.properties.customerId
output primarySharedKey string =  resourceLogAnalyticsWorkspace.listKeys().primarySharedKey
