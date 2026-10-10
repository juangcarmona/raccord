# Screen traces — pen.dev trial

Every screen in `raccord-screens.pen` is generated from Product Definition as
Code artifacts, never from imagination. This file records which artifact
decided which design decision, so a reviewer can audit spec fidelity without
reading the chat that produced the screens.

Model root: `docs/product/model/`. Translation keys (QR-MOBILE-WEB-APP) are in
`docs/product/model/requirements/quality/`. Screen mode split comes from the
mobile-delivery decision recorded in
`docs/product/changes/completed/chg-initial/change.md` (decision table:
"Mobile delivery — one responsive PWA, with a field mode for workday
check-in/out and read-only assignments, a management mode for operational
control, and a read-only large-screen board").

## Screen 1 — Invoice list (management, desktop)

File: `invoice-list.png` / `invoice-list.html`

| Design decision | Decided by |
|---|---|
| Billable lines grouped under a project, invoice draft from selection, validation then finalization | [UC-CREATE-INVOICE-DRAFT](../model/use-cases/uc-create-invoice-draft.md), [UC-BILL-FINISHED-PROJECT](../model/journeys/jrn-bill-finished-project.md) |
| Status shown as Draft preview / Issued, no back-transition from issued | [TERM-INVOICE-STATUS](../model/domain/terms/term-invoice-status.md) |
| Payment Unpaid / Partially paid / Paid + Overdue qualifier, shown in lists with totals paid for a selection | [TERM-PAYMENT-STATUS](../model/domain/terms/term-payment-status.md) |
| Series prefixes F (ordinary) / R (rectifying) included in invoice numbers | [TERM-INVOICE-SERIES](../model/domain/terms/term-invoice-series.md) |
| Selection bar with totals paid and bulk actions | [TERM-PAYMENT-STATUS](../model/domain/terms/term-payment-status.md) usage note |

## Screen 2 — Field check-in (field mode, phone)

File: `field-check-in.png` / `field-check-in.html`

| Design decision | Decided by |
|---|---|
| Clock in / clock out with server time; double clock-in refused | [UC-CLOCK-WORKDAY](../model/use-cases/uc-clock-workday.md) |
| Workday record states Not started → Working ↔ On break → Finished, shown as a state chip | [TERM-WORKDAY-RECORD](../model/domain/terms/term-workday-record.md) |
| Offline: clocking refused, retry when connected (note on screen) | [UC-CLOCK-WORKDAY](../model/use-cases/uc-clock-workday.md) failure conditions |
| Field worker sees own activity and own record only — no project edits in first field slice | [QR-MOBILE-WEB-APP](../model/requirements/quality/qr-mobile-web-app.md) |
| Today's entries list with corrections marked, not edited | [TERM-WORKDAY-RECORD](../model/domain/terms/term-workday-record.md) (append-only corrections) |
| "My jobs" and "Hours" as field nav destinations | [UC-CRW-CONSULT-OWN-SCHEDULE](../model/use-cases/uc-crw-consult-own-schedule.md), mobile-delivery decision |

## Screen 3 — Operations board (board mode, large screen)

File: `ops-board.png` / `ops-board.html`

| Design decision | Decided by |
|---|---|
| Read-only large-screen operational board for office display | [QR-MOBILE-WEB-APP](../model/requirements/quality/qr-mobile-web-app.md), mobile-delivery decision |
| Columns to pack / prepped / on location / expected back / delayed | [UC-WHS-REVIEW-WAREHOUSE-BOARD](../model/use-cases/uc-whs-review-warehouse-board.md) main flow |
| Cards show name, number, date, shortage indicator, notes | [UC-WHS-REVIEW-WAREHOUSE-BOARD](../model/use-cases/uc-whs-review-warehouse-board.md) step 3 |
| Delayed is derived automatically, never manually settable | [TERM-DELAYED-EQUIPMENT](../model/domain/terms/term-delayed-equipment.md), [BR-WHS-DELAYED-AUTOMATIC](../model/business-rules/br-whs-delayed-automatic.md) |
| Board date defaults to today (due-date semantics: start of planning period for outgoing, end for returns) | [TERM-WAREHOUSE-DUE-DATE](../model/domain/terms/term-warehouse-due-date.md) |
| Progress dots red / yellow / green per card and project | [TERM-WAREHOUSE-PROGRESS](../model/domain/terms/term-warehouse-progress.md) |
| Read-only: no actions, no edit affordances anywhere | Mobile-delivery decision (read-only board mode) |

## Screen 4 — Invoice draft editor (management, desktop)

File: `invoice-draft-editor.png` / `invoice-draft-editor.html`

| Design decision | Decided by |
|---|---|
| Two-panel layout: unbilled lines selectable on the left, draft contents on the right | [UC-CREATE-INVOICE-DRAFT](../model/use-cases/uc-create-invoice-draft.md) main flow (select unbilled lines → gathered into one draft) |
| Draft lines in project order, in the default series, with calculated VAT breakdown and totals | [UC-CREATE-INVOICE-DRAFT](../model/use-cases/uc-create-invoice-draft.md), [TERM-INVOICE-SERIES](../model/domain/terms/term-invoice-series.md) |
| Status shown as "Draft preview · open"; issuing refused until readiness is met | [TERM-INVOICE-STATUS](../model/domain/terms/term-invoice-status.md), [UC-ISSUE-INVOICE](../model/use-cases/uc-issue-invoice.md) failure conditions (all unmet conditions reported, nothing issued, no number consumed) |
| Per-line remove button returning the line to unbilled and recalculating the draft | [UC-REVISE-INVOICE-DRAFT](../model/use-cases/uc-revise-invoice-draft.md) main flow |
| Discard draft action (all lines become unbilled) | [UC-REVISE-INVOICE-DRAFT](../model/use-cases/uc-revise-invoice-draft.md) alternative flow |
| Validation issues block shown before issue (e.g. missing customer tax details) | [TERM-VALIDATION-ISSUE](../model/domain/terms/term-validation-issue.md), [UC-ISSUE-INVOICE](../model/use-cases/uc-issue-invoice.md) step 2 |
| VAT shown per rate (21%/10%/4% rows) | [BR-VAT-PER-RATE](../model/business-rules/br-vat-per-rate.md) |
| Selection and validation states use the selected-blue token, not cyan | [docs/design/color-tokens.md](../../design/color-tokens.md) `--raccord-selected` (decided 2026-10-10) |
