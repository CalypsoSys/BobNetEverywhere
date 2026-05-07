# BobNetEverywhere

`BobNetEverywhere` is a public `.NET 10` sample that demonstrates the same domain model across multiple hosting
styles:

- ASP.NET Core web hosting
- nginx on Ubuntu for reverse-proxied web hosting
- Docker-based web hosting
- IIS-hosted web deployment
- Electron.NET desktop hosting

## Repository Layout

- `bobweb/` contains the ASP.NET Core web host.
- `bobelectron/` contains the Electron.NET desktop host.
- `bobdomain/` contains shared domain logic.
- `bobstatic/` contains shared static assets.
- `bobdomain.tests/` and `bobweb.tests/` contain automated tests.
- `docs/` contains focused development and deployment notes.

## Quick Start

```bash
dotnet restore BobNetEverywhere.sln
dotnet build BobNetEverywhere.sln -c Debug
dotnet test BobNetEverywhere.sln -c Debug
dotnet run --project bobweb/bobweb.csproj
```

To run the container sample:

```bash
docker compose up --build
```

## Documentation

- `docs/local_development.md` covers local web, Docker, and Electron workflows.
- `docs/deployment_paths.md` summarizes the supported sample deployment paths.
- `docs/deployment_runbook.md` provides a generic verification and deployment checklist.
- `docs/caddy_reverse_proxy.md` shows a public-safe Caddy example for reverse proxying the web host.

## nginx on Ubuntu Sample

Publish the web host:

```bash
dotnet publish bobweb/bobweb.csproj --configuration Release
```

Example publish output path:

```text
BobNetEverywhere/bobweb/bin/Release/net10.0/publish
```

If you copy the published files to `/opt/bobnet/bobweb`, an nginx site can proxy traffic to the ASP.NET Core app on
port `6000`.

Example nginx location block:

```nginx
location / {
    proxy_pass         http://localhost:6000;
    proxy_http_version 1.1;
    proxy_set_header   Upgrade $http_upgrade;
    proxy_set_header   Connection keep-alive;
    proxy_set_header   Host $host;
    proxy_cache_bypass $http_upgrade;
    proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
    proxy_set_header   X-Forwarded-Proto $scheme;
}
```

Example `systemd` service:

```ini
[Unit]
Description=Bob Calypsos .Net Everywhere

[Service]
WorkingDirectory=/opt/bobnet/bobweb
ExecStart=/usr/bin/dotnet /opt/bobnet/bobweb/bobweb.dll --urls=http://localhost:6000
Restart=always
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=bob-calypso
User=www-data
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

[Install]
WantedBy=multi-user.target
```

Enable and start the service:

```bash
sudo systemctl enable bob-calypso.service
sudo systemctl start bob-calypso.service
sudo systemctl status bob-calypso.service
```

## VS Code

The repo includes `.vscode/` settings for a local debugging workflow that pairs:

- `bobweb` on `http://localhost:6000`
- `bobstatic` served through the Live Server extension on `http://127.0.0.1:5500`

Start Live Server for `bobstatic/index.html`, then use the `Local: Live Server + bobweb` compound launch.

## Configuration

This repo is intentionally public-facing. Keep secrets, private domains, private IPs, and machine-specific settings
out of source control.

The compose file maps the web API to `http://localhost:6000` and does not assume a specific domain, reverse proxy, or
TLS provider. If the static files are not served from the same origin as the API, set `window.bobApiBaseUrl` in
`bobstatic/js/bob_config.js` to the API origin used by the browser.
