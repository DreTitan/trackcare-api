FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY src/TrackCare.WebApi/TrackCare.WebApi.csproj TrackCare.WebApi/
COPY src/TrackCare.Application/TrackCare.Application.csproj TrackCare.Application/
COPY src/TrackCare.Domain/TrackCare.Domain.csproj TrackCare.Domain/
COPY src/TrackCare.Infrastructure/TrackCare.Infrastructure.csproj TrackCare.Infrastructure/
RUN dotnet restore TrackCare.WebApi/TrackCare.WebApi.csproj
COPY src/ .
WORKDIR /src/TrackCare.WebApi
RUN dotnet publish TrackCare.WebApi.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
HEALTHCHECK --interval=30s --timeout=10s --start-period=15s --retries=3 CMD curl -f http://localhost:8080/ || exit 1
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TrackCare.WebApi.dll"]
