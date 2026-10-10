# syntax=docker/dockerfile:1
# Build context: repository root.  docker build -f deploy/docker/Dockerfile -t raccord:local .

FROM node:24-alpine AS frontend
WORKDIR /src/frontend
COPY src/Raccord.Frontend/package.json src/Raccord.Frontend/package-lock.json ./
RUN npm ci --no-fund --no-audit
COPY src/Raccord.Frontend/ ./
RUN npm run build

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS backend
WORKDIR /src
COPY global.json Directory.Build.props Directory.Packages.props .editorconfig Raccord.slnx ./
COPY src/Raccord.Domain/Raccord.Domain.csproj src/Raccord.Domain/
COPY src/Raccord.Contracts/Raccord.Contracts.csproj src/Raccord.Contracts/
COPY src/Raccord.Application/Raccord.Application.csproj src/Raccord.Application/
COPY src/Raccord.Infrastructure/Raccord.Infrastructure.csproj src/Raccord.Infrastructure/
COPY src/Raccord.ServiceDefaults/Raccord.ServiceDefaults.csproj src/Raccord.ServiceDefaults/
COPY src/Raccord.Web/Raccord.Web.csproj src/Raccord.Web/
RUN dotnet restore src/Raccord.Web/Raccord.Web.csproj
COPY src/ src/
RUN dotnet publish src/Raccord.Web/Raccord.Web.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=backend /app ./
COPY --from=frontend /src/frontend/dist ./wwwroot
RUN mkdir -p /var/lib/raccord/documents && chown -R app:app /var/lib/raccord
ENV ASPNETCORE_URLS=http://+:8080 \
    ASPNETCORE_ENVIRONMENT=Production \
    Storage__Provider=FileSystem \
    Storage__FileSystemRoot=/var/lib/raccord/documents
USER app
EXPOSE 8080
ENTRYPOINT ["dotnet", "Raccord.Web.dll"]
