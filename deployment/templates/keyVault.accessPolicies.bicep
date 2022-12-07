param keyVaultName string
param webAppServiceObjectId string
param apiAppServiceObjectId string
param syncAppServiceObjectId string

resource resourceKeyVault 'Microsoft.KeyVault/vaults@2021-11-01-preview' existing = {
  name: keyVaultName
}

resource resourceKeyVaultAccessPolicies 'Microsoft.KeyVault/vaults/accessPolicies@2021-11-01-preview' = {
  name: 'replace'
  parent: resourceKeyVault
  properties: {
    accessPolicies: [
      {
        objectId: webAppServiceObjectId
        permissions: {
          certificates: []
          keys: []
          secrets: [
            'get'
          ]
          storage: []
        }
        tenantId: subscription().tenantId
      }
      {
        objectId: apiAppServiceObjectId
        permissions: {
          certificates: []
          keys: []
          secrets: [
            'get'
          ]
          storage: []
        }
        tenantId: subscription().tenantId
      }
      {
        objectId: syncAppServiceObjectId
        permissions: {
          certificates: []
          keys: []
          secrets: [
            'get'
          ]
          storage: []
        }
        tenantId: subscription().tenantId
      }
    ]
  }
}
