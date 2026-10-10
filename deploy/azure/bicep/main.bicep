// Raccord on Azure: Container Apps (web + migration job), PostgreSQL Flexible Server, Blob Storage,
// Key Vault for the database connection string, and Log Analytics. Deploy at resource-group scope.
targetScope = 'resourceGroup'

@description('Short environment name used in resource names, e.g. dev or prod.')
@minLength(2)
@maxLength(8)
param environmentName string

@description('Azure region; defaults to the resource group location.')
param location string = resourceGroup().location

@description('Container image of the Raccord web application, e.g. ghcr.io/<owner>/raccord:<tag>.')
param containerImage string

@description('PostgreSQL administrator login.')
param postgresAdminLogin string = 'raccordadmin'

@description('PostgreSQL administrator password. Provide it at deployment time; never commit it.')
@secure()
param postgresAdminPassword string

@description('IANA time zone used to determine the default invoice issue date.')
param timeZone string = 'Europe/Madrid'

var suffix = uniqueString(resourceGroup().id, environmentName)
var tags = {
  application: 'raccord'
  environment: environmentName
}

module monitoring 'modules/monitoring.bicep' = {
  name: 'monitoring'
  params: {
    name: 'log-raccord-${environmentName}-${suffix}'
    location: location
    tags: tags
  }
}

resource identity 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-01-31' = {
  name: 'id-raccord-${environmentName}'
  location: location
  tags: tags
}

module storage 'modules/storage.bicep' = {
  name: 'storage'
  params: {
    name: take('straccord${environmentName}${suffix}', 24)
    location: location
    tags: tags
    principalId: identity.properties.principalId
  }
}

module database 'modules/postgres.bicep' = {
  name: 'postgres'
  params: {
    name: 'psql-raccord-${environmentName}-${suffix}'
    location: location
    tags: tags
    administratorLogin: postgresAdminLogin
    administratorPassword: postgresAdminPassword
  }
}

module secrets 'modules/keyvault.bicep' = {
  name: 'keyvault'
  params: {
    name: take('kv-raccord-${environmentName}-${suffix}', 24)
    location: location
    tags: tags
    principalId: identity.properties.principalId
    databaseConnectionString: 'Host=${database.outputs.fqdn};Port=5432;Database=raccord;Username=${postgresAdminLogin};Password=${postgresAdminPassword};SSL Mode=Require'
  }
}

module apps 'modules/containerapps.bicep' = {
  name: 'containerapps'
  params: {
    environmentName: environmentName
    location: location
    tags: tags
    containerImage: containerImage
    identityId: identity.id
    identityClientId: identity.properties.clientId
    logAnalyticsWorkspaceName: monitoring.outputs.workspaceName
    databaseSecretUri: secrets.outputs.databaseSecretUri
    blobEndpoint: storage.outputs.blobEndpoint
    timeZone: timeZone
  }
}

@description('Public URL of the application.')
output applicationUrl string = 'https://${apps.outputs.fqdn}'

@description('Name of the migration job; start it after every deployment that changes the image.')
output migrationJobName string = apps.outputs.migrationJobName
