FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

FROM microsoft/aspnetcore-build:5.0 AS build
WORKDIR /src
COPY Solution.sln ./
COPY bobdomain/*.csproj ./bobdomain/
COPY bobweb/*.csproj ./bobweb/

RUN dotnet restore
COPY . .
WORKDIR /src/bobdomain
RUN dotnet build -c Release -o /app

WORKDIR /src/bobweb
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "bobweb.dll"]
