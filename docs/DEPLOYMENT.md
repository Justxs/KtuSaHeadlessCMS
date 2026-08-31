# Deploying the KTU SA CMS

How to run the Orchard Core CMS in production, how it is hosted today, and how to move it to a self-managed Linux server.

The public website is a separate Next.js application in the [KTU-SA-Site](https://github.com/Justxs/KTU-SA-Site) repository — see its `DEPLOYMENT.md`. The two deploy independently; the only thing between them is the API URL.

---

## Contents

1. [What a deployment consists of](#1-what-a-deployment-consists-of)
2. [Configuration reference](#2-configuration-reference)
3. [The state that must survive a redeploy](#3-the-state-that-must-survive-a-redeploy)
4. [Deploying to Azure App Service](#4-deploying-to-azure-app-service)
5. [Deploying with Docker on a Linux server](#5-deploying-with-docker-on-a-linux-server)
6. [Reverse proxy and TLS](#6-reverse-proxy-and-tls)
7. [Moving from Azure to your own server](#7-moving-from-azure-to-your-own-server)
8. [Backups](#8-backups)
9. [Redeploying and upgrading](#9-redeploying-and-upgrading)
10. [Post-deployment checks](#10-post-deployment-checks)
11. [Troubleshooting](#11-troubleshooting)

---

## 1. What a deployment consists of

```
                    ┌──────────────────────────┐
   visitors ───────►│  Next.js site (Vercel)   │
                    └────────────┬─────────────┘
                                 │ HTTPS, GET /api/*
                                 ▼
                    ┌──────────────────────────┐
   editors ────────►│  Orchard Core CMS        │
                    │  ASP.NET Core 10         │
                    └───┬──────────────────┬───┘
                        │                  │
              SQLite +  │                  │  media read/write
              App_Data  ▼                  ▼
              (persistent volume)   Google Cloud Storage bucket
                                    (images and PDFs, served
                                     directly to visitors)
```

Four things have to be in place for a working deployment:

| Piece | Where it lives | Notes |
| --- | --- | --- |
| The application | This repo, published as .NET 10 | Stateless. Rebuildable from source at any time. |
| `App_Data` | Persistent disk or volume | **Not** rebuildable. See [section 3](#3-the-state-that-must-survive-a-redeploy). |
| Media bucket | Google Cloud Storage | Images and PDFs. Survives redeploys on its own. |
| Fienta organiser ID | Configuration | Optional. Without it the event import dropdown is empty. |

The CMS serves both the editor admin panel and the public read-only API on the same origin. There is no separate API deployment.

---

## 2. Configuration reference

Configuration comes from `appsettings.json` in development and from environment variables in production. Never ship a filled-in `appsettings.json` — it is git-ignored and excluded from the Docker image for that reason. Start from `KtuSaHeadlessCMS/appsettings.example.json` to see the full shape.

> **There is a third source, and it is the one currently in use.** Orchard also reads `App_Data/Sites/Default/appsettings.json` — the *tenant's* settings file — and merges it into the configuration the modules see. On the existing instance that file holds a complete `OrchardCore_Media_GoogleCloudStorage` section including the service-account private key.
>
> Two consequences. A migrated `App_Data` brings working media credentials with it, so you may not need to set any of the Google variables in the container at all. And if you set them anyway, you now have the same setting defined twice — confirm which one won by reading the startup log (see below) rather than assuming.

### How keys map to environment variables

ASP.NET Core flattens nested configuration with a double underscore. The Google Cloud section name *itself* contains single underscores, which is the usual source of confusion:

```
Section:  OrchardCore_Media_GoogleCloudStorage  →  BucketName
Env var:  OrchardCore_Media_GoogleCloudStorage__BucketName
                                              ^^
                                    two underscores here, one everywhere else
```

### Google Cloud media storage

Required. If the section is missing or incomplete and `RequireGoogleCloudStorage` is `true`, **the application throws on startup** rather than silently falling back to local disk.

| Environment variable | Required | Notes |
| --- | --- | --- |
| `OrchardCore_Media_GoogleCloudStorage__BucketName` | yes | The bucket holding media |
| `OrchardCore_Media_GoogleCloudStorage__BasePath` | no | Prefix inside the bucket, if media is not at the root |
| `OrchardCore_Media_GoogleCloudStorage__PublicBaseUrl` | no | Public URL media is served from. Leave empty to use `storage.googleapis.com` directly |
| `OrchardCore_Media_GoogleCloudStorage__RequireGoogleCloudStorage` | no | Defaults to `true`. Keep it `true` in production so a misconfiguration fails loudly |
| `OrchardCore_Media_GoogleCloudStorage__UseApplicationDefaultCredentials` | no | See the two credential options below |

Whatever `PublicBaseUrl` resolves to must be allowed by the website's Content-Security-Policy. The frontend currently allows `https://storage.googleapis.com`; a custom CDN domain means editing `next.config.ts` in the site repo as well.

**Option A — Application Default Credentials (recommended for your own server).** Mount the service-account key file into the container and point Google's SDK at it. No secrets in environment variables:

```bash
OrchardCore_Media_GoogleCloudStorage__UseApplicationDefaultCredentials=true
GOOGLE_APPLICATION_CREDENTIALS=/run/secrets/gcs-service-account.json
```

**Option B — inline credentials.** Set `UseApplicationDefaultCredentials=false` and supply either the whole key file as one string:

```bash
OrchardCore_Media_GoogleCloudStorage__UseApplicationDefaultCredentials=false
OrchardCore_Media_GoogleCloudStorage__CredentialsJson={"type":"service_account",...}
```

or the individual fields:

```bash
OrchardCore_Media_GoogleCloudStorage__ProjectId=ktu-sa-...
OrchardCore_Media_GoogleCloudStorage__ClientEmail=cms@ktu-sa-....iam.gserviceaccount.com
OrchardCore_Media_GoogleCloudStorage__PrivateKeyId=...
OrchardCore_Media_GoogleCloudStorage__PrivateKey=-----BEGIN PRIVATE KEY-----\nMIIE...\n-----END PRIVATE KEY-----\n
```

`PrivateKey` is passed with **literal backslash-n sequences**, not real newlines — the application converts them back before handing the key to Google. A key with real newlines pasted into an environment variable will fail to parse.

The service account needs `roles/storage.objectAdmin` on the bucket: the CMS lists, reads, writes and deletes objects.

### Confirming what the application actually resolved

On every startup the media module logs the configuration it ended up with, at `Warning` level so it always reaches the log file:

```
GoogleCloudMediaStartup|WARN|Google Cloud media storage enabled.
  BucketName: ktusawebsite; BasePath: ; PublicBaseUrl: https://storage.googleapis.com/ktusawebsite
```

```bash
docker compose logs cms | grep 'Google Cloud media storage'
```

This is the authoritative answer to "did my environment variable take effect". If the line is absent entirely, the module did not activate and media is not on Google Cloud.

### Fienta event import

| Environment variable | Required | Notes |
| --- | --- | --- |
| `Fienta__OrganiserId` | no | Without it, **Select Fienta event** in the event editor only offers *None* |
| `Fienta__BaseUrl` | no | Defaults to `https://fienta.com/api/v1/public/events` |

### Hosting

| Environment variable | Value | Why |
| --- | --- | --- |
| `ASPNETCORE_ENVIRONMENT` | `Production` | Enables the exception handler and HSTS in `Program.cs` |
| `ASPNETCORE_HTTP_PORTS` | `8080` | The port Kestrel listens on inside the container |
| `ASPNETCORE_FORWARDEDHEADERS_ENABLED` | `true` | **Required behind a reverse proxy.** See [section 6](#6-reverse-proxy-and-tls) |

---

## 3. The state that must survive a redeploy

`App_Data/` is the whole site. Everything in it is created at runtime, none of it is in source control, and losing any of it has a distinct and unpleasant consequence:

| Path | What it is | If you lose it |
| --- | --- | --- |
| `App_Data/Sites/Default/yessql.db` | The SQLite database — every article, event, contact, user and role | The site is gone. This is the backup that matters. |
| `App_Data/Sites/Default/appsettings.json` | Tenant settings: `DatabaseProvider`, table naming — **and the Google Cloud service-account private key** | Orchard cannot open the database and reruns the setup wizard. Treat this file as a secret |
| `App_Data/Sites/Default/DataProtection-Keys/` | ASP.NET Data Protection keys | Every signed-in user is logged out and any value encrypted with those keys becomes unreadable |
| `App_Data/Sites/Default/IdentityModel-*-Certificates/` | OpenID Connect signing and encryption certificates | Issued tokens stop validating |
| `App_Data/tenants.json` | The tenant registry | Orchard no longer knows the site exists |
| `App_Data/logs/` | NLog output | Nothing important. Safe to discard or rotate. |

Two consequences worth stating plainly:

- **SQLite means one instance.** Two application instances pointed at the same database file will corrupt it. Do not scale out, do not run blue/green with both slots live against one volume. If you ever need more than one instance, move the tenant to PostgreSQL first.
- **Anything that recreates the container filesystem without a volume wipes the site.** That is why Cloud Run and similar ephemeral platforms were ruled out for this setup.

---

## 4. Deploying to Azure App Service

This is where the CMS runs today. The details below are the *requirements* the App Service must satisfy — resource names, deployment credentials and the current pipeline are in your Azure portal, not in this repository, so verify each item against the live resource rather than assuming it matches.

### Build and publish

```bash
dotnet publish KtuSaHeadlessCMS/KtuSaHeadlessCMS.csproj -c Release -o ./publish
```

```bash
cd publish && zip -r ../publish.zip . && cd ..
```

```bash
az webapp deploy --resource-group <group> --name <app-name> --src-path publish.zip --type zip
```

### App settings

Set every variable from [section 2](#2-configuration-reference) under **Configuration → Application settings**. Azure passes application settings to the process as environment variables, so the `__` mapping applies unchanged.

### Two settings that decide whether the site survives

- **`WEBSITE_RUN_FROM_PACKAGE` must be off (`0` or absent).** With run-from-package enabled, `wwwroot` is mounted read-only and Orchard cannot write `App_Data` — the site fails to start, or worse, appears to work and loses state.
- **`WEBSITES_ENABLE_APP_SERVICE_STORAGE` must be `true` if you run a custom container.** For the built-in .NET stack, `/home` is already persistent storage; `App_Data` lives under the app's content root inside `/home/site/wwwroot` and survives restarts.

Forwarded headers are handled by App Service for the built-in stack, but setting `ASPNETCORE_FORWARDEDHEADERS_ENABLED=true` explicitly costs nothing and removes the doubt.

### Getting at `App_Data`

Through the Kudu console at `https://<app-name>.scm.azurewebsites.net`, or:

```bash
az webapp ssh --resource-group <group> --name <app-name>
```

You will need this for backups and for the migration in [section 7](#7-moving-from-azure-to-your-own-server).

---

## 5. Deploying with Docker on a Linux server

The `Dockerfile` and `docker-compose.yml` at the repository root are set up for exactly this: a single container behind a reverse proxy, with `App_Data` on a named volume.

### First deployment

```bash
git clone https://github.com/Justxs/KtuSaHeadlessCMS.git && cd KtuSaHeadlessCMS
```

Create `.env.production` next to `docker-compose.yml` (it is git-ignored):

```bash
OrchardCore_Media_GoogleCloudStorage__BucketName=ktu-sa-media
OrchardCore_Media_GoogleCloudStorage__RequireGoogleCloudStorage=true
OrchardCore_Media_GoogleCloudStorage__UseApplicationDefaultCredentials=true
GOOGLE_APPLICATION_CREDENTIALS=/run/secrets/gcs-service-account.json
Fienta__OrganiserId=<organiser id>
```

Mount the service-account key by adding to the `cms` service in `docker-compose.yml`:

```yaml
    volumes:
      - app_data:/app/App_Data
      - ./secrets/gcs-service-account.json:/run/secrets/gcs-service-account.json:ro
```

Then:

```bash
docker compose up --build --detach
```

The image builds the whole solution, publishes the host project, and runs it as a non-root user on port 8080, bound to `127.0.0.1` so only the reverse proxy can reach it.

### Volume permissions

The container runs as the .NET images' non-root application user. With the **named volume** in the shipped compose file, Docker copies the image's ownership onto the fresh volume and it just works.

If you switch to a **bind mount** — `./data:/app/App_Data` — the host directory's ownership wins and the container gets permission denied on the SQLite file. Find the UID the image actually uses and chown the directory to match:

```bash
docker run --rm ktu-sa-cms:latest id -u
```

```bash
sudo chown -R <that-uid> ./data
```

Named volume unless you have a reason otherwise; it removes this whole class of problem.

### Keeping it running

Compose's `restart: unless-stopped` covers crashes and daemon restarts. For start-on-boot, enable the Docker service itself:

```bash
sudo systemctl enable docker
```

---

## 6. Reverse proxy and TLS

The container speaks plain HTTP on 8080. The proxy terminates TLS and must forward the original scheme, or `app.UseHttpsRedirection()` in `Program.cs` sees an HTTP request, redirects to HTTPS, and the browser loops.

Two halves to get right, and both are needed:

1. The proxy sends `X-Forwarded-Proto: https` and `X-Forwarded-For`.
2. The app trusts them — `ASPNETCORE_FORWARDEDHEADERS_ENABLED=true`, already set in the Dockerfile and compose file.

### Caddy

```caddyfile
cms.ktusa.lt {
    reverse_proxy 127.0.0.1:8080
}
```

Caddy sets the forwarded headers and obtains a certificate on its own. Nothing else to configure.

### nginx

```nginx
server {
    listen 443 ssl http2;
    server_name cms.ktusa.lt;

    ssl_certificate     /etc/letsencrypt/live/cms.ktusa.lt/fullchain.pem;
    ssl_certificate_key /etc/letsencrypt/live/cms.ktusa.lt/privkey.pem;

    # Media uploads and PDFs. The default 1m rejects most report files.
    client_max_body_size 64m;

    location / {
        proxy_pass         http://127.0.0.1:8080;
        proxy_http_version 1.1;
        proxy_set_header   Host              $host;
        proxy_set_header   X-Real-IP         $remote_addr;
        proxy_set_header   X-Forwarded-For   $proxy_add_x_forwarded_for;
        proxy_set_header   X-Forwarded-Proto $scheme;
        proxy_set_header   Upgrade           $http_upgrade;
        proxy_set_header   Connection        keep-alive;
    }
}
```

`client_max_body_size` is not optional here — activity reports and document PDFs regularly exceed nginx's 1 MB default, and the failure looks like a browser error rather than a CMS one.

### CORS

The website calls the API server-side, from Vercel's or your own server's runtime, so no browser CORS negotiation happens on the normal path. If you later add a browser-side call, the `OrchardCore.Cors` feature is already enabled and is configured in the admin panel under **Settings → Cross-Origin Resource Sharing**.

---

## 7. Moving from Azure to your own server

The application is rebuildable from source; `App_Data` is not. The whole migration is "copy one directory, point DNS at the new host".

Do it during a quiet hour and tell editors not to publish until it is finished — anything they change on the old instance after the copy is lost.

> **The archive contains a live Google Cloud service-account private key** in `Sites/Default/appsettings.json`. Move it over `scp`, not through a chat message, a shared drive or a public bucket; delete it from both machines once the migration is verified; and never commit it. If it does leak, rotate the key in the Google Cloud console and update the tenant file.

**1. Stop writes on Azure.** Stop the App Service so nothing is mid-transaction:

```bash
az webapp stop --resource-group <group> --name <app-name>
```

**2. Download `App_Data`.** From the Kudu console, browse to `/home/site/wwwroot/App_Data` and download it as a zip, or over SSH:

```bash
az webapp ssh --resource-group <group> --name <app-name>
```

```bash
cd /home/site/wwwroot && tar czf /tmp/app_data.tar.gz App_Data
```

**3. Verify the archive before you trust it.** It must contain `Sites/Default/yessql.db`, `Sites/Default/appsettings.json`, `Sites/Default/DataProtection-Keys/`, both `IdentityModel-*-Certificates/` directories, and `tenants.json`:

```bash
tar tzf app_data.tar.gz | grep -E 'yessql.db|appsettings.json|DataProtection-Keys|Certificates|tenants.json'
```

**4. Bring the stack up empty on the new server**, so Docker creates the volume:

```bash
docker compose up --build --detach && docker compose stop
```

**5. Restore into the volume.** Write into the volume from a throwaway container rather than hunting for its path on disk:

```bash
docker run --rm -v ktu-sa-cms_app_data:/restore -v "$PWD":/backup alpine \
  sh -c 'rm -rf /restore/* && tar xzf /backup/app_data.tar.gz -C /restore --strip-components=1'
```

`--strip-components=1` drops the leading `App_Data/` from the archive, so the contents land at the volume root. Check that they did:

```bash
docker run --rm -v ktu-sa-cms_app_data:/d alpine ls /d/Sites/Default
```

You should see `yessql.db` and the key directories, not another `App_Data`.

**6. Start it and test on the server's own address**, before touching DNS:

```bash
docker compose up --detach && curl -s -o /dev/null -w '%{http_code}\n' http://127.0.0.1:8080/api/sponsors
```

Expect `200`. Then sign in to `/admin` and confirm your existing account works — that proves the Data Protection keys came across intact.

**7. Point DNS at the new server** and let the certificate issue.

**8. Update the website.** Set `KTU_SA_WEB_API_URL` to the new origin and redeploy the frontend. Nothing on the site changes until it is rebuilt, because responses are cached for an hour.

**9. Leave the Azure App Service stopped, not deleted,** for a week or two. It is the rollback.

### If all you have is `yessql.db`

It is not enough on its own. Dropping the database file into an otherwise empty volume gets you the setup wizard, because Orchard decides whether a site exists from `tenants.json`, and it learns *how* to open the database from the tenant's `appsettings.json`. Neither is inside the `.db` file.

The three things you must supply alongside it:

| File | Can you recreate it? |
| --- | --- |
| `tenants.json` | Yes, but do not — it carries the tenant id the rest of the installation refers to. Copy it. |
| `Sites/Default/appsettings.json` | By hand, if you know the values: `DatabaseProvider: Sqlite`, `DocumentTable: Document`, `IdentityColumnSize: Int64`, `TableNameSeparator: _`. You would also have to re-add the Google Cloud section or move it to environment variables. |
| `Sites/Default/DataProtection-Keys/` | Not really. See below. |

Starting without the Data Protection keys is survivable but not free. Password hashes live in the database, so everyone can sign in again — but any value Orchard encrypted with those keys becomes unreadable, and on this installation that includes the Google authentication client secret and the OpenID server configuration. You would have to re-enter them in the admin panel, and you find out which ones by hitting the failures.

The `IdentityModel-*-Certificates/` directories regenerate themselves if missing. Only previously issued tokens break, which for a read-only public API is nothing.

Copy the whole `App_Data` directory. It is a few megabytes and it removes every one of these decisions.

### Taking the database out safely

The database is in rollback-journal mode, not WAL, so there are no `-wal` or `-shm` sidecar files to remember. That does **not** make `cp` safe while the application is running — a copy taken mid-transaction can still catch a torn write.

Either stop the application first (step 1 above, which is why it is step 1), or take a consistent online copy with SQLite's own backup command:

```bash
sqlite3 App_Data/Sites/Default/yessql.db ".backup /tmp/yessql-migration.db"
```

The archive-the-whole-directory approach in the numbered steps already stops the app first, so it needs neither.

---

## 8. Backups

### From the admin panel

**Backup → Database backup** in the admin menu, visible to Administrators only.

**Download backup** takes a consistent copy of the live SQLite database while the site keeps serving, using SQLite's online backup API, and sends it to your browser as `ktusa-cms-backup-<timestamp>.db`. Safe to run at any time. The file contains user accounts and password hashes, so keep it somewhere private.

**Upload backup** is the restore path, and it works in two steps because a SQLite file cannot be swapped underneath a running tenant:

1. The upload is validated — SQLite header, `PRAGMA quick_check`, and the presence of the `Document` and `ContentItemIndex` tables — then staged next to the database. Nothing has changed at this point, and the upload can be discarded.
2. The swap happens on the next start, before Orchard opens anything. **Apply now and restart** stops the process so whatever supervises it — Docker's `restart: unless-stopped`, systemd, App Service — starts it again immediately. If nothing supervises the process, it stays down until you start it.

The database being replaced is kept as a snapshot in `App_Data/Sites/Default/backup/snapshots/`, and the five most recent are listed on the page for download. To undo a restore, download the snapshot and upload it as a backup.

Two things the page will not do for you:

- **Media is not included.** Images and PDFs live in Google Cloud Storage. A restore can leave content pointing at a file that was deleted since.
- **Keys and certificates are not included.** A restore on the same instance keeps them, which is the normal case. Moving to a different host still needs the whole `App_Data` directory — see [section 7](#7-moving-from-azure-to-your-own-server).

Behind a reverse proxy, the upload is subject to the proxy's body size limit. nginx defaults to 1 MB and will reject the database; `client_max_body_size` is already set to 64m in the example config in [section 6](#6-reverse-proxy-and-tls) — raise it if the database outgrows that.

### From the command line

One directory, one command, and one rule.

**The rule: never `cp` `yessql.db` while the application is running.** SQLite writes in a journal, and a plain copy taken mid-write gives you a file that restores into a corrupt database — usually discovered months later. Use SQLite's own online backup, which is consistent even under concurrent writes:

```bash
docker compose exec cms sh -c 'sqlite3 /app/App_Data/Sites/Default/yessql.db ".backup /app/App_Data/backup.db"'
```

The runtime image has no `sqlite3` binary, so either add it to the Dockerfile's runtime stage or run the backup from a sidecar that mounts the same volume:

```bash
docker run --rm -v ktu-sa-cms_app_data:/d -v "$PWD/backups":/out \
  alpine sh -c 'apk add --no-cache sqlite >/dev/null && sqlite3 /d/Sites/Default/yessql.db ".backup /out/yessql-$(date +%F).db"'
```

The keys and certificates never change, so a one-off archive of them is enough — but take it, because without them a restored database still logs everyone out:

```bash
docker run --rm -v ktu-sa-cms_app_data:/d -v "$PWD/backups":/out alpine \
  tar czf /out/keys.tar.gz -C /d Sites/Default/DataProtection-Keys Sites/Default/IdentityModel-Encryption-Certificates Sites/Default/IdentityModel-Signing-Certificates tenants.json
```

Media is in Google Cloud Storage. Turn on object versioning on the bucket and it backs itself up; nothing in this repo needs to handle it.

A restore you have never tested is not a backup. Restore last night's file into a scratch volume once, start the container against it, and log in.

---

## 9. Redeploying and upgrading

An ordinary code deploy:

```bash
git pull && docker compose up --build --detach
```

Compose recreates the container and reattaches the same volume, so no state is touched.

**Schema migrations run automatically on startup.** The `DataMigration` classes in `OrchardCore.Cms.KtuSaModule/Migrations/` execute when the new version boots, and they alter content type definitions and occasionally rewrite existing content items. They are not transactional across the whole startup and there is no down-migration. Take a database backup immediately before any deploy that includes a migration change — that is your only way back.

Check the log after a migration deploy:

```bash
docker compose logs --tail 100 cms
```

To roll back, restore the backup and redeploy the previous commit. Restoring a newer database into older code will not work — migrations only run forwards.

---

## 10. Post-deployment checks

```bash
curl -s -o /dev/null -w 'sponsors %{http_code}\n' https://<cms-host>/api/sponsors
curl -s -o /dev/null -w 'articles %{http_code}\n' 'https://<cms-host>/api/articles?page=1&pageSize=1'
```

Both should return `200`, and the articles response should be an envelope with `items`, `totalCount` and `totalPages` rather than a bare array.

Then, by hand:

- [ ] `/admin` loads and your existing account signs in — proves the Data Protection keys survived
- [ ] An existing article shows its hero image — proves Google Cloud Storage credentials work for reads
- [ ] Uploading a file in **Media** succeeds — proves they work for writes, which is a different permission
- [ ] The event editor's **Select Fienta event** dropdown lists events — proves `Fienta__OrganiserId` is set
- [ ] The public website renders content from the new API URL
- [ ] `docker compose ps` reports the container `healthy`, not just `running`

---

## 11. Troubleshooting

**The container exits immediately with `Google Cloud media storage is required but not configured`.**
Exactly what it says: `BucketName` is empty, or `UseApplicationDefaultCredentials` is `false` with no credentials supplied. Check the `__` in the variable names first — a single underscore where two belong is the usual cause, and it produces this identical error.

**Endless redirect loop in the browser.**
`ASPNETCORE_FORWARDEDHEADERS_ENABLED` is not `true`, or the proxy is not sending `X-Forwarded-Proto`. `Program.cs` calls `UseHttpsRedirection()` unconditionally, so an app that believes it was reached over HTTP will redirect forever.

**Signed out after a deploy, and cannot sign back in.**
The Data Protection keys were not on the volume — the container generated new ones. Restore `Sites/Default/DataProtection-Keys/` from backup and restart.

**Uploaded a backup in the admin panel but nothing changed.**
By design — the upload is staged, not applied. Use **Apply now and restart**, or restart the CMS yourself. The swap is logged on startup with a `[backup]` prefix:

```bash
docker compose logs cms | grep '\[backup\]'
```

The same lines are appended to `App_Data/Sites/Default/backup/restore.log`.

**Applied a restore and the CMS never came back.**
`Apply now and restart` stops the process and relies on something restarting it. Under Docker with `restart: unless-stopped` that is automatic; run by hand it is not. Start it again — the restore is applied on the way up regardless of how long it stayed down.

**The admin panel says backups are unavailable.**
The page only supports SQLite. It says so, and names the provider it found instead.

**Orchard shows the setup wizard on a site that already exists.**
`App_Data` is not mounted where the app expects, so it sees no tenant. Do not complete the wizard: stop the container, fix the volume, start again. Completing it writes a new `tenants.json` over the real one.

**`SQLite Error 8: attempt to write a readonly database`.**
Volume permissions. See [Volume permissions](#volume-permissions).

**Uploads fail for larger PDFs but small images work.**
`client_max_body_size` in nginx. The default is 1 MB.

**Logs are written to two different places.**
`NLog.config` writes to `App_Data/logs/` via `${var:configDir}`, and its own internal log to `App_Data/logs/internal-nlog.txt` relative to the working directory. Depending on the content root these can resolve differently and you end up with a nested `App_Data/App_Data/logs/`. Harmless, but if you go looking for a log line and cannot find it, check both.
