FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/FamilyAccountant.WebApi/FamilyAccountant.WebApi.csproj", "src/FamilyAccountant.WebApi/"]
COPY ["src/FamilyAccountant.Application/FamilyAccountant.Application.csproj", "src/FamilyAccountant.Application/"]
COPY ["src/FamilyAccountant.Domain/FamilyAccountant.Domain.csproj", "src/FamilyAccountant.Domain/"]
COPY ["src/FamilyAccountant.Infrastructure/FamilyAccountant.Infrastructure.csproj", "src/FamilyAccountant.Infrastructure/"]
COPY ["src/FamilyAccountant.Infrastructure.Migrations/FamilyAccountant.Infrastructure.Migrations.csproj", "src/FamilyAccountant.Infrastructure.Migrations/"]
RUN dotnet restore "src/FamilyAccountant.WebApi/FamilyAccountant.WebApi.csproj"
COPY . .
COPY ["../DockerSettings", "src/FamilyAccountant.WebApi/"]
WORKDIR "/src/src/FamilyAccountant.WebApi"
RUN dotnet build "FamilyAccountant.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FamilyAccountant.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FamilyAccountant.WebApi.dll"]
