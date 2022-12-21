param location string
param standardTags object
param containerRegistry object
param userManagedIdentityPrincipalId string

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

@description('This is the built-in "AcrPull" role. See https://docs.microsoft.com/en-us/azure/role-based-access-control/built-in-roles#acrpull ')
resource acrPullRoleDefinition 'Microsoft.Authorization/roleDefinitions@2022-04-01' existing = {
  scope: subscription()
  name: '7f951dda-4ed3-4680-a7ca-43fe172d538d'
}



// Allows the service to pull images from the Azure Container Registry
resource svcUserAcrPull 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid('acrPull', userManagedIdentityPrincipalId)
  scope: resourceContainerRegistry
  properties: {
    roleDefinitionId: acrPullRoleDefinition.id
    principalId: userManagedIdentityPrincipalId
    principalType: 'ServicePrincipal'
  }
}

output id string = resourceContainerRegistry.id
output logingServer string = resourceContainerRegistry.properties.loginServer
output password string = resourceContainerRegistry.listCredentials().passwords[0].value
output username string = resourceContainerRegistry.listCredentials().username
