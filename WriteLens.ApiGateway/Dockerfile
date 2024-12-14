FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env

WORKDIR /app

COPY ../WriteLens.Shared/ ./WriteLens.Shared/
COPY WriteLens.ApiGateway/WriteLens.ApiGateway.csproj ./WriteLens.ApiGateway/

RUN dotnet restore WriteLens.ApiGateway/WriteLens.ApiGateway.csproj

COPY WriteLens.Shared/ ./WriteLens/Shared/
COPY WriteLens.ApiGateway/ ./WriteLens.ApiGateway/

WORKDIR /app/WriteLens.ApiGateway
RUN dotnet publish -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build-env /app/out ./

EXPOSE 80
ENTRYPOINT [ "dotnet", "WriteLens.ApiGateway.dll" ]