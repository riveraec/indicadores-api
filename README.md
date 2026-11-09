# Indicadores API

API REST para gestión de **indicadores de desempeño** (métricas). Proyecto demo con **.NET 8**, **EF Core**, **SQLite**, **ProblemDetails**, **Swagger** y pruebas **xUnit**.

Incluye CRUD con filtros y un endpoint de **resumen** (agregados por categoría): cantidad, promedio, mínimo y máximo.

**Autor:** [Efrain Rivera](https://www.linkedin.com/in/riveraec/) · [Portafolio](https://riveraec.github.io/portafolio/)

---

## Stack tecnológico

- .NET 8 / ASP.NET Core Web API
- Entity Framework Core 8
- SQLite (archivo local `indicadores.db`)
- Data Annotations (validación)
- Swashbuckle (OpenAPI / Swagger UI)
- Health Checks (DbContext)
- xUnit + EF InMemory

---

## Arquitectura

```
Cliente HTTP (navegador / Postman / curl)
        ↓
IndicadoresController     ← capa REST
        ↓
IndicadorService          ← lógica de negocio + resumen
        ↓
IndicadorRepository       ← EF Core
        ↓
SQLite (tabla indicadores)
```

**Carpetas principales:**

| Carpeta | Responsabilidad |
|---------|-----------------|
| `Controllers` | Endpoints REST |
| `Services` | Reglas de negocio y agregaciones |
| `Repositories` | Acceso a datos (EF Core) |
| `Models/Entities` | Entidad `Indicador` |
| `Models/Dtos` | Request, Response, Filtro, Resumen |
| `Data` | `DbContext` y seed |
| `Exceptions` | 404 → ProblemDetails |
| `Config` | OpenAPI / Swagger |

---

## Requisitos

- .NET SDK 8
- Git (opcional, para clonar)

```powershell
dotnet --version
```

---

## Ejecutar

```powershell
cd indicadores-api
dotnet run --project src/Indicadores.Api --launch-profile http
```

La API inicia en `http://localhost:5080`.

Al arrancar:

1. Crea el esquema SQLite si no existe (`EnsureCreated`)
2. Carga 10 indicadores de ejemplo si la tabla está vacía (seed idempotente)

### Docker (si Windows bloquea el `.exe` / Smart App Control)

```powershell
docker build -t indicadores-api:local .
docker run --rm -p 5080:8080 --name indicadores-api indicadores-api:local
```

Misma URL: http://localhost:5080/swagger

---

## URLs útiles

| Recurso | URL |
|---------|-----|
| Listar indicadores | http://localhost:5080/api/indicadores |
| Resumen por categoría | http://localhost:5080/api/indicadores/resumen |
| Health | http://localhost:5080/health |
| Swagger UI | http://localhost:5080/swagger |
| OpenAPI JSON | http://localhost:5080/swagger/v1/swagger.json |

También puedes usar `src/Indicadores.Api/Indicadores.Api.http` desde Cursor / Visual Studio.

---

## Endpoints

| Método | Ruta | Descripción |
|--------|------|-------------|
| `GET` | `/api/indicadores` | Listar (`?categoria=&desde=&hasta=`) |
| `GET` | `/api/indicadores/resumen` | Agregados por categoría |
| `GET` | `/api/indicadores/{id}` | Detalle por id |
| `POST` | `/api/indicadores` | Crear (201 Created) |
| `PUT` | `/api/indicadores/{id}` | Actualizar |
| `DELETE` | `/api/indicadores/{id}` | Eliminar (204 No Content) |
| `GET` | `/health` | Health check (SQLite) |

---

## Ejemplos con curl

**Health:**

```powershell
curl http://localhost:5080/health
```

**Listar:**

```powershell
curl http://localhost:5080/api/indicadores
```

**Filtrar por categoría:**

```powershell
curl "http://localhost:5080/api/indicadores?categoria=latencia"
```

**Resumen:**

```powershell
curl http://localhost:5080/api/indicadores/resumen
```

**Crear:**

```powershell
curl -X POST http://localhost:5080/api/indicadores `
  -H "Content-Type: application/json" `
  -d '{"nombre":"Latencia p95 demo","categoria":"latencia","valor":110.5,"unidad":"ms","fechaMedicion":"2026-08-02T18:00:00Z","origen":"manual"}'
```

**Detalle / 404:**

```powershell
curl http://localhost:5080/api/indicadores/1
curl http://localhost:5080/api/indicadores/99999
```

**Actualizar:**

```powershell
curl -X PUT http://localhost:5080/api/indicadores/1 `
  -H "Content-Type: application/json" `
  -d '{"nombre":"Latencia p95 actualizada","categoria":"latencia","valor":95.0,"unidad":"ms","fechaMedicion":"2026-08-02T19:00:00Z","origen":"manual"}'
```

**Eliminar:**

```powershell
curl -X DELETE http://localhost:5080/api/indicadores/1
```

---

## Errores

Los errores siguen **ProblemDetails** (RFC 7807):

- `400` — validación fallida (`ValidationProblemDetails`)
- `404` — indicador no encontrado

Ejemplo 404:

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Recurso no encontrado",
  "status": 404,
  "detail": "Indicador no encontrado con id: 99999",
  "instance": "/api/indicadores/99999"
}
```

---

## Pruebas

```powershell
dotnet test
```

Cubre el servicio: resumen (agregados y filtro de fechas), 404, crear y filtro por categoría (EF InMemory).

---

## Estructura del proyecto

```
indicadores-api/
├── Indicadores.Api.sln
├── src/Indicadores.Api/
│   ├── Program.cs
│   ├── Controllers/
│   ├── Services/
│   ├── Repositories/
│   ├── Models/
│   ├── Data/
│   ├── Exceptions/
│   ├── Config/
│   └── Properties/
└── tests/Indicadores.Api.Tests/
    └── IndicadorServiceTests.cs
```

---

## Notas

- SQLite es un **archivo local** (`indicadores.db`), no Docker ni SQL Server. Persiste entre reinicios.
- El seed solo corre si la tabla está vacía. Para regenerarlo, borra `indicadores.db` y vuelve a arrancar.
- Categorías de ejemplo: `latencia`, `disponibilidad`, `throughput`, `errores`, `batch`.
- Proyecto educativo/demo — no usar en producción.
