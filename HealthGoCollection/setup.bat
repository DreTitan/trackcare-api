@echo off
echo ================================================
echo   HealthGo Collection Manager - Setup
echo ================================================
echo.

echo Verificando .NET SDK...
dotnet --version >nul 2>&1
if errorlevel 1 (
    echo ERRO: .NET SDK nao encontrado!
    echo Por favor, instale o .NET 8 SDK:
    echo https://dotnet.microsoft.com/download/dotnet/8.0
    pause
    exit /b 1
)

echo OK - .NET SDK encontrado
echo.

echo Restaurando dependencias...
dotnet restore
if errorlevel 1 (
    echo ERRO ao restaurar dependencias
    pause
    exit /b 1
)

echo.
echo ================================================
echo   Setup concluido com sucesso!
echo ================================================
echo.
echo Para executar o projeto, digite:
echo   dotnet run
echo.
echo O sistema estara disponivel em:
echo   http://localhost:5000
echo   https://localhost:5001
echo.
echo Login padrao:
echo   Usuario: admin
echo   Senha:   admin123
echo.
pause
