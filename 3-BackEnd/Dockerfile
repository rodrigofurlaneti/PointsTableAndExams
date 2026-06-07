# ── Build stage ────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy all source files and publish (restore is implicit)
COPY src/ src/
RUN dotnet publish src/PointsTableAndExams.Api/PointsTableAndExams.Api.csproj \
    -c Release -o /app/publish

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
