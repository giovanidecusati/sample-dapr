param location string
param standardTags object
param containerApp object
param managedEnvironmentId string

resource resourceContainerApp 'Microsoft.App/containerApps@2022-06-01-preview' = {
  name: containerApp.name
  location: location
  tags: standardTags
  properties: {
    managedEnvironmentId: managedEnvironmentId
  }
}

output fqdn string = resourceContainerApp.properties.configuration.ingress.fqdn
