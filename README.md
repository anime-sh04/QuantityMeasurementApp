# Quantity Measurement - Microservices Architecture

Refactored from a single monolithic ASP.NET Core 9 backend into **4 independent microservices**.

---

## Architecture Overview

```
                        ┌──────────────────────────────────┐
  All Client Traffic    │       API Gateway  :5000         │
  ──────────────────►   │  YARP reverse-proxy + JWT inject │
                        │  Swagger UI at http://localhost:5000 │
                        └───────┬──────────┬──────────┬────┘
                                │          │          │
                         :5001  │   :5002  │   :5003  │
                    ┌───────────┴┐ ┌───────┴───┐ ┌────┴──────────┐
                    │  Auth      │ │Conversion │ │  History      │
                    │  Service   │ │ Service   │ │  Service      │
                    │            │ │           │ │               │
                    │ /register  │ │ /compare  │ │ /history      │
                    │ /login     │ │ /add      │ │ /history/{op} │
                    │ /google    │ │ /subtract │ │ /history/type │
                    │ /me        │ │ /divide   │ │ /stats        │
                    └────────────┘ │ /convert  │ │ /delete       │
                                   └───────────┘ └───────────────┘
                                         │               │
                                         └───────┬───────┘
                                         PostgreSQL (shared DB,
                                         separate tables per domain)
```

---

## Services

| Service | Port | Responsibility |
|---|---|---|
| **API Gateway** | `5000` | YARP reverse proxy, JWT forwarding, single Swagger UI |
| **Auth Service** | `5001` | Register, Login, Google OAuth, JWT issuance, `/me` |
| **Conversion Service** | `5002` | Compare, Add, Subtract, Divide, Convert — writes history |
| **History Service** | `5003` | Read / filter / delete measurement history, stats |

---

## Running with Docker Compose

```bash
cd microservices
docker-compose up --build
```

Then open **http://localhost:5000** for the unified Swagger UI.

Each service also exposes its own Swagger at:
- http://localhost:5001/swagger  (Auth)
- http://localhost:5002/swagger  (Conversion)
- http://localhost:5003/swagger  (History)

---

## Running Locally (without Docker)

Open 4 terminals:

```bash
# Terminal 1 — Auth Service
cd AuthService && dotnet run

# Terminal 2 — Conversion Service
cd ConversionService && dotnet run

# Terminal 3 — History Service
cd HistoryService && dotnet run

# Terminal 4 — API Gateway (update appsettings.json cluster addresses to localhost)
cd ApiGateway && dotnet run
```

> For local runs, change cluster addresses in `ApiGateway/appsettings.json` from
> `http://auth-service:5001` → `http://localhost:5001` (and so on for the others).

---

## API Reference

### Auth Service — `/api/auth`

| Method | Path | Auth | Description |
|---|---|---|---|
| POST | `/api/auth/register` | ❌ | Create a new account |
| POST | `/api/auth/login` | ❌ | Login, returns JWT |
| POST | `/api/auth/google` | ❌ | Google OAuth login |
| GET  | `/api/auth/me` | ✅ | Current user profile |

**Register example:**
```json
POST /api/auth/register
{
  "email": "user@example.com",
  "password": "password123",
  "firstName": "Jane",
  "lastName": "Doe"
}
```

---

### Conversion Service — `/api/quantity`

All endpoints are public (JWT optional — userId is stored when provided).

| Method | Path | Description |
|---|---|---|
| POST | `/api/quantity/compare`  | Are two quantities equal? |
| POST | `/api/quantity/add`      | Sum in target unit |
| POST | `/api/quantity/subtract` | Difference in target unit |
| POST | `/api/quantity/divide`   | Dimensionless ratio |
| POST | `/api/quantity/convert`  | Convert to different unit |

**Supported measurement types & units:**

| Type | Units |
|---|---|
| `Length` | `Feet` `Inches` `Yards` `Centimeters` |
| `Weight` | `Kilogram` `Gram` `Pound` |
| `Volume` | `Litre` `Millilitre` `Gallon` |
| `Temperature` | `Celsius` `Fahrenheit` `Kelvin` |

**Compare example:**
```json
POST /api/quantity/compare
{
  "quantityOne": { "value": 12, "unit": "Inches", "measurementType": "Length" },
  "quantityTwo": { "value": 1,  "unit": "Feet",   "measurementType": "Length" }
}
```

**Convert example:**
```json
POST /api/quantity/convert
{
  "source": { "value": 100, "unit": "Celsius", "measurementType": "Temperature" },
  "targetUnit": "Fahrenheit"
}
```

---

### History Service — `/api/quantity`

All endpoints require a valid JWT (`Authorization: Bearer <token>`).

| Method | Path | Description |
|---|---|---|
| GET    | `/api/quantity/history`                       | All your history |
| GET    | `/api/quantity/history/operation/{type}`      | Filter by op (Compare/Add/…) |
| GET    | `/api/quantity/history/type/{measurementType}`| Filter by type (Length/…) |
| DELETE | `/api/quantity/history`                       | Delete your history |
| GET    | `/api/quantity/stats`                         | Record counts |

---

## JWT Flow

```
1. Client → POST /api/auth/login          → receives JWT
2. Client → GET  /api/quantity/history    (Authorization: Bearer <jwt>)
3. Gateway validates JWT, injects X-User-Id + X-User-Role headers
4. HistoryService reads X-User-Id to scope results per user
```

---

## Project Structure

```
microservices/
├── QuantityMeasurement.sln
├── docker-compose.yml
│
├── ApiGateway/                  # YARP reverse proxy — port 5000
│   ├── ApiGateway.csproj
│   ├── Program.cs
│   ├── appsettings.json         # YARP routes & clusters
│   ├── Dockerfile
│   └── Middleware/
│       └── JwtForwardingMiddleware.cs
│
├── AuthService/                 # Auth — port 5001
│   ├── AuthService.csproj
│   ├── Program.cs
│   ├── appsettings.json
│   ├── Dockerfile
│   ├── Auth/                    # JwtTokenService, GoogleTokenValidator
│   ├── Controllers/             # AuthController
│   ├── Data/                    # AuthDbContext
│   ├── DTOs/                    # RegisterRequestDTO, LoginRequestDTO …
│   ├── Repository/              # UserRepository
│   ├── Services/                # EncryptionService
│   └── Shared/                  # ApplicationUser, QuantityMeasurementEntity
│
├── ConversionService/           # Conversion logic — port 5002
│   ├── ConversionService.csproj
│   ├── Program.cs
│   ├── appsettings.json
│   ├── Dockerfile
│   ├── Controllers/             # ConversionController
│   ├── Data/                    # ConversionDbContext
│   ├── DTOs/                    # CompareRequestDTO, AddRequestDTO …
│   ├── Exceptions/              # ConversionException
│   ├── Middleware/              # GlobalExceptionMiddleware
│   ├── Repository/              # ConversionRepository
│   ├── Services/                # ConversionServiceImpl
│   ├── Util/                    # UnitConverter
│   └── Shared/                  # QuantityMeasurementEntity
│
└── HistoryService/              # History queries — port 5003
    ├── HistoryService.csproj
    ├── Program.cs
    ├── appsettings.json
    ├── Dockerfile
    ├── Controllers/             # HistoryController
    ├── Data/                    # HistoryDbContext
    ├── Repository/              # HistoryRepository
    └── Shared/                  # QuantityMeasurementEntity
```

---

## Notes

- **Database:** All three backend services share the same PostgreSQL instance (your existing Render DB). `Users` is owned by Auth; `QuantityMeasurements` is owned by Conversion and read by History.
- **JWT secret** is the same across all services so tokens issued by Auth are accepted by Conversion and History without a network call.
- **Gateway JWT forwarding:** The gateway decodes the JWT and injects `X-User-Id` / `X-User-Role` headers. Downstream services can trust these headers since they are set internally.
- **Scaling:** Each service can be independently scaled in Docker Compose / Kubernetes by adjusting `replicas`.
