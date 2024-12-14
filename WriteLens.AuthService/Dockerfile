FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env

WORKDIR /app

COPY ../WriteLens.Shared/ ./WriteLens.Shared/
COPY WriteLens.AuthService/WriteLens.AuthService.csproj ./WriteLens.AuthService/

RUN dotnet restore WriteLens.AuthService/WriteLens.AuthService.csproj

COPY WriteLens.Shared/ ./WriteLens/Shared/
COPY WriteLens.AuthService/ ./WriteLens.AuthService/

WORKDIR /app/WriteLens.AuthService
RUN dotnet publish -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build-env /app/out ./

EXPOSE 80
ENTRYPOINT [ "dotnet", "WriteLens.AuthService.dll" ]