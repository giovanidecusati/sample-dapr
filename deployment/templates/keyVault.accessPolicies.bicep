param keyVaultName string

resource resourceKeyVault 'Microsoft.KeyVault/vaults@2021-11-01-preview' existing = {
  name: keyVaultName
}

resource resourceKeyVaultAccessPolicies 'Microsoft.KeyVault/vaults/accessPolicies@2021-11-01-preview' = {
  name: 'add'
  parent: resourceKeyVault
  properties: {
    accessPolicies: [
      {
        // azuredevops service principal to integrate AKV with Environment Variables Group
        objectId: '8235c2d5-546b-44bd-863f-28d2ca81041a'
        permissions: {
          certificates: []
          keys: []
          secrets: [
            'get'
            'list'
          ]
          storage: []
        }
        tenantId: subscription().tenantId
      }
    ]
  }
}
