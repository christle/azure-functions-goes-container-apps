param location string = 'northeurope'
param registry string
param registryUsername string

@secure()
param registryPassword string

@secure()
param connectionString string

@secure()
param storageConnectionString string

@secure()
param instrumentationKey string

resource law 'Microsoft.OperationalInsights/workspaces@2020-03-01-preview' = {
  name: 'capps-workspace'
  location: location
  properties: any({
    retentionInDays: 30
    features: {
      searchVersion: 1
    }
    sku: {
      name: 'PerGB2018'
    }
  })
}

resource env 'Microsoft.Web/kubeEnvironments@2021-02-01' = {
  name: 'capps-env'
  location: location
  properties: {
    type: 'managed'
    internalLoadBalancerEnabled: false
    appLogsConfiguration: {
      destination: 'log-analytics'
      logAnalyticsConfiguration: {
        customerId: law.properties.customerId
        sharedKey: law.listKeys().primarySharedKey
      }
    }
  }
}

resource positionHandlerApp 'Microsoft.Web/containerApps@2021-03-01' = {
  name: 'position-handler-capp'
  kind: 'containerapp'
  location: location
  properties: {
    kubeEnvironmentId: env.id
    configuration: {
      secrets: [
        {
          name: 'container-registry-password'
          value: registryPassword
        }
        {
          name: 'eventhub-connectionstring'
          value: connectionString
        }
        {
          name: 'storage-connectionstring'
          value: storageConnectionString
        }
        {
          name: 'instrumentation-key'
          value: instrumentationKey
        }
      ]      
      registries: [
        {
          server: registry
          username: registryUsername
          passwordSecretRef: 'container-registry-password'
        }
      ]
    }
    template: {
      containers: [
        {
          image: '${registry}/mc/capp-position-handler:1.0.0'
          name: 'position-handler-capp'
          env: [
          {
            name: 'positionEventhubConnection'
            secretRef: 'eventhub-connectionstring'
          }
          {
            name: 'AzureWebJobsStorage'
            secretRef: 'storage-connectionstring'
          }
          {
            name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
            secretRef: 'instrumentation-key'
          }
          {
            name: 'WORKER_API'
            value: workerApiApp.properties.configuration.ingress.fqdn
          }
          ]
        }
      ]
      scale: {
        minReplicas: 0
        maxReplicas: 5  
        rules: [
          {
            name: 'azure-eventhub'
            custom: {
              type: 'azure-eventhub'
              metadata: {
                eventHubName: 'hub-positions'
                consumerGroup: 'capp-consumer-group'
              }
              auth: [
              {
                secretRef: 'eventhub-connectionstring'
                triggerParameter: 'connection'
              }
              {
                secretRef: 'storage-connectionstring'
                triggerParameter: 'storageConnection'
              }
              ]
            }
          }
        ]
      }
    }
  }
}

resource workerApiApp 'Microsoft.Web/containerApps@2021-03-01' = {
  name: 'worker-api-capp'
  kind: 'containerapp'
  location: location
  properties: {
    kubeEnvironmentId: env.id
    configuration: {
      secrets: [
        {
          name: 'container-registry-password'
          value: registryPassword
        }
        {
          name: 'instrumentation-key'
          value: instrumentationKey
        }
      ]      
      registries: [
        {
          server: registry
          username: registryUsername
          passwordSecretRef: 'container-registry-password'
        }
      ]
      ingress: {
        external: false
        targetPort: 80
      }
    }
    template: {
      containers: [
        {
          image: '${registry}/mc/capp-worker-api:1.0.0'
          name: 'worker-api-capp'
          env: [
          {
            name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
            secretRef: 'instrumentation-key'
          }
          ]
        }
      ]
      scale: {
        minReplicas: 0
      }
    }
  }
}
