# 5. Building block view

## Level 1 — deployable and projects

```text
Raccord.Aspire ──(orchestrates)──► Raccord.Web, PostgreSQL, Azurite, Vite dev server
Raccord.Frontend (React) ──HTTP──► Raccord.Web

Raccord.Web ─────────► Raccord.Application ─► Raccord.Domain ─► Raccord.SharedKernel
     │   └───────────► Raccord.Contracts ◄────┘ (Application returns contracts)
     ├───────────────► Workspace module (settings and series)
     ├───────────────► Projects module (projects and billable work)
     ├───────────────► Invoicing module (drafts, numbering and issued invoices)
     ├───────────────► Raccord.Infrastructure ─► Application, Domain, Contracts, BuildingBlocks
     └───────────────► Raccord.BuildingBlocks ─► Raccord.SharedKernel
     └───────────────► Raccord.ServiceDefaults (health, telemetry, resilience)
```

| Block | Responsibility | Public surface |
| --- | --- | --- |
| `Raccord.SharedKernel` | BCL-only shared values and ports | Money, parties, pricing, clock, current user and audit append |
| `Raccord.Domain` | Business rules; no framework dependencies | Aggregates and module rules |
| `Raccord.Contracts` | Public HTTP contracts (records) | Request/response records |
| `Raccord.Application` | Use cases and ports | `InvoicingWorkflow`, `AuditLog`; port interfaces |
| `Raccord.Modules.Workspace` | Workspace settings and invoice series | `OrganizationSettings`; Workspace contracts and domain rules |
| `Raccord.Modules.Projects` | Projects and billable work | `ProjectWorkspace`; Projects contracts and project rules |
| `Raccord.Modules.Invoicing` | Drafts, numbering, issued invoices and documents | `InvoicingWorkflow`; Invoicing contracts and finalization rules |
| `Raccord.BuildingBlocks` | Shared technical seams | Sender, problem mapping, storage adapters and shared transaction coordinator |
| `Raccord.Infrastructure` | Adapters | `AddRaccordInfrastructure`, `DatabaseMaintenance`, `RaccordDbContext` |
| `Raccord.Web` | HTTP endpoints, error mapping, SPA hosting, `migrate` command | `/api/*`, `/health/*`, `/openapi/v1.json` |
| `Raccord.Frontend` | User interface | Routes under `/` |

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
| `ICurrentUser` | `HttpCurrentActor` (Web) | `unauthenticated` until sign-in exists |

## Level 2 — frontend

```text
src/app        shell, routes, not-found
src/features   projects | customers | invoicing | organization
src/shared     api (generated schema + typed client) | i18n | format | hooks | ui
```
