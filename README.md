# KTU SA Headless CMS

Headless CMS for KTU Student Association content, built with Orchard Core and custom modules for content modeling, API delivery, admin tooling, and Google Cloud media storage.

## What This Project Contains

This repository is a multi-project Orchard Core solution:

- `KtuSaHeadlessCMS`:
  - Main ASP.NET Core host application
  - Boots Orchard Core
  - Registers global configuration (for example `Fienta` settings)
- `OrchardCore.Cms.KtuSaModule`:
  - Core content management module
  - Custom content parts, migrations, drivers, handlers, permissions, and admin navigation
  - Includes setup recipe: `ktu-sa-headless.recipe.json`
- `OrchardCore.Cms.KtuSaApi`:
  - Public API module built with FastEndpoints
  - OpenAPI generation + Scalar API docs
- `OrchardCore.Cms.KtuSaGoogleMedia`:
  - Google Cloud Storage integration for Orchard Media
  - Replaces default media storage when configured
- `OrchardCore.KtuSaTheme`:
  - Custom admin theme extending `TheAdmin`

## Tech Stack

- .NET `10.0`
- Orchard Core `2.2.1`
- FastEndpoints `8.0.1`
- Scalar.AspNetCore `2.12.48`
- Google Cloud Storage SDK (`Google.Cloud.Storage.V1`)
- NLog (`OrchardCore.Logging.NLog`)

## Prerequisites

- .NET SDK `10.0.x`
- A database supported by Orchard Core (SQLite for local development is the easiest option)
- Optional:
  - Google Cloud Storage bucket and credentials (if using cloud media)
  - Fienta organizer ID (if using event import from Fienta)

## Quick Start

1. Clone repository.
2. Create local app settings file:
   - copy `KtuSaHeadlessCMS/appsettings.example.json` to `KtuSaHeadlessCMS/appsettings.json`
3. Decide media storage mode:
   - for Google Cloud media: fill `OrchardCore_Media_GoogleCloudStorage`
   - for local development without Google Cloud: set `OrchardCore_Media_GoogleCloudStorage.RequireGoogleCloudStorage` to `false`
4. Restore and build:
   - `dotnet restore`
   - `dotnet build KtuSaHeadlessCMS.sln`
5. Run:
   - `dotnet run --project KtuSaHeadlessCMS/KtuSaHeadlessCMS.csproj`
6. Open the site in browser:
   - `https://localhost:5001` (default launch profile)
7. Complete Orchard setup wizard:
   - choose site name and admin credentials
   - choose database provider/connection
   - select recipe `KTU SA Headless recipe` (from `KtuSaHeadlessRecipe`)

## Configuration

### `appsettings.json`

Start from `KtuSaHeadlessCMS/appsettings.example.json`.

Important sections:

- `Fienta`:
  - `BaseUrl`: defaults to `https://fienta.com/api/v1/public/events`
  - `OrganiserId`: required for successful Fienta import in admin
- `OrchardCore_Media_GoogleCloudStorage`:
  - `BucketName`: required when Google media is enabled
  - `RequireGoogleCloudStorage`: if `true` and config is incomplete, app startup will fail
  - `UseApplicationDefaultCredentials`: use ADC when `true`
  - `CredentialsJson` or service account fields can be used when `UseApplicationDefaultCredentials` is `false`

### Media Storage Behavior

- When Google Cloud configuration is valid, the project swaps Orchard's default media services with Google Cloud-backed implementations.
- When invalid:
  - if `RequireGoogleCloudStorage = true`, startup throws
  - if `RequireGoogleCloudStorage = false`, app continues with default Orchard media behavior

## Development Workflow

Useful commands:

- Build full solution:
  - `dotnet build KtuSaHeadlessCMS.sln`
- Build API module only:
  - `dotnet build OrchardCore.Cms.KtuSaApi/OrchardCore.Cms.KtuSaApi.csproj`
- Build content module only:
  - `dotnet build OrchardCore.Cms.KtuSaModule/OrchardCore.Cms.KtuSaModule.csproj`
- Run host:
  - `dotnet run --project KtuSaHeadlessCMS/KtuSaHeadlessCMS.csproj`

## API Documentation

API documentation is served via Scalar from the API module (`OrchardCore.Cms.KtuSaApi`).  
Use Scalar/OpenAPI in the running application for endpoint-level docs.

## Notes

- This repo includes a large localization set under `KtuSaHeadlessCMS/Localization`.
- The setup recipe enables core Orchard features, custom KTU SA modules, roles, and admin theme configuration.
- If build fails with file lock errors, stop running app instances / IIS Express / Visual Studio debug sessions and rebuild.
