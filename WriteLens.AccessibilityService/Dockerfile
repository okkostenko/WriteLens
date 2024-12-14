FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env

WORKDIR /app

COPY ../WriteLens.Shared/ ./WriteLens.Shared/
COPY WriteLens.AccessibilityService/WriteLens.AccessibilityService.csproj ./WriteLens.AccessibilityService/

RUN dotnet restore WriteLens.AccessibilityService/WriteLens.AccessibilityService.csproj

COPY WriteLens.Shared/ ./WriteLens/Shared/
COPY WriteLens.AccessibilityService/ ./WriteLens.AccessibilityService/

WORKDIR /app/WriteLens.AccessibilityService
RUN dotnet publish -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build-env /app/out ./

EXPOSE 80
ENTRYPOINT [ "dotnet", "WriteLens.AccessibilityService.dll" ]