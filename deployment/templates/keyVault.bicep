param location string
param standardTags object
param constants object
param keyVault object
param logAnalyticsWorkspaceId string

// KeyVault
resource resourceKeyVault 'Microsoft.KeyVault/vaults@2022-07-01' = {
  name: keyVault.name
  location: location
  tags: standardTags
  properties: {
    createMode: 'default'
    enabledForDeployment: false
    enabledForDiskEncryption: false
    enabledForTemplateDeployment: true
    // enablePurgeProtection: false
    enableRbacAuthorization: false
    enableSoftDelete: true
    accessPolicies: []
    networkAcls: {
      bypass: 'AzureServices'
      defaultAction: 'Deny'
      ipRules: []
    }
    publicNetworkAccess: 'enabled'
    sku: {
      family: 'A'
      name: 'standard'
    }
    softDeleteRetentionInDays: 7
    tenantId: tenant().tenantId
  }
}

// KeyVault - diagnosticSettings
resource resourceKeyVaultDiagnostics 'Microsoft.Insights/diagnosticSettings@2021-05-01-preview' = {
  name: constants.diagnosticSettingName
  scope: resourceKeyVault
  properties: {
    workspaceId: logAnalyticsWorkspaceId
    logs: [
      {
        enabled: true
        category: 'AuditEvent'
      }
      {
        enabled: true
        category: 'AzurePolicyEvaluationDetails'
      }
    ]
    metrics: [
      {
        enabled: true
        category: 'AllMetrics'
      }
    ]
  }
}

output id string = resourceKeyVault.id
