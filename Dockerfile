FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app 
#
# copy csproj and restore as distinct layers
COPY bobweb/*.csproj ./bobweb/
COPY bobdomain/*.csproj ./bobdomain/
#
RUN dotnet restore ./bobweb/bobweb.csproj
RUN dotnet restore ./bobdomain/bobdomain.csproj 
#
# copy everything else and build app
COPY bobweb/. ./bobweb/
COPY bobstatic/. ./bobweb/wwwroot/
COPY bobdomain/. ./bobdomain/
#
WORKDIR /app/bobweb
RUN dotnet publish -c Release -o out 
#
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /app 
#
COPY --from=build /app/bobweb/out ./
ENTRYPOINT ["dotnet", "bobweb.dll"]
