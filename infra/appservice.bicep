@description('Región de Azure donde se desplegarán los recursos.')
param location string = resourceGroup().location

@description('Nombre único global para la Web App.')
param webAppName string

@description('Nombre del App Service Plan.')
param appServicePlanName string = '${webAppName}-plan'

@description('SKU del App Service Plan (por ejemplo: B1, S1, P1v3).')
param appServicePlanSku string = 'B1'

@description('Runtime para Linux App Service. Ejemplo: DOTNETCORE|10.0')
param linuxFxVersion string = 'DOTNETCORE|10.0'

@description('Cadena de conexión de la aplicación. Se inyecta como ConnectionStrings__DefaultConnection.')
param defaultConnectionString string = 'Data Source=/home/data/empresa.db'

@description('Habilita Always On en la Web App.')
param alwaysOn bool = true

@description('Tags opcionales para todos los recursos.')
param tags object = {}

resource appServicePlan 'Microsoft.Web/serverfarms@2023-12-01' = {
  name: appServicePlanName
  location: location
  kind: 'linux'
  tags: tags
  sku: {
    name: appServicePlanSku
    tier: startsWith(appServicePlanSku, 'P') ? 'PremiumV3' : (startsWith(appServicePlanSku, 'S') ? 'Standard' : 'Basic')
    capacity: 1
  }
  properties: {
    reserved: true
  }
}

resource webApp 'Microsoft.Web/sites@2023-12-01' = {
  name: webAppName
  location: location
  kind: 'app,linux'
  tags: tags
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
    clientAffinityEnabled: false
    siteConfig: {
      linuxFxVersion: linuxFxVersion
      alwaysOn: alwaysOn
      ftpsState: 'Disabled'
      minTlsVersion: '1.2'
      appSettings: [
        {
          name: 'ASPNETCORE_ENVIRONMENT'
          value: 'Production'
        }
        {
          name: 'ConnectionStrings__DefaultConnection'
          value: defaultConnectionString
        }
        {
          name: 'WEBSITES_ENABLE_APP_SERVICE_STORAGE'
          value: 'true'
        }
      ]
    }
  }
}

output appServicePlanResourceId string = appServicePlan.id
output webAppResourceId string = webApp.id
output webAppDefaultHostName string = webApp.properties.defaultHostName
