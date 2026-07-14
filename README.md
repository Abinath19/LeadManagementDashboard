# Lead Management Dashboard

A Kanban-style lead management dashboard built with **.NET 10 Razor Pages** and **Entity Framework Core** (SQL Server, Code-First).

Leads are displayed in color-coded columns grouped by status. Users can move leads forward or backward through the pipeline, with every transition validated and logged to a full activity history.

## Features

- Kanban board with one column per status — status name, lead count, background color, and a "No leads" message when empty
- Columns are fully data-driven (`DisplayOrder` / `IsActive`): statuses can be added, reordered, or deactivated without any code changes
- Move Forward / Move Backward on each lead card, with validation (cannot move forward from the last status or backward from the first)
- Every move atomically updates the lead's status and logs an activity (from-status, to-status, timestamp, note)
- Bootstrap toast notifications on success and failure
- Lead detail modal with complete activity history (click any card)
- Responsive Bootstrap 5 layout (1 column on mobile, 2 on tablet, 4 on desktop)

## Architecture

```
LeadManagement/
├── Models/         # EF Core entities: Status, Lead, LeadActivity
├── Data/           # AppDbContext, entity configuration, seed data
├── Migrations/     # EF Core migrations (schema + seed)
├── Services/       # ILeadService / LeadService — all business logic
├── ViewModels/     # DTOs for the pages (EF entities are never exposed to views)
├── Pages/          # Razor Pages (Index = Kanban dashboard)
└── wwwroot/        # Static assets
```

Design notes:

- **Service layer** — status transition rules, validation, and activity logging live in `LeadService`, not in PageModels. Pages only handle HTTP concerns.
- **ViewModels only** — views receive precomputed, display-shaped data (e.g. `IsFirst`/`IsLast` flags decide which move buttons render).
- **Atomic moves** — the status update and its activity log entry are saved in a single `SaveChangesAsync`, so they commit or fail together.
- **`DeleteBehavior.Restrict`** on status foreign keys — avoids multiple-cascade-path errors and protects activity history.
- **Nullable `FromStatusId`** — represents the initial "Lead created" activity, which has no prior status.
- **`Company` field** — not in the assignment's table spec, but required by the lead card spec, so it was added to `Leads`.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server — LocalDB (installed with Visual Studio) works out of the box
- EF Core tooling (one of):
  - Visual Studio: `Microsoft.EntityFrameworkCore.Tools` NuGet package (already referenced)
  - CLI: `dotnet tool install --global dotnet-ef`

## Getting started

**1. Clone and restore**

```bash
git clone <this-repo-url>
cd LeadManagement
dotnet restore
```

**2. Configure the connection string** (optional)

The default in `appsettings.json` targets LocalDB:

```
Server=(localdb)\MSSQLLocalDB;Database=LeadManagementDb;Trusted_Connection=True;TrustServerCertificate=True
```

Point it at your own SQL Server instance if needed.

**3. Create the database**

The migration (including all seed data) is committed in `Migrations/`, so you only need to apply it:

Visual Studio — *Package Manager Console*:

```powershell
Update-Database
```

or CLI:

```bash
dotnet ef database update
```

**4. Run**

```bash
dotnet run
```

Open the URL printed in the console (e.g. `https://localhost:5001`).

## Seed data

Applying the migration seeds:

| Data | Details |
|---|---|
| 4 statuses | New (blue), Contacted (yellow), Qualified (green), Closed (gray) |
| 5 leads | Distributed across all statuses |
| Activity history | Full transition history for 2 leads, including "Lead created" entries |

## Testing the flow

1. Move a lead forward — green toast, card moves, activity logged
2. Try moving past the last status / before the first — blocked (buttons hidden; server-side validation also rejects forged requests with an error toast)
3. Click any lead card — modal shows lead details and complete activity history, newest first

## Tech stack

.NET 10 · Razor Pages · Entity Framework Core (Code-First, SQL Server) · Bootstrap 5
