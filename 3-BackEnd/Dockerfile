# ── Build stage ────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Restore
COPY PointsTableAndExams.sln ./
COPY src/PointsTableAndExams.Domain/PointsTableAndExams.Domain.csproj             src/PointsTableAndExams.Domain/
COPY src/PointsTableAndExams.Application/PointsTableAndExams.Application.csproj   src/PointsTableAndExams.Application/
COPY src/PointsTableAndExams.Infrastructure/PointsTableAndExams.Infrastructure.csproj src/PointsTableAndExams.Infrastructure/
COPY src/PointsTableAndExams.Api/PointsTableAndExams.Api.csproj                   src/PointsTableAndExams.Api/
RUN dotnet restore src/PointsTableAndExams.Api/PointsTableAndExams.Api.csproj

# Build & publish
COPY src/ src/
RUN dotnet publish src/PointsTableAndExams.Api/PointsTableAndExams.Api.csproj \
    -c Release -o /app/publish --no-restore

# ── Runtime stage ───────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

LABEL org.opencontainers.image.source="https://github.com/rodrigofurlaneti/PointsTableAndExams"
LABEL org.opencontainers.image.description="VitaLog API — .NET 9 Clean Architecture"
LABEL org.opencontainers.image.licenses="MIT"

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "PointsTableAndExams.Api.dll"]
