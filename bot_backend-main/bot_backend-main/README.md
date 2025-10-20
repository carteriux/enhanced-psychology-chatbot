# 📦 CPC Backend API

Este proyecto es una solución backend construida en **.NET** que sigue una arquitectura en capas modularizada para ofrecer servicios de negocio relacionados con CPC (Customer/Product/Catalog, según interpretación). Utiliza Entity Framework con MySQL, JWT para autenticación, y está preparado para despliegue en contenedores con Docker.

---

## 🧱 Estructura del Proyecto

La solución incluye los siguientes proyectos:

- **CPC.API**: Proyecto principal de API en ASP.NET Core.
- **CPC.Application**: Contiene la lógica de aplicación (casos de uso, DTOs, etc).
- **CPC.Domain**: Modelos de dominio y objetos de valor.
- **CPC.Common**: Funciones y utilidades compartidas entre capas.
- **CPC.Infrastructure.DataPersistent**: Implementación de acceso a datos (EF Core, contextos, repositorios).
- **CPC.Infrastructure.Crosscutting**: Validaciones, helpers, middleware, logging, etc.

---

## 🚀 Tecnologías

- ASP.NET Core
- Entity Framework Core (MySQL)
- JWT Bearer Authentication
- Docker
- Swagger para documentación de APIs

---

## 🔧 Configuración

### Archivos de configuración

- `appsettings.Development.json`
- `appsettings.Production.json`

Configuraciones importantes:
```json
"APIBaseSettings": {
  "ConnectionStrings": {
    "CPConnection": "cadena-de-conexión-mysql"
  }
}
```

---

## 🐳 Docker

```bash
docker build -t cpc-api .
docker run -p 5000:80 cpc-api
```

---

## 🔐 Autenticación

Usa JWT con el esquema Bearer:
```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => { ... });
```

---

## 📡 Ejemplos de Endpoints

### 🔐 Autenticación

```http
POST /api/Security/login
Content-Type: application/json

{
  "username": "usuario",
  "password": "contraseña"
}
```

Respuesta:
```json
{
  "data": {
    "token": "jwt-token-aquí",
    "expiration": "2025-04-08T00:00:00Z"
  },
  "success": true
}
```

---

### 👤 Usuarios

#### 🔍 Obtener todos los usuarios

```http
GET /api/User
Authorization: Bearer <jwt-token>
```

#### 🔍 Obtener usuario por ID

```http
GET /api/User/5
Authorization: Bearer <jwt-token>
```

#### ➕ Crear un nuevo usuario

```http
POST /api/User
Content-Type: application/json
Authorization: Bearer <jwt-token>

{
  "name": "Juan Pérez",
  "email": "juan@example.com",
  "role": "admin"
}
```

---

## 🧪 Ejecución local

1. Configura la cadena de conexión a MySQL en `appsettings.Development.json`.
2. Ejecuta el proyecto `CPC.API` desde Visual Studio o por CLI:
   ```bash
   dotnet run --project CPC-API/CPC.API.csproj
   ```
3. Accede a la documentación interactiva en `http://localhost:<puerto>/swagger`.

