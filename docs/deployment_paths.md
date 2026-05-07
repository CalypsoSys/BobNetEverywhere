# Deployment Paths

## Purpose

`BobNetEverywhere` intentionally keeps three sample deployment paths alive:

- Web hosting with ASP.NET Core
- Containerized web hosting with Docker Compose
- Desktop hosting with Electron.NET

This repo is a sample, not a private production repo, so the docs stay generic and avoid secrets.

## ASP.NET Core Web Host

Use this path when you want a plain published web app behind a reverse proxy such as Caddy or nginx.

```bash
dotnet publish bobweb/bobweb.csproj --configuration Release
```

Deploy the published output to the target host and run it under `dotnet`, `systemd`, IIS, or another host-specific
process manager.

## Docker Compose Web Host

Use this path when you want the sample web API packaged with a container image.

```bash
docker compose up -d --build
```

The current compose file exposes the API on `http://localhost:6000` and leaves domain, proxy, and TLS concerns to the
host environment.

## IIS on Windows

Use this path when demonstrating the sample as a conventional Windows web deployment.

- Publish `bobweb`.
- Create an IIS site or application for the published output.
- Configure the ASP.NET Core Hosting Bundle on the target machine if needed.
- Keep environment-specific settings out of source control.

## Electron.NET Desktop

Use this path when demonstrating the same app shape as a desktop host.

- Run `bobelectron` locally during development.
- Treat packaging and installer creation as environment-specific tasks.
- Keep machine-specific Electron packaging details out of this public sample repo unless they are generic.
