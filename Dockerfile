# syntax=docker/dockerfile:1

# ---------- build ----------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Restore first, on project files only, so the layer is cached across source edits.
COPY KtuSaHeadlessCMS/KtuSaHeadlessCMS.csproj                                 KtuSaHeadlessCMS/
COPY OrchardCore.Cms.KtuSaApi/OrchardCore.Cms.KtuSaApi.csproj                 OrchardCore.Cms.KtuSaApi/
COPY OrchardCore.Cms.KtuSaGoogleMedia/OrchardCore.Cms.KtuSaGoogleMedia.csproj OrchardCore.Cms.KtuSaGoogleMedia/
COPY OrchardCore.Cms.KtuSaModule/OrchardCore.Cms.KtuSaModule.csproj           OrchardCore.Cms.KtuSaModule/
COPY OrchardCore.KtuSaTheme/OrchardCore.KtuSaTheme.csproj                     OrchardCore.KtuSaTheme/
RUN dotnet restore KtuSaHeadlessCMS/KtuSaHeadlessCMS.csproj

COPY . .
RUN dotnet publish KtuSaHeadlessCMS/KtuSaHeadlessCMS.csproj \
    --configuration Release \
    --no-restore \
    --output /app/publish

# ---------- runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# curl is only here so the compose healthcheck has something to call.
RUN apt-get update \
    && apt-get install --yes --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish ./

# Orchard writes the SQLite database, Data Protection keys, OpenID certificates
# and logs here. Mount a volume over it or everything is lost on redeploy.
RUN mkdir -p /app/App_Data && chown -R $APP_UID /app/App_Data

USER $APP_UID

ENV ASPNETCORE_HTTP_PORTS=8080 \
    ASPNETCORE_FORWARDEDHEADERS_ENABLED=true \
    ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

ENTRYPOINT ["dotnet", "KtuSaHeadlessCMS.dll"]
