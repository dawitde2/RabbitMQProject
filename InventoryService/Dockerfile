FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj files FIRST (for Docker cache)
COPY InventoryService/InventoryService.csproj InventoryService/
COPY Share/Shared.csproj Share/
RUN dotnet restore InventoryService/InventoryService.csproj

# Copy everything else
COPY . .

WORKDIR /src/InventoryService
RUN dotnet build InventoryService.csproj -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
WORKDIR /src/InventoryService
RUN dotnet publish InventoryService.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InventoryService.dll"]
