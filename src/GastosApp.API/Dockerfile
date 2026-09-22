# ---- Etapa de build ----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/GastosApp.API/GastosApp.API.csproj", "GastosApp.API/"]
COPY ["src/GastosApp.Application/GastosApp.Application.csproj", "GastosApp.Application/"]
COPY ["src/GastosApp.Infrastructure/GastosApp.Infrastructure.csproj", "GastosApp.Infrastructure/"]
COPY ["src/GastosApp.Domain/GastosApp.Domain.csproj", "GastosApp.Domain/"]

RUN dotnet restore "GastosApp.API/GastosApp.API.csproj"

COPY src/ .
WORKDIR /src/GastosApp.API
RUN dotnet publish -c Release -o /app/publish

# ---- Etapa final ----
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "GastosApp.API.dll"]
