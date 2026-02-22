using './appservice.bicep'

param webAppName = 'empresa-api-prod-001'
param appServicePlanName = 'empresa-api-prod-plan'
param appServicePlanSku = 'S1'
param linuxFxVersion = 'DOTNETCORE|10.0'
param defaultConnectionString = 'Data Source=/home/data/empresa.db'
param alwaysOn = true
param tags = {
  environment: 'prod'
  project: 'backend-empresa'
}
