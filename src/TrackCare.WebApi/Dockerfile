FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TrackCare.WebApi/TrackCare.WebApi.csproj", "TrackCare.WebApi/"]
COPY ["TrackCare.Application/TrackCare.Application.csproj", "TrackCare.Application/"]
COPY ["TrackCare.Domain/TrackCare.Domain.csproj", "TrackCare.Domain/"]
COPY ["TrackCare.Infrastructure/TrackCare.Infrastructure.csproj", "TrackCare.Infrastructure/"]
RUN dotnet restore "TrackCare.WebApi/TrackCare.WebApi.csproj"
COPY . ./
WORKDIR "/src/TrackCare.WebApi"
RUN dotnet build "TrackCare.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TrackCare.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TrackCare.WebApi.dll"]
