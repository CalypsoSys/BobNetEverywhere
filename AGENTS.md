# Repository Guidelines

## Project Structure & Module Organization

This repository is a `.NET 10` sample solution centered on `BobNetEverywhere.sln`.

- `bobweb/` contains the ASP.NET Core web host, controllers, startup code, and web-specific configuration.
- `bobelectron/` contains the Electron.NET desktop host, Electron manifest, and desktop startup code.
- `bobdomain/` contains shared domain classes used by both hosts.
- `bobstatic/` contains shared static HTML, CSS, JavaScript, images, and components for the sample UI.
- `bobdomain.tests/` and `bobweb.tests/` contain the current automated test coverage.
- `docs/` contains deployment, reverse proxy, and local-development notes for this public sample repo.
- `Dockerfile` and `docker-compose.yml` support the containerized web sample.

## Build, Test, and Development Commands

- `dotnet restore BobNetEverywhere.sln` restores NuGet packages for the solution.
- `dotnet build BobNetEverywhere.sln -c Debug` builds the web, Electron, domain, and test projects.
- `dotnet test BobNetEverywhere.sln -c Debug` runs the current test suite.
- `dotnet run --project bobweb/bobweb.csproj` runs the web host locally.
- `dotnet run --project bobelectron/bobelectron.csproj` starts the Electron.NET sample; desktop packaging may
  require Electron.NET tooling installed locally.
- `dotnet publish bobweb/bobweb.csproj --configuration Release` creates a publish output for web deployment.
- `docker compose up --build` builds and runs the containerized web sample.

## Coding Style & Naming Conventions

Use the existing C# style: four-space indentation, braces on new lines, PascalCase for public types and methods,
camelCase for parameters and locals, and `_camelCase` for private fields. Keep namespaces aligned with project names
such as `bobweb.Controllers`. Place shared logic in `bobdomain` instead of duplicating it across `bobweb` and
`bobelectron`.

For static assets, keep shared UI code in `bobstatic/`. Use host-specific overrides only for behavior that genuinely
differs between web and Electron.

Use comments sparingly. Only comment complex code, never remove existing comments, and add method comments for
non-trivial methods.

Do not refactor code unrelated to the requested change. Do not rewrite loops, alter formatting, or restructure
unaffected code while making a targeted fix.

## Testing Guidelines

This repository already includes automated tests in `bobdomain.tests/` and `bobweb.tests/`. Run
`dotnet test BobNetEverywhere.sln -c Debug` before finishing a change, then manually smoke-test the affected sample
flow when UI, routing, Electron, or deployment behavior changes.

Always add unit tests for new or modified code when the change is testable. If a change cannot be covered
meaningfully, document the reason and the manual verification performed.

## Change Workflow

Always ask qualifying questions before proceeding with code changes so scope, behavior, and acceptance criteria are
clear.

## Commit & Pull Request Guidelines

Keep commits focused and use clear, imperative summaries that describe the shipped change, for example
`Add Caddy deployment guide` or `Fix CORS origin handling`.

Pull requests should include a brief description, manual verification notes or build output, linked issues when
applicable, and screenshots for visible UI changes in `bobstatic`, `bobweb`, or `bobelectron`.

For post-change deliverables, always ask whether the user is ready to produce the commit, PR, and ticket. Only
provide the requested combination after the user confirms. Post-change deliverables should include:

- Commit message format.
- PR description template.
- Jira ticket description template.

## Security & Configuration Tips

This is a public sample repo. Do not commit secrets, PII, passwords, tokens, private hostnames, or machine-specific
settings.

Keep production-specific values out of the repo. Use environment variables, deployment-time configuration, or
platform-specific secret stores instead of committed local secrets.

`appsettings.Development.json` may contain non-secret local sample values such as allowed localhost origins, but it
must not contain credentials or environment-specific private endpoints.
