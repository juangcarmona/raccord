---
title: Quality requirements
arc42-section: "10"
description: Defines measurable quality scenarios and how the architecture realizes them.
---
<!-- markdownlint-disable MD025 -->

# 10. Quality requirements

## Quality tree

```mermaid
mindmap
    accTitle: Quality tree and verification scenario groups
  root((Quality))
    Correctness
      Q-1 Totals
      Q-2 Numbering
    Integrity
      Q-3 Immutability
      Q-4 Tamper detection
      Q-5 Billing conflicts
    Operability
      Q-6 Installation
      Q-7 Restore
    Maintainability
      Q-8 Architecture boundaries
    Portability
      Q-9 Storage and hosting
    Security
      Q-10 Secrets and access
      Q-11 Authentication
      Q-12 Coordination
```

## Scenarios

| ID | Scenario | Expected response | Verified by |
| --- | --- | --- | --- |
| Q-1 | Three lines of 0,15 € at 10 % VAT are invoiced | VAT 0,05 € (base-level), total consistent on screen and PDF | `InvoiceTotalsTests` |
| Q-2 | Six invoices are finalized concurrently | Numbers 1–6, no duplicates, no gaps | `Concurrent_finalizations_never_duplicate_or_skip_numbers` |
| Q-3 | An operator runs `UPDATE issued_invoices …` | The database refuses | `The_database_rejects_changes…` |
| Q-4 | A stored representation is altered with triggers disabled | Invoice not served, critical log, 500 with code | `A_tampered_issued_invoice_is_refused…` |
| Q-5 | Two users bill the same line at the same moment | Exactly one draft gets it | `Concurrent_drafts_for_the_same_line…` |
| Q-6 | An administrator installs with Docker Compose | Healthy stack in minutes from `.env` + one command | Manual (2026-10-06) |
| Q-7 | A backup is restored into a clean stack | Same invoices, identical fingerprints and PDFs, triggers active | Manual rehearsal (2026-10-06) |
| Q-8 | A developer references EF Core from Domain | Architecture tests fail | `DependencyRulesTests` (verified by injection) |
| Q-9 | An installation moves from local files to Azure Blob | Configuration change only (`Storage:Provider`) | Aspire run with Azurite |
| Q-10 | Repository is scanned for secrets | No credentials; `.env` ignored; Azure secrets in Key Vault | Review; CI vulnerability checks |
| Q-11 | An anonymous request reaches a protected API | 401 without a session; sign-in establishes a cookie session; health remains probeable | `AuthenticationJourney` and web authorization tests |
| Q-12 | A request is converted into a project | Source request remains resolved, project keeps period/type/team, and no partial conversion is committed | `ProjectRequestTests`; `ProjectCoordination` integration path |

Also tenant-related scenarios from the initial draft are reframed by ADR-0012: isolation between companies is per deployment.
