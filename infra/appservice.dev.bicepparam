using './appservice.bicep'

param webAppName = 'empresa-api-dev-001'
param appServicePlanName = 'empresa-api-dev-plan'
param appServicePlanSku = 'B1'
param linuxFxVersion = 'DOTNETCORE|10.0'
param defaultConnectionString = 'Data Source=/home/data/empresa-dev.db'
param alwaysOn = false
param tags = {
  environment: 'dev'
  project: 'backend-empresa'
}
