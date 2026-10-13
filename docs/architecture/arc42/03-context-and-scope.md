# 3. Context and scope

## Business context

```text
                 ┌──────────────────────────────┐
 Operations ────►│                              │──► Invoice PDFs (download; e-mail: MVP)
 Billing staff ─►│           Raccord            │──► Accounting exports (MVP)
 Owners ────────►│  (system of record: projects,│──► AEAT VERI*FACTU web services (MVP, after review)
 Warehouse/crew ►│   customers, invoices, audit)│◄─► B2B e-invoice platforms (Next)
 (later)         │                              │◄── Customer-provided import files (optional, Next)
                 └──────────────────────────────┘
```

| Partner | Exchange | Status |
| --- | --- | --- |
| Users (browser) | HTTPS, React UI over the JSON API | Implemented |
| Accountant | Export files | MVP |
| AEAT | Billing records over SOAP/XML web services with certificate | Seam only (see [compliance seam](../compliance-seam.md)) |
| E-invoice platforms | UBL/EN 16931 and status messages | Next |
| Mail server | SMTP for invoice delivery | MVP |
| Import sources | Spreadsheets/exports supplied by the customer | Optional, isolated |

## Technical context

```text
Browser ──HTTPS──► Raccord.Web (API + static UI) ──► PostgreSQL
                         │                      └──► Object storage (volume | Azure Blob)
                         └──► OTLP collector (optional)
Raccord.Web migrate ───────► PostgreSQL (one-shot job)
```

## Scope

**In scope now:** issuer profile, customers, projects and billable lines, drafts, validation, finalization, numbering, issued representation, PDF, audit, compliance record, deployment (Compose, Aspire, Azure), CI.

**Out of scope now:** role-based authorization, warehouse, crew, quotes, payments, e-mail, VERI*FACTU submission, B2B exchange, general-ledger accounting.
