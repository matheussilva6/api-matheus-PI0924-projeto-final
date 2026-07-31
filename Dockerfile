# --- Fase 1: build ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar apenas o .csproj primeiro (aproveita cache do Docker no restore)
COPY ApiMatheusProjetoFinal/ApiMatheusProjetoFinal.csproj ./ApiMatheusProjetoFinal/
RUN dotnet restore ./ApiMatheusProjetoFinal/ApiMatheusProjetoFinal.csproj

# Copiar o resto do código e publicar
COPY ApiMatheusProjetoFinal/. ./ApiMatheusProjetoFinal/
WORKDIR /src/ApiMatheusProjetoFinal
RUN dotnet publish -c Release -o /app/publish --no-restore

# --- Fase 2: imagem final (só o runtime, mais leve) ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "ApiMatheusProjetoFinal.dll"]
