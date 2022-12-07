param keyVaultName string
param name string
@secure()
param secretValue string
param contentType string
param tags object
param expiryDate string = '-1'
param notBeforeDate string = '-1'

var expiryAttributes = {
  exp: expiryDate
  nbf: notBeforeDate
}

var standardAttributes = {

}

resource resourceKeyVault 'Microsoft.KeyVault/vaults@2022-07-01' existing = {
  name: keyVaultName
}

resource resourceKeyVaultSecretSecret 'Microsoft.KeyVault/vaults/secrets@2022-07-01' = {
  name: name
  tags: tags
  parent: resourceKeyVault
  properties: {
    contentType: contentType
    value: secretValue
    attributes: (expiryDate == '-1' ? standardAttributes : expiryAttributes)
  }
}

output secretUriWithVersion string = resourceKeyVaultSecretSecret.properties.secretUriWithVersion
