---
title: Runtime view
arc42-section: "06"
description: Shows the runtime interactions for the system's important scenarios.
---
<!-- markdownlint-disable MD025 -->

# 6. Runtime view

## R-1 Create a draft

1. `POST /api/invoice-drafts {projectId, billableLineIds, seriesCode?}`.
2. `InvoicingWorkflow` loads the project and resolves the series (default when omitted).
3. `InvoiceDraft.CreateFromProject` asks the project to reserve the lines (unbilled → in draft), ordered by line position, and copies them.
4. Audit `invoice-draft.created`; one `SaveChanges`. A concurrent reservation of the same line fails on `xmin` → 409, or on the domain rule → 422.
5. The response includes server totals and validation issues for today's date.

## R-2 Finalize (issue) an invoice

1. `POST /api/invoice-drafts/{id}/finalize {issueDate?}`; default date is today in the organization time zone.
2. Load draft (must be open), project, customer, issuer profile and the sequence for (series, year) — created if missing.
3. `draft.Validate(...)` → any issue → 422 `draft.invalid` with all issues; nothing is written, no number consumed.
4. `draft.Finalize(...)` allocates the next number, snapshots parties and lines, and returns `IssuedInvoice`; the project marks the lines invoiced.
5. Build `IssuedInvoiceRepresentation` (JSON + SHA-256); store it append-only; register the compliance record; audit `invoice.issued`.
6. Render the PDF, store it in object storage, audit `invoice.document.stored` with the PDF hash.
7. Commit everything in one transaction. If the commit fails, the stored PDF is an orphan under a deterministic key and no invoice exists.

```text
Web ─► InvoicingWorkflow ─► Domain (validate, allocate, snapshot)
                         ├─► IssuedInvoiceRepository.Add
                         ├─► ComplianceRecorder.Register
                         ├─► AuditTrail.Append ×2
                         ├─► Renderer.Render ─► DocumentStore.Save
                         └─► UnitOfWork.SaveChanges (single DB transaction)
```

## R-3 Read an invoice / download its document

1. Load the stored JSON; recompute the hash; mismatch → 500 `issued-invoice.integrity-violation` + critical log.
2. For the document: read from storage; if missing, render again from the representation (identical bytes), store it and audit `invoice.document.regenerated`.

## R-4 Start-up and migration

- Compose: `postgres` healthy → `migrate` (`dotnet Raccord.Web.dll migrate`) applies EF migrations and reference series → exits 0 → `web` starts → readiness checks the database.
- Aspire: the same sequence with `migrate --seed-demo`, plus Azurite and the Vite dev server.
- Azure: run the Container Apps Job, then roll the Container App.

## R-5 Future: VERI*FACTU submission

A background worker claims pending compliance records per issuer in order, builds/sends batches, stores evidence, and handles outages and rejections (see [compliance seam](../compliance-seam.md)).
