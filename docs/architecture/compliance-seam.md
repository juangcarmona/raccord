# Compliance seam: VERI*FACTU and e-invoicing

Status: seam implemented, submission **not** implemented. Last updated: 2026-10-20. Requirements source: [Spanish invoicing compliance requirements](../research/spanish-invoicing-compliance-requirements.md). The product boundary for self-hosted installations is recorded in [ADR-0029](../adr/0029-self-hosted-verifactu-responsibility-boundary.md); statutory producer identity and liability remain subject to independent legal review. Nothing here is a claim of legal compliance; it is the engineering contract that a reviewed implementation must satisfy.

<!-- pdac:cite id="CON-SIF-REGULATION" digest="sha256:a3922433f348bd87649eede3e8a57ca51fba868ad49927883feac172775f8dc3" -->
<!-- pdac:cite id="CON-NO-COMPLIANCE-CLAIM" digest="sha256:a5035db4daaf71d39067c492306313089d3916634c536c81b4b91e41c7452492" -->

## 1. Where the seam is

```text
InvoicingWorkflow.FinalizeAsync  (one unit of work)
  ├─ InvoiceDraft.Finalize ─► IssuedInvoice + IssuedInvoiceRepresentation (fingerprint)
  ├─ IIssuedInvoiceRepository.Add              (append-only)
  ├─ IComplianceRecorder.RegisterIssuedInvoice ◄── THE SEAM
  ├─ IAuditTrail.Append(invoice.issued)
  ├─ IInvoiceDocumentRenderer + IDocumentStore (PDF)
  └─ IUnitOfWork.SaveChanges                   (all or nothing)
```

Today `PendingComplianceRecorder` writes one `compliance_records` row per invoice:

| Column | Meaning |
| --- | --- |
| `id` | Record id (UUIDv7, time-ordered) |
| `invoice_id`, `invoice_number`, `content_hash` | Link to the immutable issued representation |
| `kind` | `invoice-issued` (future: `invoice-cancelled`, `correction`) |
| `status` | `pending` |
| `attempts`, `last_attempt_at`, `last_error` | Reserved for the submitter |
| `created_at` | Server clock (UTC) |

Rows can change status but can never be deleted (database trigger).

## 2. Inputs the adapter receives

From the issued representation (never from mutable tables):

- issuer legal name and tax ID; recipient legal name, tax ID (or other identifier) and country;
- series, number, issue date (local date in the issuer's time zone), operation date;
- invoice type (today always a full invoice, F1); for future corrections: R1–R5, method (substitution/differences) and the referenced invoices;
- VAT breakdown per rate: regime key, operation qualification or exemption cause, rate, base, quota (and equivalence surcharge when supported);
- totals; description; Raccord's correlation id (invoice id).

Gaps to close before implementation: regime key and qualification per line/rate (today implicit: general regime, subject and not exempt), exemption causes, recipients without Spanish tax ID, and simplified invoices.

## 3. Outputs and evidence

The adapter must produce and store, append-only:

1. **Billing record** (alta or anulación): canonical XML, generation timestamp with offset, reference to the previous record, `Huella` (SHA-256 over the AEAT-specified field string), and — in NO VERI*FACTU mode — the XAdES signature.
2. **QR URL** for the document, plus the legend in VERI*FACTU mode. The record must exist **before** the document is released, so record generation will move ahead of PDF rendering inside finalization, and the PDF renderer gains a QR element.
3. **Submission evidence** per attempt: request XML, raw response, global and per-record status, error codes and texts, the secure verification code (stored on receipt; it cannot be retrieved later), presentation timestamp, the wait time returned by AEAT, certificate/representation used, endpoint.

## 4. Chain rules

- One chain per (issuer tax ID, installation). Appending is a single-writer critical section enforced by the database (unique "previous record" link or a per-chain sequence row with `xmin`, like invoice numbering).
- The first record has an empty previous hash. Timestamps never go backwards within a chain and come only from the server clock.
- The hash function is pure and covered by golden tests using AEAT's published examples; a chain verifier can recheck any range.

## 5. Submission behaviour

- Outbox per issuer: send in generation order, at most 1,000 records per message, respecting the wait time AEAT returns (60 s initially) unless 1,000 records are waiting.
- **Retries resend the same record** (same XML, hash and timestamp). A new record is created only for an AEAT-defined correction path.
- Status per record: `pending` → `accepted` | `accepted-with-errors` | `rejected` | `incident` (AEAT unreachable); derived `superseded` (corrected) and `cancelled` (anulación), each linked to the record that caused it.
- When AEAT is unreachable, records are flagged as delayed and retried at least hourly; the operator sees the incident in the UI.
- Rejections never change the invoice. They surface as tasks: submit a corrected record, or issue a rectifying invoice, depending on the error class.

## 6. Immutability rules

- Issued invoices, billing records and evidence are never updated or deleted; corrections and cancellations are new records.
- Raccord must not offer any feature that hides, deletes or alters the record trail (including manual SQL paths documented as supported). Database triggers and fingerprints are the first line of defense; access control is the second.
- Test and training invoices go to a clearly labelled series and are cancelled with anulación records, never deleted.

## 7. B2B e-invoicing

A separate adapter behind the same issued representation: UBL 2.1 / EN 16931 for the public solution, status messages (accepted, rejected, paid with date) inbound and outbound, delivery through a private platform or the public solution. It must not be conflated with the VERI*FACTU record.

## 8. Not decided yet (blocks implementation)

See open questions 9–16: producer responsibility and declaration, VERI*FACTU vs NO VERI*FACTU, certificate custody in self-hosting, enacted deadlines, R-code mapping.
