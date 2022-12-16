param location string
param standardTags object
param managedIdentity object

resource resourceUserManagedIdentity 'Microsoft.ManagedIdentity/userAssignedIdentities@2022-01-31-preview' = {
  name: managedIdentity.name
  location: location
  tags:standardTags
}

output id string = resourceUserManagedIdentity.id
output principalId string = resourceUserManagedIdentity.properties.principalId
