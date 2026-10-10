@description('Key Vault name (3-24 characters).')
param name string
param location string
param tags object

@description('Principal allowed to read secrets (the application identity).')
param principalId string

@secure()
param databaseConnectionString string

resource vault 'Microsoft.KeyVault/vaults@2023-07-01' = {
  name: name
  location: location
  tags: tags
  properties: {
    tenantId: subscription().tenantId
    sku: { family: 'A', name: 'standard' }
    enableRbacAuthorization: true
    enableSoftDelete: true
    softDeleteRetentionInDays: 30
    enablePurgeProtection: true
  }
}

resource databaseSecret 'Microsoft.KeyVault/vaults/secrets@2023-07-01' = {
  parent: vault
  name: 'database-connection-string'
  properties: { value: databaseConnectionString }
}

var keyVaultSecretsUser = '4633458b-17de-408a-b874-0445c86b69e6'

resource secretAccess 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  scope: vault
  name: guid(vault.id, principalId, keyVaultSecretsUser)
  properties: {
    principalId: principalId
    principalType: 'ServicePrincipal'
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', keyVaultSecretsUser)
  }
}

output databaseSecretUri string = databaseSecret.properties.secretUri
