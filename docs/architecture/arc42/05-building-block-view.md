# 5. Building block view

## Level 1 — deployable and projects

```text
Raccord.Aspire ──(orchestrates)──► Raccord.Web, PostgreSQL, Azurite, Vite dev server
Raccord.Frontend (React) ──HTTP──► Raccord.Web

Raccord.Web ─────────► Raccord.Application ─► Raccord.Domain ─► Raccord.SharedKernel
     │   └───────────► Raccord.Contracts ◄────┘ (Application returns contracts)
     ├───────────────► Raccord.Infrastructure ─► Application, Domain, Contracts, BuildingBlocks
     └───────────────► Raccord.BuildingBlocks ─► Raccord.SharedKernel
     └───────────────► Raccord.ServiceDefaults (health, telemetry, resilience)
```

| Block | Responsibility | Public surface |
| --- | --- | --- |
| `Raccord.SharedKernel` | BCL-only shared values and ports | Money, parties, pricing, clock, current user and audit append |
| `Raccord.Domain` | Business rules; no framework dependencies | Aggregates and module rules |
| `Raccord.Contracts` | Public HTTP contracts (records) | Request/response records |
| `Raccord.Application` | Use cases and ports | `CustomerDirectory`, `ProjectWorkspace`, `InvoicingWorkflow`, `OrganizationSettings`, `AuditLog`; port interfaces |
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
| `Organization` | `IssuerProfile` | Mandatory issuer details |
| `Projects` | `Project`, `BillableLine`, `BillingState` | Periods, line order, double-billing protection |
| `Billing` | `InvoiceDraft`, `InvoiceNumberSequence`, `InvoiceSeries`, `IssuedInvoice`, `IssuedInvoiceRepresentation` | Validation, numbering, finalization, immutability, fingerprint |
| `Auditing` | `AuditEvent` | Append-only facts |

## Level 2 — application ports and adapters

| Port | Adapter | Notes |
| --- | --- | --- |
| `ICustomerRepository`, `IProjectRepository`, `IInvoiceDraftRepository`, `IIssuedInvoiceRepository`, `IInvoiceSeriesRepository`, `IIssuerProfileRepository`, `IAuditTrail`, `IUnitOfWork` | EF Core repositories over `RaccordDbContext` | Unit of work maps concurrency/unique violations to `ConcurrencyConflictException` |
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
