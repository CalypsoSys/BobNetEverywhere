# Local Development

## Goals

`BobNetEverywhere` is a public sample repo that demonstrates the same domain logic across three paths:

- ASP.NET Core web hosting
- Docker-hosted web deployment
- Electron.NET desktop hosting

The local workflow should stay simple and should not require committed secrets.

## Prerequisites

- .NET 10 SDK
- Docker Desktop or Docker Engine for the container sample
- Electron.NET tooling if you want to run the desktop sample

## Web Host

Restore, build, and run the web sample:

```bash
dotnet restore BobNetEverywhere.sln
dotnet build BobNetEverywhere.sln -c Debug
dotnet run --project bobweb/bobweb.csproj
```

The development config in `bobweb/appsettings.Development.json` intentionally contains only non-secret localhost
origins for CORS and stores sample item data in the repo-level `data/` folder.

In hosted environments, the Bob sample can point the static site at a separate API host such as
`https://api.bobcalypso.com`. The local Live Server workflow keeps using `window.bobApiBaseUrl` to point browser
requests at `http://localhost:8081`.

## VS Code Workflow

The repository includes `.vscode/launch.json`, `.vscode/tasks.json`, and `.vscode/settings.json` for a lightweight
local workflow similar to `mma`, but without a JavaScript build tool.

1. Open `bobstatic/index.html` in VS Code.
2. Start the Live Server extension for that file.
3. Run the `Local: Live Server + bobweb` compound launch.

That flow keeps the static sample on `http://127.0.0.1:5500` and the API on `http://localhost:8081`, which matches the
existing development CORS examples.

## Docker Sample

Build and run the web sample in Docker:

```bash
docker compose up --build
```

The compose file publishes the ASP.NET Core app on `http://localhost:8081` and mounts repo-level `./data` into the
container at `/app/data`. It also mounts repo-level `./logs` into `/app/logs` so access and error logs are visible on
the host during local smoke tests.

## Electron Sample

Run the desktop sample:

```bash
dotnet run --project bobelectron/bobelectron.csproj
```

If Electron.NET tooling is not installed locally yet, install and configure it before treating the desktop sample as
verified.

## Tests

Run the current automated test suite:

```bash
dotnet test BobNetEverywhere.sln -c Debug
```
