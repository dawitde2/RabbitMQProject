FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files for cache
COPY OrderService/OrderService.csproj OrderService/
COPY Share/Shared.csproj Share/
RUN dotnet restore OrderService/OrderService.csproj

# Copy everything else
COPY . .

WORKDIR /src/OrderService
RUN dotnet build OrderService.csproj -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
WORKDIR /src/OrderService
RUN dotnet publish OrderService.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderService.dll"]
