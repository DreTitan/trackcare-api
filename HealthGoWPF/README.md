# HealthGo Collection Manager - WPF Desktop

Sistema de gestão de recolhimento de equipamentos médicos para Windows.

## Requisitos

- Windows 10/11
- [.NET 8 Runtime](https://dotnet.microsoft.com/download/dotnet/8.0)

## Como Executar

1. Navegue até a pasta do projeto:
   ```
   cd HealthGoWPF
   ```

2. Restaure as dependências:
   ```
   dotnet restore
   ```

3. Execute o projeto:
   ```
   dotnet run
   ```

   Ou compile e abra o .exe em:
   ```
   bin\Debug\net8.0-windows\HealthGoWPF.exe
   ```

## Funcionalidades

- Dashboard com métricas
- Cadastro de recolhimentos
- Atualização de status
- Busca e filtros
- Interface profissional Windows

## Estrutura

```
HealthGoWPF/
├── Models/           # Modelos de dados
├── Views/            # Janelas XAML
├── Services/        # Lógica de negócio
├── Data/            # Banco de dados
└── App.xaml         # Recursos e estilos
```

## Tecnologias

- .NET 8
- WPF (Windows Presentation Foundation)
- Entity Framework Core (SQLite)
- CommunityToolkit.Mvvm
