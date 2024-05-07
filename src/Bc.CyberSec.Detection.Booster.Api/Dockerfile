FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

COPY . ./
RUN dotnet restore Bc.CyberSec.Detection.Booster.Api/Bc.CyberSec.Detection.Booster.Api.csproj
RUN dotnet publish Bc.CyberSec.Detection.Booster.Api/Bc.CyberSec.Detection.Booster.Api.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "Bc.CyberSec.Detection.Booster.Api.dll"]