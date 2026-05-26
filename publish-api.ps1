# TrackCare - Script de Publicação para Azure App Service
# Instruções:
# 1. Preencha as variáveis abaixo com os dados do seu Azure
# 2. Execute: .\publish-api.ps1

param(
    [string]$ResourceGroup = "trackcare-rg",
    [string]$AppName = "trackcare-api",
    [string]$Subscription = "SUA_ASSINATURA"
)

# Variáveis - preencha com seus dados do Azure
$env:DB_CONNECTION_STRING = "postgresql://postgres:ewcldebtsrjfdkckvqin@db.ewcldebtsrjfdkckvqin.supabase.co:5432/postgres"
$env:SUPABASE_URL = "https://ewcldebtsrjfdkckvqin.supabase.co"
$env:SUPABASE_KEY = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImV3Y2xkZWJ0c3JqZmRrY2t2cWluIiwicm9sZSI6ImFub24iLCJpYXQiOjE3Nzg4MTcxNzksImV4cCI6MjA5NDM5MzE3OX0.PhQPmhTrhNMGRLEtrOBAc-9heIf451rKkL0KzPu7r4s"

# Selecionar subscription
if (-not [string]::IsNullOrEmpty($Subscription)) {
    az account set --subscription $Subscription
}

Write-Host "Publicando TrackCare API..." -ForegroundColor Cyan

# Publicar
dotnet publish src/TrackCare.WebApi/TrackCare.WebApi.csproj -c Release -o ./publish

# Criar zip
Compress-Archive -Path ./publish/* -DestinationPath ./publish.zip -Force

# Deploy
Write-Host " Fazendo deploy no Azure..." -ForegroundColor Yellow
az webapp deploy `
    --resource-group $ResourceGroup `
    --name $AppName `
    --src-path ./publish.zip `
    --type zip

# Configurar variáveis de ambiente
Write-Host " Configurando variáveis de ambiente..." -ForegroundColor Yellow
az webapp config appsettings set `
    --resource-group $ResourceGroup `
    --name $AppName `
    --settings `
        DB_CONNECTION_STRING="$env:DB_CONNECTION_STRING" `
        SUPABASE_URL="$env:SUPABASE_URL" `
        SUPABASE_KEY="$env:SUPABASE_KEY" `
        ASPNETCORE_ENVIRONMENT="Production" `
        FRONTEND_URL="*"

Write-Host " Deploy concluído!" -ForegroundColor Green
Write-Host " API disponível em: https://$AppName.azurewebsites.net" -ForegroundColor Green
Write-Host " Swagger: https://$AppName.azurewebsites.net/swagger" -ForegroundColor Green
