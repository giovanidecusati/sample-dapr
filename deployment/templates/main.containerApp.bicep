@allowed([
  'prd'
  'uat'
  'dev'
  'lab'
])
param environmentName string = 'dev'
param solutionName string = 'dapr'
param keyVaultName string = 'kv-giodapr-lab-ause'
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

var containerAppEnvironment = {
  name: 'cae-${solutionName}-${environmentName}-aue'
  location: 'australiaeast'
}

// ##################################################################
//  Modules
// ##################################################################

resource resourceKeyVault 'Microsoft.KeyVault/vaults@2022-07-01' existing = {
  name: keyVaultName
}

module moduleContainerAppEnvironment './containerAppEnvironment.bicep' = {
  name: 'containerAppEnvironment-${buildId}'
  dependsOn: []
  params: {
    location: containerAppEnvironment.location
    standardTags: standardTags
    containerAppEnvironment: containerAppEnvironment
    logAnalyticsCustomerId: resourceKeyVault.getSecret('logAnalyticsCustomerId')
    logAnalyticsPrimarySharedKey: resourceKeyVault.getSecret('logAnalyticsPrimarySharedKey')
  }
}

// // Key-vault secret: servicebus-connectionString
// module moduleAkvSecret_servicebus_connectionString './keyVault.secret.bicep' = {
//   name: 'akvSecret_servicebus-connectionString-${buildId}'
//   dependsOn: [
//     moduleKeyVault
//     moduleContainerRegistry
//   ]
//   params: {
//     keyVaultName: keyVault.name
//     name: 'servicebus-connectionString'
//     secretValue: moduleServiceBus.outputs.primaryConnectionString
//     contentType: 'plain/text'
//     tags: {
//       CredentialId: 'connectionString'
//       ProviderAddress: moduleServiceBus.outputs.id
//       ValidityPeriodDays: -1
//     }
//   }
// }

output containerAppEnvironment object = {
  id: moduleContainerAppEnvironment.outputs.id
  name: containerAppEnvironment.name
}
