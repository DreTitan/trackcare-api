# HealthGo - Clean Architecture Migration

## Estrutura do Projeto

```
HealthGo/
├── src/
│   ├── HealthGo.Domain/          # Entidades, Enums, Interfaces de Repositório
│   ├── HealthGo.Application/     # CQRS (MediatR), DTOs, Validators, AutoMapper
│   ├── HealthGo.Infrastructure/  # EF Core DbContext, Repositories, Supabase Storage
│   └── HealthGo.WebApi/          # Controllers REST, Swagger, Program.cs
├── src-react/
│   └── healthgo-client/          # Vite + React 18 + TypeScript + TailwindCSS
├── tests/
│   ├── HealthGo.Application.Tests/
│   └── HealthGo.Infrastructure.Tests/
├── HealthGo.sln
└── run-migrations.ps1           # Script para gerar e aplicar migrations
```

## Pré-requisitos

- .NET 8.0 SDK
- Node.js 18+
- PostgreSQL (Supabase)

## Configuração

### 1. Variáveis de Ambiente

Crie um arquivo `.env` na raiz do projeto (ou preencha o script `run-migrations.ps1`):

```bash
DB_CONNECTION_STRING=postgresql://postgres:SUA_SENHA@db.ewcldebtsrjfdkckvqin.supabase.co:5432/postgres
SUPABASE_URL=https://ewcldebtsrjfdkckvqin.supabase.co
SUPABASE_KEY=sua_chave_supabase_aqui
```

### 2. Migrations

```powershell
# Preencha as variáveis no script run-migrations.ps1 e execute:
.\run-migrations.ps1

# Ou manualmente:
$env:DB_CONNECTION_STRING="sua_connection_string"
$env:SUPABASE_URL="https://seu_supabase.co"
$env:SUPABASE_KEY="sua_chave"
dotnet ef migrations add InitialCreate --project src/HealthGo.Infrastructure/HealthGo.Infrastructure.csproj --startup-project src/HealthGo.WebApi/HealthGo.WebApi.csproj --output-dir Migrations
dotnet ef database update --project src/HealthGo.Infrastructure/HealthGo.Infrastructure.csproj --startup-project src/HealthGo.WebApi/HealthGo.WebApi.csproj
```

### 3. Backend (.NET)

```bash
cd HealthGo
dotnet restore HealthGo.sln
dotnet build HealthGo.sln
dotnet run --project src/HealthGo.WebApi/HealthGo.WebApi.csproj
# API disponível em http://localhost:5000
# Swagger: http://localhost:5000/swagger
```

### 4. Frontend (React)

```bash
cd src-react/healthgo-client
npm install
npm run dev
# Frontend disponível em http://localhost:5173
```

### 5. Testes

```bash
dotnet test
```

## API Endpoints

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | /api/recolhimentos | Listar todos |
| GET | /api/recolhimentos/{id} | Buscar por ID |
| GET | /api/recolhimentos/search?termo=&status= | Buscar com filtros |
| GET | /api/recolhimentos/dashboard/stats | Métricas do dashboard |
| GET | /api/recolhimentos/recent/{count} | Recentemente criados |
| GET | /api/recolhimentos/{id}/historico | Histórico de status |
| POST | /api/recolhimentos | Criar novo |
| PUT | /api/recolhimentos/{id} | Atualizar |
| PUT | /api/recolhimentos/{id}/status | Alterar status |
| DELETE | /api/recolhimentos/{id} | Excluir |
| GET | /api/anexos/{recolhimentoId} | Listar anexos |
| POST | /api/anexos/{recolhimentoId} | Upload PDF |
| DELETE | /api/anexos/{id} | Excluir anexo |
| GET | /api/comentarios/{recolhimentoId} | Listar comentários |
| POST | /api/comentarios | Adicionar comentário |
| DELETE | /api/comentarios/{id} | Excluir comentário |

## Arquitetura

- **Domain**: Sem dependências externas — entidades puras e interfaces
- **Application**: CQRS via MediatR, validação FluentValidation, mapeamento AutoMapper
- **Infrastructure**: EF Core 8 com PostgreSQL, Supabase Storage para anexos
- **WebApi**: Controllers REST, CORS configurado, Swagger
- **React**: React Router, TanStack Query, TailwindCSS
