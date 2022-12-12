param location string
param standardTags object
param cosmosDb object

resource resourceCosmosDb 'Microsoft.DocumentDB/databaseAccounts@2022-08-15' = {
  name: cosmosDb.name
  location: location
  tags: standardTags
  kind: 'GlobalDocumentDB'
  properties: {
    databaseAccountOfferType: 'Standard'
    locations: cosmosDb.locations
    backupPolicy: {
      type: 'Periodic'
      periodicModeProperties: {
        backupIntervalInMinutes: 240
        backupRetentionIntervalInHours: 8
        backupStorageRedundancy: 'Local'
      }
    }
    isVirtualNetworkFilterEnabled: false
    virtualNetworkRules: []
    ipRules: []
    capabilities: [
      {
        name: 'EnableServerless'
      }
    ]
    enableFreeTier: false
    capacity: {
      totalThroughputLimit: cosmosDb.totalThroughputLimit
    }
  }
}

resource resourceDatabases 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases@2022-08-15' = [for database in cosmosDb.databases: {
  name: database.name
  location: location
  tags: standardTags
  parent: resourceCosmosDb
  properties: {
    resource: {
      id: database.name
    }
  }
}]

output id string = resourceCosmosDb.id
output primaryMasterKey string = resourceCosmosDb.listKeys().primaryMasterKey
output documentEndpoint string = resourceCosmosDb.properties.documentEndpoint
