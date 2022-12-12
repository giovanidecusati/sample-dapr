param keyVaultName string
@secure()
param objectId string
param secrets array

resource resourceKeyVault 'Microsoft.KeyVault/vaults@2021-11-01-preview' existing = {
  name: keyVaultName
}

resource resourceKeyVaultAccessPolicies 'Microsoft.KeyVault/vaults/accessPolicies@2021-11-01-preview' = {
  name: 'add'
  parent: resourceKeyVault
  properties: {
    accessPolicies: [
      {
        objectId: objectId
        permissions: {
          certificates: []
          keys: []
          secrets: secrets
          storage: []
        }
        tenantId: subscription().tenantId
      }
    ]
  }
}
