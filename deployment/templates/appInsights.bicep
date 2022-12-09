param location string
param standardTags object
param appInsights object
param logAnalyticsWorkspaceId string

resource resourceAppInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: appInsights.name
  location: location
  tags: standardTags
  kind: 'web'
  properties: {
    Application_Type: 'web'
    WorkspaceResourceId: logAnalyticsWorkspaceId
  }
}

output id string = resourceAppInsights.id
output ConnectionString string = resourceAppInsights.properties.ConnectionString
