using 'main.bicep'

// Example parameters. The password is read from the environment at deployment time and never stored here.
param environmentName = 'dev'
param containerImage = readEnvironmentVariable('RACCORD_IMAGE', 'ghcr.io/juangcarmona/raccord:latest')
param postgresAdminPassword = readEnvironmentVariable('RACCORD_POSTGRES_PASSWORD')
