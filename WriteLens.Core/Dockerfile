FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env

WORKDIR /app

COPY ../WriteLens.Shared/ ./WriteLens.Shared/

COPY WriteLens.Core/WriteLens.Core.csproj ./WriteLens.Core/

RUN dotnet restore WriteLens.Core/WriteLens.Core.csproj

COPY WriteLens.Shared/ ./WriteLens/Shared/
COPY WriteLens.Core/ ./WriteLens.Core/

WORKDIR /app/WriteLens.Core
RUN dotnet publish -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build-env /app/out ./

EXPOSE 80
ENTRYPOINT ["sh", "-c", "dotnet WriteLens.Core.dll && dotnet ef database update"]