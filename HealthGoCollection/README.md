# HealthGo Collection Manager

Sistema de gestão de recolhimento de equipamentos médicos.

## Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Windows 10/11 ou Linux/macOS

## Instalação do .NET 8

1. Baixe o .NET 8 SDK em: https://dotnet.microsoft.com/download/dotnet/8.0
2. Execute o instalador e siga as instruções
3. Após instalar, reinicie o terminal e verifique com:
   ```
   dotnet --version
   ```

## Executando o Projeto

1. Navegue até a pasta do projeto:
   ```bash
   cd HealthGoCollection
   ```

2. Restaure as dependências:
   ```bash
   dotnet restore
   ```

3. Execute o projeto:
   ```bash
   dotnet run
   ```

4. Abra o navegador em: https://localhost:5001 ou http://localhost:5000

## Login Padrão

- **Usuário:** admin
- **Senha:** admin123

## Funcionalidades

- Dashboard com métricas
- Cadastro de recolhimentos
- Timeline visual do processo
- Upload de anexos (PDFs, fotos)
- Histórico de alterações
- Busca e filtros

## Estrutura do Projeto

```
HealthGoCollection/
├── Components/         # Componentes reutilizáveis
├── Data/              # DbContext e configurações
├── Models/            # Modelos de dados
├── Pages/             # Páginas Razor
├── Services/          # Serviços de negócio
├── Shared/            # Layouts e componentes compartilhados
└── wwwroot/           # Arquivos estáticos
```

## Tecnologias

- .NET 8
- Blazor Server
- Entity Framework Core (SQLite)
- MudBlazor (UI Components)

## Autor

HealthGo - Sistema de Gestão de Recolhimento
