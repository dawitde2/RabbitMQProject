FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj files FIRST (Docker layer cache friendly)
COPY PaymentService/PaymentService.csproj PaymentService/
COPY Share/Shared.csproj Share/
RUN dotnet restore PaymentService/PaymentService.csproj

# Copy everything else
COPY . .

WORKDIR /src/PaymentService
RUN dotnet build PaymentService.csproj -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
WORKDIR /src/PaymentService
RUN dotnet publish PaymentService.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PaymentService.dll"]
