# Deployment Runbook

## Purpose

This runbook covers a generic deployment flow for the public `BobNetEverywhere` sample. It is intentionally written
without secrets, private domains, or machine-specific assumptions.

## Verify Before Deploying

```bash
dotnet restore BobNetEverywhere.sln
dotnet build BobNetEverywhere.sln -c Debug
dotnet test BobNetEverywhere.sln -c Debug
docker compose config
```

## Docker-Based Web Deployment

1. Build and start the service.
2. Confirm the container is healthy and listening on port `6000`.
3. Put Caddy or nginx in front of the service if you need TLS or public routing.
4. Smoke-test the API routes and any static UI that depends on them.

Example command:

```bash
docker compose up -d --build
```

## Non-Docker Web Deployment

1. Publish `bobweb`.
2. Copy the publish output to the target host.
3. Start it under the host's process manager or IIS.
4. Place a reverse proxy in front of it if required.
5. Smoke-test the same public routes after deployment.

Example command:

```bash
dotnet publish bobweb/bobweb.csproj --configuration Release
```

## Desktop Sample Verification

1. Launch `bobelectron`.
2. Confirm the core sample flows still behave the same way as the web host.
3. Treat desktop packaging as a separate environment-specific concern.

## Post-Deploy Checks

- Confirm the expected host and port respond.
- Confirm CORS still matches the intended browser origin pattern.
- Confirm no environment-specific secrets or config files were added to Git.
