@allowed([
  'prd'
  'uat'
  'dev'
  'lab'
])
param environmentName string = 'dev'
param solutionName string = 'dapr'
param location string = resourceGroup().location
param buildId string
param baseTime string = utcNow('u')

var standardTags = {
  environment: environmentName
  solutionName: solutionName
  department: 'IT'
  businessOwner: 'giovani'
  technicalOwner: 'giovani'
}

var constants = {
  nonprod: [
    'dev'
    'lab'
  ]
  diagnosticSettingName: 'defaultDiagnosticSettings'
  dataCenterCode: 'ause'
}

var logAnalyticsWorkspace = {
  name: 'log-${solutionName}-${environmentName}-${constants.dataCenterCode}'
  logRetentionDays: contains(constants.nonprod, environmentName) ? 30 : 90
}

var appInsights = {
  name: 'appi-${solutionName}-${environmentName}-${constants.dataCenterCode}'
}

var containerRegistry = {
  name: 'cr${solutionName}${environmentName}${constants.dataCenterCode}'
}

var keyVault = {
  name: 'kv-${solutionName}-${environmentName}-${constants.dataCenterCode}'
}

// ##################################################################
//  Modules
// ##################################################################

module moduleLogAnalyticsWorkspace './logAnalyticsWorkspace.bicep' = {
  name: 'logAnalyticsWorkspace-${buildId}'
  params: {
    location: location
    standardTags: standardTags
    logAnalyticsWorkspace: logAnalyticsWorkspace
  }
}

module moduleAppInsights './appInsights.bicep' = {
  name: 'appInsights-${buildId}'
  dependsOn: [ ]
  params: {
    location: location
    standardTags: standardTags
    appInsights: appInsights
    logAnalyticsWorkspaceId: moduleLogAnalyticsWorkspace.outputs.id
  }
}

module moduleKeyVault './keyVault.bicep' = {
  name: 'keyVault-${buildId}'
  dependsOn: [
    moduleLogAnalyticsWorkspace
  ]
  params: {
    location: location
    standardTags: standardTags
    constants: constants
    keyVault: keyVault
    logAnalyticsWorkspaceId: moduleLogAnalyticsWorkspace.outputs.id
  }
}

module moduleKeyVaultAccessPolicy './keyVault.accessPolicies.bicep' = {
  name: 'keyVaultAccessPolicy-${buildId}'
  dependsOn: [
    moduleKeyVault
  ]
  params: {
    keyVaultName: keyVault.name
  }
}

module moduleContainerRegistry './containerRegistry.bicep' = {
  name: 'containerRegistry-${buildId}'
  dependsOn: []
  params: {
    location: location
    standardTags: standardTags
    containerRegistry: containerRegistry
  }
}

// Key-vault secret: acrLoginServer
module moduleAkvSecret_acrLoginServer './keyVault.secret.bicep' = {
  name: 'akvSecret_acrLoginServer-${buildId}'
  dependsOn: [
    moduleKeyVault
    moduleContainerRegistry
  ]
  params: {
    keyVaultName: keyVault.name
    name: 'acrLoginServer'
    secretValue: moduleContainerRegistry.outputs.logingServer
    contentType: 'plain/text'
    tags: {
      CredentialId: 'loginServer'
      ProviderAddress: moduleContainerRegistry.outputs.id
      ValidityPeriodDays: -1
    }
  }
}

// Key-vault secret: acrPassword
module moduleAkvSecret_acrPassword './keyVault.secret.bicep' = {
  name: 'akvSecret_acrPassword-${buildId}'
  dependsOn: [
    moduleKeyVault
    moduleContainerRegistry
  ]
  params: {
    keyVaultName: keyVault.name
    name: 'acrPassword'
    secretValue: moduleContainerRegistry.outputs.password
    contentType: 'plain/text'
    tags: {
      CredentialId: 'password'
      ProviderAddress: moduleContainerRegistry.outputs.id
      ValidityPeriodDays: 365
    }
    expiryDate: '${dateTimeToEpoch(dateTimeAdd(baseTime, 'P1Y'))}'
  }
}

// Key-vault secret: acrUserName
module moduleAkvSecret_acrUserName './keyVault.secret.bicep' = {
  name: 'akvSecret_acrUserName-${buildId}'
  dependsOn: [
    moduleKeyVault
    moduleContainerRegistry
  ]
  params: {
    keyVaultName: keyVault.name
    name: 'acrUserName'
    secretValue: moduleContainerRegistry.outputs.username
    contentType: 'plain/text'
    tags: {
      CredentialId: 'username'
      ProviderAddress: moduleContainerRegistry.outputs.id
      ValidityPeriodDays: -1
    }
  }
}

// Key-vault secret: logAnalyticsWorkspaceCustomerId
module moduleAkvSecret_logAnalyticsWorkspaceCustomerId './keyVault.secret.bicep' = {
  name: 'akvSecret_logAnalyticsWorkspaceCustomerId-${buildId}'
  dependsOn: [
    moduleKeyVault
    moduleContainerRegistry
  ]
  params: {
    keyVaultName: keyVault.name
    name: 'logAnalyticsWorkspaceCustomerId'
    secretValue: moduleLogAnalyticsWorkspace.outputs.customerId
    contentType: 'plain/text'
    tags: {
      CredentialId: 'customerId'
      ProviderAddress: moduleLogAnalyticsWorkspace.outputs.customerId
      ValidityPeriodDays: -1
    }
  }
}

// Key-vault secret: logAnalyticsWorkspacePrimarySharedKey
module moduleAkvSecret_logAnalyticsWorkspacePrimarySharedKey './keyVault.secret.bicep' = {
  name: 'akvSecret_logAnalyticsWorkspacePrimarySharedKey-${buildId}'
  dependsOn: [
    moduleKeyVault
    moduleContainerRegistry
  ]
  params: {
    keyVaultName: keyVault.name
    name: 'logAnalyticsWorkspacePrimarySharedKey'
    secretValue: moduleLogAnalyticsWorkspace.outputs.primarySharedKey
    contentType: 'plain/text'
    tags: {
      CredentialId: 'primarySharedKey'
      ProviderAddress: moduleLogAnalyticsWorkspace.outputs.primarySharedKey
      ValidityPeriodDays: -1
    }
  }
}

output logAnalyticsWorkspace object = {
  id: moduleLogAnalyticsWorkspace.outputs.id
  name: logAnalyticsWorkspace.name
}

output keyVault object = {
  id: moduleKeyVault.outputs.id
  name: keyVault.name
}

output containerRegistry object = {
  id: moduleContainerRegistry.outputs.id
  name: containerRegistry.name
}
