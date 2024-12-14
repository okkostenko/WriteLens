FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env

WORKDIR /app

COPY ../WriteLens.Shared/ ./WriteLens.Shared/
COPY WriteLens.ReadabilityService/WriteLens.ReadabilityService.csproj ./WriteLens.ReadabilityService/

RUN dotnet restore WriteLens.ReadabilityService/WriteLens.ReadabilityService.csproj

COPY WriteLens.Shared/ ./WriteLens/Shared/
COPY WriteLens.ReadabilityService/ ./WriteLens.ReadabilityService/

WORKDIR /app/WriteLens.ReadabilityService
RUN dotnet publish -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build-env /app/out ./

EXPOSE 80
ENTRYPOINT [ "dotnet", "WriteLens.ReadabilityService.dll" ]