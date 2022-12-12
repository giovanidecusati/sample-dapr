param location string
param standardTags object
param cosmosDbName string
param databaseName string
param containerNames array

resource resourceCosmosDb 'Microsoft.DocumentDB/databaseAccounts@2022-08-15' existing = {
  name: cosmosDbName
}

resource resourceDatabase 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases@2022-08-15' existing = {
  name: databaseName
  parent: resourceCosmosDb
}

resource resourceDatabaseContainers 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2022-08-15' = [for container in containerNames: {
  name: container
  location: location
  tags: standardTags
  parent: resourceDatabase
  properties: {
    options: {}
    resource: {
      id: container
      partitionKey: {
        kind: 'Hash'
        paths: [
          '/partitionKey'
        ]
      }
    }
  }
}]
