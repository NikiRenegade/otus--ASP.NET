# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY Homeworks/EF/src/PromoCodeFactory.WebHost/PromoCodeFactory.WebHost.csproj PromoCodeFactory.WebHost/
COPY Homeworks/EF/src/PromoCodeFactory.Core/PromoCodeFactory.Core.csproj PromoCodeFactory.Core/
COPY Homeworks/EF/src/PromoCodeFactory.DataAccess/PromoCodeFactory.DataAccess.csproj PromoCodeFactory.DataAccess/

RUN dotnet restore PromoCodeFactory.WebHost/PromoCodeFactory.WebHost.csproj

COPY Homeworks/EF/src/PromoCodeFactory.WebHost/ PromoCodeFactory.WebHost/
COPY Homeworks/EF/src/PromoCodeFactory.Core/ PromoCodeFactory.Core/
COPY Homeworks/EF/src/PromoCodeFactory.DataAccess/ PromoCodeFactory.DataAccess/

WORKDIR /src/PromoCodeFactory.WebHost
RUN dotnet build "PromoCodeFactory.WebHost.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "PromoCodeFactory.WebHost.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PromoCodeFactory.WebHost.dll"]
