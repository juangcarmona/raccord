# 5. Building block view

## Level 1 — deployable and projects

```text
Raccord.Aspire ──(orchestrates)──► Raccord.Web, PostgreSQL, Azurite, Vite dev server
Raccord.Frontend (React) ──HTTP──► Raccord.Web

Raccord.Web ─────────► module implementations ─► module contracts ─► Raccord.SharedKernel
     ├───────────────► Workspace module (settings and series)
     ├───────────────► Projects module (projects and billable work)
     ├───────────────► Invoicing module (drafts, numbering and issued invoices)
     └───────────────► Raccord.BuildingBlocks ─► Raccord.SharedKernel
     └───────────────► Raccord.ServiceDefaults (health, telemetry, resilience)
```

| Block | Responsibility | Public surface |
| --- | --- | --- |
| `Raccord.SharedKernel` | BCL-only shared values and ports | Money, parties, pricing, clock, current user and audit append |
| `Raccord.Modules.*.Contracts` | Small module seams | Commands, snapshots and ports |
| `Raccord.Modules.*` | Module use cases and persistence | Module workflows, aggregates and adapters |
| `Raccord.Modules.Workspace` | Workspace settings and invoice series | `OrganizationSettings`; Workspace contracts and domain rules |
| `Raccord.Modules.Projects` | Projects and billable work | `ProjectWorkspace`; Projects contracts and project rules |
| `Raccord.Modules.Invoicing` | Drafts, numbering, issued invoices and documents | `InvoicingWorkflow`; Invoicing contracts and finalization rules |
| `Raccord.BuildingBlocks` | Shared technical seams | Sender, problem mapping, storage adapters and shared transaction coordinator |
| `Raccord.Web` | Root HTTP host, SPA hosting and `migrate` command | `/api/*`, `/health/*`, `/openapi/v1.json` |
| `Raccord.Frontend` | User interface | Routes under `/` |

Project coordination is owned by the Projects module. `ProjectCoordination` persists request lifecycle records and reusable project types/teams, while `ProjectWorkspace` owns the resulting project and billable work.

<!-- pdac:cite id="UC-PRJ-RECEIVE-PROJECT-REQUEST" digest="sha256:dd19f461f00bac47b39603582fa87cdd8f3ad836ca659c0fce729ca27b1e7050" -->

## Level 2 — domain modules

| Module (namespace) | Key types | Rules |
| --- | --- | --- |
| `Common` | `Money`, `Currency`, `BusinessRuleViolationException` | Rounding, currency consistency, stable rule codes |
| `Pricing` | `LinePricing`, `VatRate`, `InvoiceTotals`, `VatGroup` | Line amounts, VAT per rate, totals |
| `Parties` | `LegalParty`, `PostalAddress`, `TaxId`, `AddressFields` | Normalization, completeness |
| `Customers` | `Customer` | Registration, legal snapshot |
| `Workspace` | `IssuerProfile`, `InvoiceSeries` | Mandatory issuer details and series definitions |
| `Projects` | `Project`, `BillableLine`, `BillingState` | Periods, line order, double-billing protection |
| `Billing` | `InvoiceDraft`, `InvoiceNumberSequence`, `IssuedInvoice`, `IssuedInvoiceRepresentation` | Validation, numbering, finalization, immutability, fingerprint |
| `Auditing` | `AuditEvent` | Append-only facts |

## Level 2 — application ports and adapters

| Port | Adapter | Notes |
| --- | --- | --- |
| `ICustomerRepository`, `IProjectRepository`, `IInvoiceDraftRepository`, `IIssuedInvoiceRepository`, `IInvoiceSeriesCatalog`, `IInvoiceNumberSequenceRepository`, `IWorkspaceStore`, `IAuditTrail`, `IUnitOfWork` | EF Core repositories over `RaccordDbContext` | Workspace owns settings and series; Invoicing consumes contracts and retains yearly numbering |
| `IInvoiceDocumentRenderer` | `PdfInvoiceRenderer` | Deterministic PDF |
| `IDocumentStore` | `FileSystemDocumentStore`, `AzureBlobDocumentStore` | `Storage:Provider` |
| `IComplianceRecorder` | `PendingComplianceRecorder` | See compliance seam |
| `IClock` | `SystemClock` | UTC now; "today" in `Raccord:TimeZone` |
| `ICurrentUser` | `HttpCurrentActor` (Web) | Authenticated cookie identity, or `unauthenticated` in the explicit test bypass |

## Level 2 — frontend

```text
src/app        shell, routes, not-found, authenticated route boundary
src/features   auth | projects | customers | invoicing | organization
src/shared     api (generated schema + typed client) | i18n | format | hooks | ui
```

The frontend entry point (`src/main.tsx`) composes `AuthProvider` around the router. `RequireAuth` protects the application shell and feature routes; the sign-in feature remains public.
