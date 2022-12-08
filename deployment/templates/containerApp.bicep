param location string
param standardTags object
param containerApp object
param managedEnvironmentId string
@secure()
param acrServer string
@secure()
param acrUserName string
@secure()
param acrPassword string

var passwordSecretRef = 'acr-password-secret'

resource resourceContainerApp 'Microsoft.App/containerApps@2022-06-01-preview' = {
  name: containerApp.name
  location: location
  tags: standardTags
  properties: {
    managedEnvironmentId: managedEnvironmentId
    configuration: {
      dapr: {
        appId: containerApp.appId
        appPort: 443
        appProtocol: 'http'
        enableApiLogging: true
        enabled: true
        httpMaxRequestSize: 4
        httpReadBufferSize: 4
        logLevel: 'debug'
      }
      ingress: {
        external: false
        transport: 'auto'
        allowInsecure: false
        targetPort: 443
      }
      registries: [
        {
          passwordSecretRef: passwordSecretRef
          server: acrServer
          username: acrUserName
        }
      ]
      secrets: [
        {
          name: passwordSecretRef
          value: acrPassword
        }
      ]
    }
    template: {
      containers: [ {
          image: containerApp.image
          name: containerApp.name
          command: []
          resources: {
            cpu: 1
            memory: '2Gi'
          }
          env: [
            {
              name: 'ASPNETCORE_ENVIRONMENT'
              value: 'Development'
            }
          ]
        } ]
    }
  }
}

output fqdn string = resourceContainerApp.properties.configuration.ingress.fqdn
output id string = resourceContainerApp.id
