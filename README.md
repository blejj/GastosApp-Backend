# GastosApp — Backend (.NET 8)

API REST para gestión de gastos personales, construida con Clean Architecture.

## Estructura

```
src/
├── GastosApp.Domain          → Entidades (Usuario, Gasto, Categoria) y enums
├── GastosApp.Application     → DTOs, interfaces, servicios y validadores
├── GastosApp.Infrastructure  → EF Core, repositorios, generación de JWT
└── GastosApp.API             → Controllers, Program.cs, middleware
```

## Requisitos

- .NET 8 SDK
- Docker (opcional, para correr todo con `docker-compose`)
- SQL Server o PostgreSQL (cambiar en `Program.cs` si preferís Postgres:
  reemplazar `UseSqlServer` por `UseNpgsql` y ajustar el paquete correspondiente)

## Correr localmente (sin Docker)

```bash
cd src/GastosApp.API
dotnet restore
dotnet ef database update    # aplica las migraciones (requiere dotnet-ef instalado)
dotnet run
```

La API queda disponible en `https://localhost:5001` (o el puerto que asigne Kestrel),
con Swagger en `/swagger`.

## Correr con Docker

```bash
docker-compose up --build
```

Esto levanta la base SQL Server y la API en el puerto `8080`.

## Crear la primera migración

```bash
dotnet tool install --global dotnet-ef   # si no lo tenés instalado
cd src/GastosApp.API
dotnet ef migrations add InicialSetup -p ../GastosApp.Infrastructure -s .
dotnet ef database update -p ../GastosApp.Infrastructure -s .
```

## Endpoints principales

| Método | Ruta | Descripción | Auth |
|---|---|---|---|
| POST | `/api/auth/register` | Registrar usuario | No |
| POST | `/api/auth/login` | Login, devuelve JWT | No |
| GET | `/api/gastos` | Listar gastos (filtros: desde, hasta, categoriaId, tipo, pagina) | Sí |
| POST | `/api/gastos` | Crear gasto | Sí |
| PUT | `/api/gastos/{id}` | Actualizar gasto | Sí |
| DELETE | `/api/gastos/{id}` | Eliminar gasto | Sí |
| GET | `/api/categorias` | Listar categorías del usuario | Sí |
| POST | `/api/categorias` | Crear categoría | Sí |
| GET | `/api/reportes/resumen-mensual?anio=2026&mes=9` | Ingresos, gastos y balance del mes | Sí |
| GET | `/api/reportes/por-categoria` | Totales agrupados por categoría | Sí |

## Variables de entorno / configuración (`appsettings.json`)

- `ConnectionStrings:DefaultConnection` — cadena de conexión a la base.
- `Jwt:Key` — clave secreta para firmar los tokens (**cambiarla** antes de producción).
- `Jwt:Issuer` / `Jwt:Audience` — identificadores del token.
- `FrontendUrl` — origen permitido por CORS (la URL de tu app React).

## Notas de seguridad antes de producción

- Mover `Jwt:Key` y la cadena de conexión a Azure Key Vault / AWS Secrets Manager, o a variables de entorno — nunca commitear secretos reales en `appsettings.json`.
- Habilitar HTTPS obligatorio en el hosting final.
- Agregar rate limiting en los endpoints de auth para evitar fuerza bruta.

## Próximos pasos sugeridos

- Tests unitarios (xUnit + Moq) sobre `AuthService` y `GastoService`.
- Paginación ya implementada en `/api/gastos`; falta exponer el total de páginas en la respuesta si hace falta.
- Logging estructurado con Serilog.
- Pipeline de CI/CD con GitHub Actions (build, test, build de imagen Docker, deploy a Azure App Service).
