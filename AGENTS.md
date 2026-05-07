# Repository Guidelines

## Project Structure & Module Organization

This repository is a .NET 5 solution centered on `BobNetEverywhere.sln`.

- `bobweb/` contains the ASP.NET Core web host, controllers, app settings, and project-specific `wwwroot/javascript/overrides.js`.
- `bobelectron/` contains the Electron.NET desktop host, Electron manifest, assets, controllers, and desktop-specific overrides.
- `bobdomain/` contains shared domain classes used by both hosts, such as item and graph data models.
- `bobstatic/` contains shared HTML, CSS, JavaScript, image, and component assets. Both host projects link this folder into `wwwroot` during build.
- `Dockerfile` and `docker-compose.yml` support container deployment.

## Build, Test, and Development Commands

- `dotnet restore BobNetEverywhere.sln` restores NuGet packages for all projects.
- `dotnet build BobNetEverywhere.sln` compiles the web, Electron, and domain projects.
- `dotnet run --project bobweb/bobweb.csproj` runs the web host locally using `bobweb/Properties/launchSettings.json`.
- `dotnet run --project bobelectron/bobelectron.csproj` starts the Electron.NET host; desktop packaging may require Electron.NET tooling installed locally.
- `dotnet publish bobweb/bobweb.csproj --configuration Release` creates a release publish output for web deployment.
- `docker build -t bobcalypsoweb .` builds the container image described by `Dockerfile`.

## Coding Style & Naming Conventions

Use the existing C# style: four-space indentation, braces on new lines, PascalCase for public types and methods, camelCase for parameters and locals, and `_camelCase` for private fields. Keep namespaces aligned with project names such as `bobweb.Controllers`. Place shared logic in `bobdomain` instead of duplicating it across `bobweb` and `bobelectron`.

For static assets, keep shared UI code in `bobstatic/js`, `bobstatic/css`, and `bobstatic/comp`. Use host-specific override files only for behavior that differs between web and Electron.

Use comments sparingly. Only comment complex code, never remove existing comments, and add method comments for all methods that are not trivial.

Do not refactor code that is unrelated to the requested change. For example, do not rewrite a loop, change formatting, or alter structure in unaffected code while applying a targeted fix.

## Testing Guidelines

There is currently no dedicated test project. Before opening a PR, run `dotnet build BobNetEverywhere.sln` and manually smoke-test changed routes or UI flows. If adding tests, create a sibling project such as `bobdomain.tests/`, use `*Tests.cs` file names, and add it to `BobNetEverywhere.sln`.

Always add unit tests for any new or modified code. If a change truly cannot be unit tested, document the reason and the manual verification performed.

## Change Workflow

Always ask qualifying questions before proceeding with code changes so the scope, behavior, and acceptance criteria are clear.

## Commit & Pull Request Guidelines

Recent commits use short, imperative summaries such as `docker` and `fix nl`. Keep commits focused and use clearer summaries when possible, for example `Fix chart data serialization` or `Update Docker deployment config`.

Pull requests should include a brief description, manual test notes or build output, linked issues when applicable, and screenshots for UI changes in `bobstatic`, `bobweb`, or `bobelectron`.

For post-change deliverables, always ask whether the user is ready to produce the commit, PR, and ticket. Only provide the requested combination after the user confirms. Post-change deliverables should include:

- Commit message format.
- PR description template.
- Jira ticket description template.

## Security & Configuration Tips

Do not commit secrets or machine-specific paths in `appsettings.Development.json`. Keep production file paths, service users, and deployment URLs in environment-specific configuration.

Do not commit PII, secrets, passwords, or machine-specific settings. Ensure code follows security best practices for storing and handling PII, passwords, secrets, and other sensitive data.
