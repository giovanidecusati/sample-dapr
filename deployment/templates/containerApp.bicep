param location string
param standardTags object
param containerApp object
param managedEnvironmentId string
@secure()
param appInsightsConnectionString string
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
      activeRevisionsMode:'Single'
      dapr: {
        appId: containerApp.appId
        appPort: 80
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
        targetPort: 80
        traffic: [
          {
            latestRevision: true
            weight: 100
          }
        ]
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
          probes: [
            {
              type: 'Readiness'
              httpGet: {
                port: 80
                path: '/api/health'
                scheme: 'HTTP'
              }
              periodSeconds: 240
              timeoutSeconds: 5
              initialDelaySeconds: 5
              successThreshold: 1
              failureThreshold: 3
            }
          ]
          env: [
            {
              name: 'ASPNETCORE_ENVIRONMENT'
              value: 'Production'
            }
            {
              name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
              value: appInsightsConnectionString
            }
          ]
        } ]
      scale: {
        minReplicas: 1
        maxReplicas: 2
      }
    }
  }
}

output fqdn string = resourceContainerApp.properties.configuration.ingress.fqdn
output id string = resourceContainerApp.id
