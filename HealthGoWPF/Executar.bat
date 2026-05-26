@echo off
echo ========================================
echo   HealthGo Collection Manager
echo ========================================
echo.
cd /d "%~dp0bin\Release\net8.0-windows"
start HealthGoWPF.exe
echo.
echo O sistema sera iniciado automaticamente.
echo Se nao abrir, verifique se o .NET 8 esta instalado.
pause
