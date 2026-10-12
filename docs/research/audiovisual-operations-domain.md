# Audiovisual operations domain: findings for Raccord

Research date: 2026-10-06. This note summarizes, in Raccord's own words, what the operations side of audiovisual rental and production involves. It was built from public documentation of the market's existing tools and from industry practice. Source-specific provenance is kept in private maintainer notes and is deliberately not redistributed; no third-party wording, screens or structure are reproduced here.

## 1. The core lifecycle

```text
enquiry ─► quoted ─► confirmed ─► in progress ─► completed
                         │              │
                         ▼              ▼
              warehouse: pack ─► out ─► (partial) return
                         │
                         ▼
              billing: due ─► draft ─► issued ─► sent ─► partially paid ─► paid
                                         └─► corrected by a new document
cancelled: reachable from any commercial state, with clean-up of holds and assignments
```

Findings:

- **Commercial status and warehouse status are different things.** A project may be "in progress" while half the equipment is already back. Warehouse progress belongs to packing lists or individual items, not to the project.
- **Unconfirmed work still affects availability.** Soft holds for enquiries and quotes, firm reservations for confirmed jobs; cancelled jobs release everything.
- **Overbooking is normal and must be visible, not forbidden.** Planning past stock produces a shortage to resolve (alternative item, transfer, sub-rental).

## 2. Two periods per job

Every job has at least two time spans:

- a **planning period** (from leaving the warehouse to returning, including prep and cleaning) that drives availability;
- a **usage period** (the days the customer uses the gear) that drives price.

Duration pricing commonly uses day-range factors (for example five rental days billed as about three). Increasingly, individual lines need their own periods (staggered departures and returns within one job).

## 3. Equipment

- Items are counted (bulk) or individually tracked (serialized). Changing tracking after use is unsafe; the safe path is to retire and replace the item.
- **Kits** come in two flavours: physical containers with expected contents (checked complete/incomplete on packing) and flexible planning bundles.
- Repairs, losses and inspections affect availability. A damaged item reported on site is not yet back in the warehouse.
- Long holds with only some billable days are common (for example, equipment installed for a week, billed per show day). Availability and billing must therefore be decoupled.

## 4. Crew and vehicles

- Jobs define **roles** (what is needed, how many, when) before **people** are assigned.
- Cost rates and price rates differ; overtime is expressed as time-band factors; expenses (meals, travel, lodging) have both cost and price.
- Freelancers are invited, confirm availability and log hours; logged hours feed actual cost, not payroll.
- Vehicles are priced per distance plus a fixed component.

## 5. Billing patterns

- **Milestone billing** is common: e.g. 30 % on confirmation and 70 % after the job, with later invoices deducting earlier ones.
- Invoices are often summaries per revenue category (rental, sale, crew, transport, other) rather than copies of every planned line. Raccord keeps both: the issued line snapshot for presentation and the VAT breakdown for accounting.
- **Discounts stack** (customer default, item family, line, category, whole project). The order must be defined explicitly; customer defaults are copied when chosen and do not change running jobs.
- **Rounding differences of one or two cents** are a recurring pain when tools round only for display. Raccord defines one rounding policy and tests it (ADR-0006).
- **Immutability after issuing** is expected in Spain; corrections are new documents. Duplicating an invoice must never silently change amounts.
- Payment tracking (partial payments, outstanding balance, manual reminders) belongs to invoicing; deposits and insurance need explicit treatment.

## 6. Documents

Quotes and contracts are versioned snapshots of the plan; customer acceptance confirms the job. Packing lists, call sheets, repair orders and invoices are generated documents. Configurable templates are expected eventually; a deterministic default is enough to start.

## 7. Integration and reporting lessons

- Reports computed only from live data cannot answer "what did we invoice last quarter" reliably; financial figures must be snapshotted at issue.
- Outbound change notifications without event identifiers, ordering guarantees or replay force consumers to poll and reconcile. If Raccord offers webhooks, they will be signed, carry an event id and entity version, be retried with backoff, and be replayable.
- Long-lived, user-scoped API tokens are a weak default; Raccord will use scoped, revocable credentials.

## 8. Implications already applied in the first slice

| Finding | Raccord decision |
| --- | --- |
| Rounding gaps | Single policy: line rounded once, VAT per rate on the summed base (ADR-0006). |
| Double billing risk | Billing state per billable line with optimistic concurrency. |
| Issued documents changing | Immutable representation, fingerprint, append-only tables (ADR-0004, ADR-0005). |
| Line order instability | Explicit line positions. |
| Live-only reporting | Issued invoices store their totals and indexed columns at issue time. |
