param environmentName string
param location string
param tags object
param containerImage string

@description('Resource ID of the user-assigned identity used for Key Vault and Blob Storage.')
param identityId string
param identityClientId string
param logAnalyticsWorkspaceName string
param databaseSecretUri string
param blobEndpoint string
param timeZone string

resource workspace 'Microsoft.OperationalInsights/workspaces@2023-09-01' existing = {
  name: logAnalyticsWorkspaceName
}

resource environment 'Microsoft.App/managedEnvironments@2024-03-01' = {
  name: 'cae-raccord-${environmentName}'
  location: location
  tags: tags
  properties: {
    appLogsConfiguration: {
      destination: 'log-analytics'
      logAnalyticsConfiguration: {
        customerId: workspace.properties.customerId
        sharedKey: workspace.listKeys().primarySharedKey
      }
    }
  }
}

var secrets = [
  { name: 'database', keyVaultUrl: databaseSecretUri, identity: identityId }
]

var settings = [
  { name: 'ConnectionStrings__raccord', secretRef: 'database' }
  { name: 'ConnectionStrings__documents', value: blobEndpoint }
  { name: 'Storage__Provider', value: 'AzureBlob' }
  { name: 'AZURE_CLIENT_ID', value: identityClientId }
  { name: 'Raccord__TimeZone', value: timeZone }
]

resource web 'Microsoft.App/containerApps@2024-03-01' = {
  name: 'ca-raccord-${environmentName}'
  location: location
  tags: tags
  identity: {
    type: 'UserAssigned'
    userAssignedIdentities: { '${identityId}': {} }
  }
  properties: {
    managedEnvironmentId: environment.id
    configuration: {
      secrets: secrets
      ingress: {
        external: true
        targetPort: 8080
        transport: 'auto'
        allowInsecure: false
      }
    }
    template: {
      containers: [
        {
          name: 'web'
          image: containerImage
          env: settings
          resources: { cpu: json('0.5'), memory: '1Gi' }
          probes: [
            { type: 'Liveness', httpGet: { path: '/health/live', port: 8080 }, periodSeconds: 30 }
            { type: 'Readiness', httpGet: { path: '/health/ready', port: 8080 }, periodSeconds: 10 }
          ]
        }
      ]
      // One replica keeps invoice numbering contention trivial; scale out only after load testing.
      scale: { minReplicas: 1, maxReplicas: 1 }
    }
  }
}

resource migrations 'Microsoft.App/jobs@2024-03-01' = {
  name: 'caj-raccord-migrate-${environmentName}'
  location: location
  tags: tags
  identity: {
    type: 'UserAssigned'
    userAssignedIdentities: { '${identityId}': {} }
  }
  properties: {
    environmentId: environment.id
    configuration: {
      triggerType: 'Manual'
      replicaTimeout: 600
      replicaRetryLimit: 1
      manualTriggerConfig: { parallelism: 1, replicaCompletionCount: 1 }
      secrets: secrets
    }
    template: {
      containers: [
        {
          name: 'migrate'
          image: containerImage
          args: [ 'migrate' ]
          env: settings
          resources: { cpu: json('0.5'), memory: '1Gi' }
        }
      ]
    }
  }
}

output fqdn string = web.properties.configuration.ingress.fqdn
output migrationJobName string = migrations.name
