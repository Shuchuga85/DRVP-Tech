# 🎭 FlowArtes — Ent'Artes Dance School Management Platform

> **Branch:** `dev` — active development. See `main` for the last stable release.

FlowArtes is a full-stack web application built for **Ent'Artes**, a real performing-arts school in Portugal. It replaces a manual Excel and Google Sheets workflow with a structured platform covering private coaching sessions, studio scheduling, inventory management, user accounts, events, and automated billing reconciliation.

---

## 👥 Team — DRVP Tech

| Name | Student No. |
|------|-------------|
| João Pereira | a31505 |
| Paulo Silva | a31506 |
| David Faria | a31517 |
| Vítor Rezende | a31521 |
| Rafael Costa | a31524 |

**Institution:** Instituto Politécnico do Cávado e do Ave — Escola Superior de Tecnologia  
**Degree:** Licenciatura em Engenharia de Sistemas Informáticos  
**Academic Year:** 2025/2026

---

## 🛠️ Tech Stack

| Layer | Technology |
|-------|------------|
| Backend | ASP.NET Core 10, EF Core 10 |
| Database | SQL Server (Azure), SQLite (tests) |
| Frontend | React 19, Vite, React Router 7 |
| Auth | JWT (HttpOnly cookie) |
| Tests | xUnit, FluentAssertions, Moq, WebApplicationFactory |

---

## 📂 Project Structure

```
DanceSchoolApp.slnx
├── DanceSchoolApp.Server/      ← ASP.NET Core API + React SPA host
│   ├── Controllers/            ← Domain controllers (Classes, People, School, …)
│   ├── Services/               ← Business logic (auto-registered via reflection)
│   ├── Models/                 ← EF Core entities
│   ├── DTOs/                   ← Request/Response shapes
│   ├── Data/AppDbContext.cs
│   └── Program.cs
├── danceschoolapp.client/      ← React 19 + Vite SPA
├── DanceSchoolApp.Tests/       ← xUnit unit + integration tests
│   ├── Unit/                   ← Service-layer tests (no HTTP, no DB)
│   └── Integration/            ← Full HTTP tests against SQLite in-memory
├── Doc/                        ← API documentation
│   ├── V1BasicAPI.md
│   ├── V2AdvancedAPI.md
│   ├── V3TestsFramework.md
│   └── EndpointsMapping.md
├── PostmanCfg/                 ← Postman collection
└── script.sql                  ← Full DDL for SQL Server
```

---

## ⚙️ Environment Variables

| Variable | Purpose |
|----------|---------|
| `DanceSchoolApp_DB` | SQL Server connection string |
| `DanceSchoolApp_JWT_Secret` | JWT signing secret (min 32 chars) |
| `DanceSchoolApp_Email_Password` | SMTP password (optional — skipped in tests) |

---

## 🚀 Running the Project

### Backend

```bash
cd DanceSchoolApp.Server
dotnet build
dotnet run          # API on https://localhost:7003, React proxy on https://localhost:5173
```

### Frontend (standalone dev)

```bash
cd danceschoolapp.client
npm install
npm run dev
```

### Full stack (Visual Studio)

Open `DanceSchoolApp.slnx` and run the Server project. It starts both the API and the Vite dev server via the SPA proxy.

### Database

```bash
# Apply schema to a new SQL Server instance
# Run script.sql in SSMS or Azure Data Studio

# If you add a migration (code-first change):
dotnet ef migrations add <MigrationName> --project DanceSchoolApp.Server
dotnet ef database update --project DanceSchoolApp.Server
```

---

## 🧪 Tests

```bash
# All tests
dotnet test

# Unit tests only (fast, no HTTP)
dotnet test --filter "Category=Unit"

# Integration tests only (SQLite in-memory, sequential)
dotnet test --filter "Category=Integration"
```

Integration tests are configured to run **sequentially** (no parallelism) to avoid SQLite connection-registry race conditions. Each test class gets its own isolated named in-memory SQLite database.

---

## 📖 Documentation

| Document | Description |
|----------|-------------|
| [V1BasicAPI.md](Doc/V1BasicAPI.md) | All primitive REST endpoints (auth, users, classes, billing, …) |
| [V2AdvancedAPI.md](Doc/V2AdvancedAPI.md) | Role-scoped portal controllers (`/api/staff/*`, `/api/ee/*`, `/api/coach/*`, `/api/admin/*`) |
| [V3TestsFramework.md](Doc/V3TestsFramework.md) | Testing architecture, bug fixes, and automation added in patch v3 |
| [EndpointsMapping.md](Doc/EndpointsMapping.md) | Page-by-page map from every React route to its API calls |
| [PostmanCfg/](PostmanCfg/) | Full Postman collection with flows and debug endpoints |

---

## 🔄 Class Lifecycle

```
Requested(0) → StaffApproved(7) → Approved(1) ──→ Finished(4) ──→ Pending(6) → Validated(5)
                                ↘ Rejected(2)      ↑ automated    ↑ automated
Requested(0) → Rejected(2)                         after end      after 48h window
Requested(0) | Approved(1) → Cancelled(3)          datetime       expires
```

The `Approved → Finished` and `Finished → Pending` transitions are **automated** by `ClassLifecycleWorker` (a background service polling every 5 minutes). Cancelled classes are never touched by the worker.

---

## 🔑 Roles

| role_id | role_name | Access |
|---------|-----------|--------|
| 0 | admin | Platform administration |
| 1 | staff | School management (Direção) |
| 2 | coach | Teaching staff (Professor) |
| 3 | parent | Guardians (Encarregado de Educação) |