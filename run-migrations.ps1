# TrackCare - Script para gerar e aplicar migrations
# Instruções:
# 1. Preencha as variáveis abaixo com suas credenciais do Supabase
# 2. Execute: .\run-migrations.ps1

$env:DB_CONNECTION_STRING = "postgresql://postgres:SUA_SENHA@db.ewcldebtsrjfdkckvqin.supabase.co:5432/postgres"
$env:SUPABASE_URL = "https://ewcldebtsrjfdkckvqin.supabase.co"
$env:SUPABASE_KEY = "SUA_CHAVE_AQUI"

# Gerar migrations
dotnet ef migrations add InitialCreate `
    --project src/TrackCare.Infrastructure/TrackCare.Infrastructure.csproj `
    --startup-project src/TrackCare.WebApi/TrackCare.WebApi.csproj `
    --output-dir Migrations

# Aplicar migrations ao banco
dotnet ef database update `
    --project src/TrackCare.Infrastructure/TrackCare.Infrastructure.csproj `
    --startup-project src/TrackCare.WebApi/TrackCare.WebApi.csproj
