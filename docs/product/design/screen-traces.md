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

## Screen — Online quote page (customer-facing, public web)

File: `online-quote.png` / `online-quote.html`

| Design decision | Decided by |
|---|---|
| Customer-facing page always showing the latest synchronised version | [UC-FIN-PUBLISH-ONLINE-QUOTE](../model/use-cases/uc-fin-publish-online-quote.md) goal |
| Kicker shows quote number + current-version marker; unsynchronised new versions stay hidden | [UC-FIN-PUBLISH-ONLINE-QUOTE](../model/use-cases/uc-fin-publish-online-quote.md), [TERM-ONLINE-QUOTE](../model/domain/terms/term-online-quote.md) |
| Welcome message to the quote contact (default from configuration) | [UC-FIN-PUBLISH-ONLINE-QUOTE](../model/use-cases/uc-fin-publish-online-quote.md) step 2 |
| Quoted line items with quantities, unit and total prices | [UC-FIN-GENERATE-QUOTE](../model/use-cases/uc-fin-generate-quote.md) step 2 (document structure) |
| Totals per revenue category, VAT, total due | [UC-FIN-REVIEW-PROJECT-FINANCIALS](../model/use-cases/uc-fin-review-project-financials.md) step 2 |
| Validity date shown ("Offer valid until 24 Feb 2026") | [BR-FIN-QUOTE-VALIDITY-DEFAULT](../model/business-rules/br-fin-quote-validity-default.md) |
| Customer actions: accept quote and sign / request a change (updated version flow) | [UC-FIN-PUBLISH-ONLINE-QUOTE](../model/use-cases/uc-fin-publish-online-quote.md) (customer as supporting actor), [JRN-FIN-QUOTE-TO-SIGNATURE](../model/journeys/jrn-fin-quote-to-signature.md) |
| Private-link note: password protection, link + password delivered by e-mail, no PDF | [BR-FIN-ONLINE-QUOTE-ACCESS](../model/business-rules/br-fin-online-quote-access.md), [BR-FIN-ONLINE-QUOTE-EMAIL](../model/business-rules/br-fin-online-quote-email.md) |

## Screen — Item detail (management, desktop)

File: `item-detail.png` / `item-detail.html`

| Design decision | Decided by |
|---|---|
| Tabs: general, prices, physical & logistics, accessories, alternatives, usage, files | [UC-EQP-CONFIGURE-ITEM-DETAILS](../model/use-cases/uc-eqp-configure-item-details.md) main flow steps 2–6 |
| Code, folder, default equipment group, planner visibility on the item | [UC-EQP-CONFIGURE-ITEM-DETAILS](../model/use-cases/uc-eqp-configure-item-details.md) step 2; code generated when left empty (flow 2a) |
| Location in warehouse, may stay empty | [UC-EQP-CONFIGURE-ITEM-DETAILS](../model/use-cases/uc-eqp-configure-item-details.md) step 3, flow 3a |
| Financial block: rental price, discount group, factor group, margin price, VAT class, ledger account | [UC-EQP-CONFIGURE-ITEM-DETAILS](../model/use-cases/uc-eqp-configure-item-details.md) step 4 |
| External and internal remarks with their audiences | [UC-EQP-CONFIGURE-ITEM-DETAILS](../model/use-cases/uc-eqp-configure-item-details.md) step 6, [BR-EQP-REMARK-AUDIENCE](../model/business-rules/br-eqp-remark-audience.md) |
| Accessories with automatic / skip-if-present / ask flags; not inherited by kits | [UC-EQP-CONFIGURE-ACCESSORIES](../model/use-cases/uc-eqp-configure-accessories.md) steps 2–3, [BR-EQP-ACCESSORY-OPTIONS](../model/business-rules/br-eqp-accessory-options.md), [BR-EQP-ACCESSORIES-NOT-INHERITED](../model/business-rules/br-eqp-accessories-not-inherited.md) |
| Alternatives offered when the item is short | [UC-EQP-SET-ALTERNATIVES](../model/use-cases/uc-eqp-set-alternatives.md) steps 2–3, [BR-WHS-ALTERNATIVE-SAVED](../model/business-rules/br-whs-alternative-saved.md) |
| Saved data applies to new planned lines; existing lines keep their snapshot | [UC-EQP-CONFIGURE-ITEM-DETAILS](../model/use-cases/uc-eqp-configure-item-details.md) postconditions |
| Hidden planner items remain bookable | [UC-EQP-CONFIGURE-ITEM-DETAILS](../model/use-cases/uc-eqp-configure-item-details.md) step 2, [BR-EQP-HIDDEN-ITEM-STILL-BOOKABLE](../model/business-rules/br-eqp-hidden-item-still-bookable.md) |

## Screen — Equipment import (management, desktop)

File: `equipment-import.png` / `equipment-import.html`

| Design decision | Decided by |
|---|---|
| Wizard steps: mode → file → mapping → folder → preview | [UC-EQP-IMPORT-EQUIPMENT](../model/use-cases/uc-eqp-import-equipment.md) main flow steps 1–7 |
| Import mode choice (update / skip) with match columns | [UC-EQP-IMPORT-EQUIPMENT](../model/use-cases/uc-eqp-import-equipment.md) steps 2, 6, [BR-EQP-IMPORT-MATCH-UNIQUE](../model/business-rules/br-eqp-import-match-unique.md) |
| Update mode chooses whether scan codes replace or add | [UC-EQP-IMPORT-EQUIPMENT](../model/use-cases/uc-eqp-import-equipment.md) alternative flow 2a |
| Proposed mapping of file columns to equipment fields and custom fields, correctable | [UC-EQP-IMPORT-EQUIPMENT](../model/use-cases/uc-eqp-import-equipment.md) step 4, [TERM-IMPORT-MATCH-COLUMN](../model/domain/terms/term-import-match-column.md) |
| Root folder choice; items placed by folder paths | [UC-EQP-IMPORT-EQUIPMENT](../model/use-cases/uc-eqp-import-equipment.md) step 5, [BR-PRJ-IMPORT-FOLDER-PLACEMENT](../model/business-rules/br-prj-import-folder-placement.md) |
| Everything tagged with an import tag; wrong import undone by archiving by tag | [UC-EQP-IMPORT-EQUIPMENT](../model/use-cases/uc-eqp-import-equipment.md) step 8, alternative flow 8b, [BR-PRJ-IMPORT-TAG](../model/business-rules/br-prj-import-tag.md) |
| Kit content and accessories not imported — added afterwards | [UC-EQP-IMPORT-EQUIPMENT](../model/use-cases/uc-eqp-import-equipment.md) postconditions, [BR-EQP-IMPORT-NO-STRUCTURE](../model/business-rules/br-eqp-import-no-structure.md) |

## Screen — Stock movements & disposition (management, desktop)

File: `stock-movements.png` / `stock-movements.html`

| Design decision | Decided by |
|---|---|
| Movements scheduled from the item's stock history: positive (delivery) / negative (reduction) quantity with date and description | [UC-EQP-SCHEDULE-STOCK-MOVEMENT](../model/use-cases/uc-eqp-schedule-stock-movement.md) steps 1–2 |
| Availability changes from the movement's date | [UC-EQP-SCHEDULE-STOCK-MOVEMENT](../model/use-cases/uc-eqp-schedule-stock-movement.md) step 3, [BR-EQP-MOVEMENT-EFFECTIVE-DATE](../model/business-rules/br-eqp-movement-effective-date.md) |
| Selling rented equipment via a separate sale copy, never by flipping the rental item | [UC-EQP-SELL-RENTAL-EQUIPMENT](../model/use-cases/uc-eqp-sell-rental-equipment.md) steps 1–3, [BR-EQP-SELL-RENTAL-VIA-SALE-COPY](../model/business-rules/br-eqp-sell-rental-via-sale-copy.md) |
| Rental stock reduced by quantity sold; note explaining the stock change | [UC-EQP-SELL-RENTAL-EQUIPMENT](../model/use-cases/uc-eqp-sell-rental-equipment.md) step 2, alternative flow 2a |
| Archive removes items from stock and planning, keeps history (struck through in planning) | [UC-EQP-ARCHIVE-ITEM](../model/use-cases/uc-eqp-archive-item.md) steps 1–3, [BR-EQP-ARCHIVE-PRESERVES-HISTORY](../model/business-rules/br-eqp-archive-preserves-history.md) |
| Restore and delete actions on archived items | [UC-EQP-ARCHIVE-ITEM](../model/use-cases/uc-eqp-archive-item.md) alternative flows 3a–3b |
| Hidden (planner-invisible) items stay bookable | [BR-EQP-HIDDEN-ITEM-STILL-BOOKABLE](../model/business-rules/br-eqp-hidden-item-still-bookable.md) |

## Screen — Stock count session (management, desktop)

File: `stock-count.png` / `stock-count.html`

| Design decision | Decided by |
|---|---|
| Count given a name and remark, tasks/notes may be added; subtitle shows who started it | [UC-EQP-COUNT-SELECTED-EQUIPMENT](../model/use-cases/uc-eqp-count-selected-equipment.md) step 1 |
| Items listed with expected warehouse stock at the moment of creation | [UC-EQP-COUNT-SELECTED-EQUIPMENT](../model/use-cases/uc-eqp-count-selected-equipment.md) step 2, [TERM-EXPECTED-WAREHOUSE-STOCK](../model/domain/terms/term-expected-warehouse-stock.md) |
| Quantities recorded by entry or by scanning codes with a camera | [UC-EQP-COUNT-SELECTED-EQUIPMENT](../model/use-cases/uc-eqp-count-selected-equipment.md) step 3, [TERM-SCAN-CODE](../model/domain/terms/term-scan-code.md) |
| Per line: correct / short / in surplus, so differences can be reviewed | [UC-EQP-COUNT-SELECTED-EQUIPMENT](../model/use-cases/uc-eqp-count-selected-equipment.md) step 4, [TERM-COUNT-RESULT](../model/domain/terms/term-count-result.md) |
| Count of selected items only (part of the warehouse in doubt) | [JRN-EQP-KEEP-STOCK-ACCURATE](../model/journeys/jrn-eqp-keep-stock-accurate.md) variants |
| Completing corrects stock with the differences; missing units recorded as lost equipment | [UC-EQP-COUNT-SELECTED-EQUIPMENT](../model/use-cases/uc-eqp-count-selected-equipment.md) steps 5–6, [TERM-LOST-EQUIPMENT](../model/domain/terms/term-lost-equipment.md) |
| Virtual kits excluded — stock derived from content | [BR-EQP-COUNT-EXCLUDES-VIRTUAL-KITS](../model/business-rules/br-eqp-count-excludes-virtual-kits.md) |

## Screen — Return handling (management/warehouse, desktop)

File: `return-handling.png` / `return-handling.html`

| Design decision | Decided by |
|---|---|
| Scan-return session: for each scanned article pick project and returned quantity; mixed truck from several projects booked back without sorting | [UC-WHS-SCAN-RETURN](../model/use-cases/uc-whs-scan-return.md) steps 1–5 |
| Optional note per return row (case clasp repaired) | [UC-WHS-SCAN-RETURN](../model/use-cases/uc-whs-scan-return.md) main flow |
| Lost line with quantity lost, charge decision and remark; recording reduces stock | [UC-WHS-MARK-LOST](../model/use-cases/uc-whs-mark-lost.md) steps 1–3 |
| Damage charged to the project rides the same charge decision | [UC-WHS-MARK-LOST](../model/use-cases/uc-whs-mark-lost.md) step 3 (charge may be added) |
| Lost equipment may invalidate reservations elsewhere — flagged in footnote | [UC-WHS-MARK-LOST](../model/use-cases/uc-whs-mark-lost.md) effects |
| Early return releases the reservation for other projects | [UC-WHS-SCAN-RETURN](../model/use-cases/uc-whs-scan-return.md) effects |

## Screen — Packing list detail (management/warehouse, desktop)

File: `packing-list.png` / `packing-list.html`

| Design decision | Decided by |
|---|---|
| Hand-over lock: list marked "Ready to pack" from the locked plan, visible on the warehouse board | [UC-WHS-MARK-READY-TO-PACK](../model/use-cases/uc-whs-mark-ready-to-pack.md) steps 1–2, [TERM-READY-TO-PACK](../model/domain/terms/term-ready-to-pack.md) |
| Scan in/out: each scan books one unit to the target status, live-scanning bar with target | [UC-WHS-BOOK-BY-SCANNING](../model/use-cases/uc-whs-book-by-scanning.md) steps 1–3 |
| Unplanned codes offer to add additional equipment | [UC-WHS-BOOK-BY-SCANNING](../model/use-cases/uc-whs-book-by-scanning.md) alternative flows, [BR-WHS-UNPLANNED-SCAN-ADDS](../model/business-rules/br-whs-unplanned-scan-adds.md) |
| Booking out takes the reservation; no early check-out | [BR-WHS-BOOK-OUT-TAKES-RESERVATION](../model/business-rules/br-whs-book-out-takes-reservation.md), [BR-WHS-NO-EARLY-CHECK-OUT](../model/business-rules/br-whs-no-early-check-out.md) |
| Per-article scan codes from equipment labels | [TERM-SCAN-CODE](../model/domain/terms/term-scan-code.md), [UC-WHS-ASSIGN-SCAN-CODES](../model/use-cases/uc-whs-assign-scan-codes.md) |
| Print packing slip per list or whole project grouped by times, with template/letterhead/text | [UC-WHS-PRINT-PACKING-SLIP](../model/use-cases/uc-whs-print-packing-slip.md) step 1 |
| Packed/Packing/To-scan chips in status tints | [BR-WHS-AUTOMATIC-PROGRESS](../model/business-rules/br-whs-automatic-progress.md), [docs/design/color-tokens.md](../../design/color-tokens.md) |

## Screen — Project equipment plan & shortages (management, desktop)

File: `equipment-plan.png` / `equipment-plan.html`

| Design decision | Decided by |
|---|---|
| Planned lines grouped into named groups with own planning/usage periods, duration option and quotation remark | [UC-WHS-ORGANIZE-GROUPS](../model/use-cases/uc-whs-organize-groups.md) step 1 |
| Editing group times applies to all its lines | [UC-WHS-ORGANIZE-GROUPS](../model/use-cases/uc-whs-organize-groups.md) step 3, [BR-WHS-LINE-TIMES-LINKED](../model/business-rules/br-whs-line-times-linked.md) |
| Search by name, catalog tags/folders, plus ranked suggestions | [UC-WHS-PLAN-EQUIPMENT](../model/use-cases/uc-whs-plan-equipment.md) step 1 |
| Availability computed per line over its planning period; shortage lines marked | [UC-WHS-PLAN-EQUIPMENT](../model/use-cases/uc-whs-plan-equipment.md) step 4, [BR-WHS-AVAILABILITY-BY-PLANNING-PERIOD](../model/business-rules/br-whs-availability-by-planning-period.md), [BR-WHS-SHORTAGE-DETECTION](../model/business-rules/br-whs-shortage-detection.md) |
| Shortage shown in error tint with negative count badge | [TERM-SHORTAGE](../model/domain/terms/term-shortage.md), [docs/design/color-tokens.md](../../design/color-tokens.md) |
| Quick availability check from anywhere (header action) | [UC-WHS-CHECK-AVAILABILITY](../model/use-cases/uc-whs-check-availability.md) |
| Alternative-article suggestion for shortages (warning tint) | [TERM-EQUIPMENT-ALTERNATIVE](../model/domain/terms/term-equipment-alternative.md), [UC-WHS-REALLOCATE-RESERVATIONS](../model/use-cases/uc-whs-reallocate-reservations.md) |
| Reserve action (timeline, lower other projects' reservations) | [UC-WHS-REALLOCATE-RESERVATIONS](../model/use-cases/uc-whs-reallocate-reservations.md) steps 1–3, [BR-WHS-RESERVATION-PRIORITY](../model/business-rules/br-whs-reservation-priority.md) |
| Over-availability lines planned anyway (footnote); combinations plan their content | [UC-WHS-PLAN-EQUIPMENT](../model/use-cases/uc-whs-plan-equipment.md) failure conditions and alternative flows |

## Screen — Task workflow settings (management, desktop)

File: `task-workflow-settings.png` / `task-workflow-settings.html`

| Design decision | Decided by |
|---|---|
| Company-wide task statuses with name + colour, reorderable | [UC-PRJ-CONFIGURE-TASK-STATUSES](../model/use-cases/uc-prj-configure-task-statuses.md) step 1, [BR-PRJ-TASK-STATUS-ORDER](../model/business-rules/br-prj-task-status-order.md) |
| To do and Completed fixed endpoints (renameable, recolourable, not deletable) | [UC-PRJ-CONFIGURE-TASK-STATUSES](../model/use-cases/uc-prj-configure-task-statuses.md) failure conditions, [BR-PRJ-TASK-STATUS-ENDPOINTS](../model/business-rules/br-prj-task-status-endpoints.md) |
| Deleting a status asks where its tasks move (migration) | [UC-PRJ-CONFIGURE-TASK-STATUSES](../model/use-cases/uc-prj-configure-task-statuses.md) step 2, [BR-PRJ-TASK-STATUS-MIGRATION](../model/business-rules/br-prj-task-status-migration.md) |
| Boards show columns in configured order (subtitle note) | [UC-PRJ-CONFIGURE-TASK-STATUSES](../model/use-cases/uc-prj-configure-task-statuses.md) postconditions |
| Teams with members; team boards span several teams; add/remove notifications | [UC-PRJ-MANAGE-TEAMS](../model/use-cases/uc-prj-manage-teams.md) steps 1–3, [BR-PRJ-TEAM-MEMBERSHIP-NOTICE](../model/business-rules/br-prj-team-membership-notice.md) |

## Screen — Service planned-vs-actual (management, desktop)

File: `service-planned-actual.png` / `service-planned-actual.html`

| Design decision | Decided by |
|---|---|
| Planned vs tracked hours compared per service, with tasks count, cost/h, price, margin | [UC-PRJ-REPORT-TASK-TIME](../model/use-cases/uc-prj-report-task-time.md) step 1, [BR-PRJ-SERVICE-BREAKDOWN](../model/business-rules/br-prj-service-breakdown.md) |
| Hours tracked on tasks flow to their linked service | [UC-PRJ-TRACK-TASK-TIME](../model/use-cases/uc-prj-track-task-time.md) step 2, [UC-PRJ-MANAGE-SERVICES](../model/use-cases/uc-prj-manage-services.md) step 3 |
| Export of tracked time (team-board export contains only that team's work) | [UC-PRJ-REPORT-TASK-TIME](../model/use-cases/uc-prj-report-task-time.md) main/alternative flows |
| Export warns when it includes unapproved hours (footnote) | [BR-PRJ-UNAPPROVED-HOURS-EXPORT-WARNING](../model/business-rules/br-prj-unapproved-hours-export-warning.md) |
| Negative margin shown in error tint, positive in success tint | [docs/design/color-tokens.md](../../design/color-tokens.md) status tints |
| Services listed in their own section of quotes/invoices (context) | [BR-PRJ-SERVICE-DOCUMENT-SECTION](../model/business-rules/br-prj-service-document-section.md) |

## Screen — Project task board (management, desktop)

File: `project-task-board.png` / `project-task-board.html`

| Design decision | Decided by |
|---|---|
| Board (status columns) or list view shared with team and external collaborators | [UC-PRJ-WORK-WITH-TASK-BOARD](../model/use-cases/uc-prj-work-with-task-board.md) main flow, [JRN-PRJ-DELIVER-PRODUCTION-WORK](../model/journeys/jrn-prj-deliver-production-work.md) narrative |
| Columns are statuses; task statuses configured per workspace | [UC-PRJ-PROGRESS-TASK](../model/use-cases/uc-prj-progress-task.md) step 1, [UC-PRJ-CONFIGURE-TASK-STATUSES](../model/use-cases/uc-prj-configure-task-statuses.md) |
| Filters, grouping/ordering, extra columns (planned/tracked time) | [UC-PRJ-WORK-WITH-TASK-BOARD](../model/use-cases/uc-prj-work-with-task-board.md) step 2 |
| Saved views, personal vs shared-for-everyone grouping | [UC-PRJ-WORK-WITH-TASK-BOARD](../model/use-cases/uc-prj-work-with-task-board.md) step 3, [BR-PRJ-BOARD-GROUPING-SCOPE](../model/business-rules/br-prj-board-grouping-scope.md), [TERM-TASK-BOARD](../model/domain/terms/term-task-board.md) |
| Cards show assignee, planned/tracked time, deadline; overdue chip in error tint | [UC-PRJ-ADD-TASK](../model/use-cases/uc-prj-add-task.md), [BR-PRJ-TASK-OVERDUE](../model/business-rules/br-prj-task-overdue.md) |
| Status tints readable in grayscale (error overdue, neutral done) | [docs/design/color-tokens.md](../../design/color-tokens.md) status tints |
| Footnote: moving a card edits the task; one running timer; hours flow to the linked service and need approval | [BR-PRJ-BOARD-MOVE-EDITS-TASK](../model/business-rules/br-prj-board-move-edits-task.md), [BR-PRJ-SINGLE-RUNNING-TIMER](../model/business-rules/br-prj-single-running-timer.md), [UC-PRJ-TRACK-TASK-TIME](../model/use-cases/uc-prj-track-task-time.md), [BR-PRJ-TASK-HOURS-APPROVAL](../model/business-rules/br-prj-task-hours-approval.md) |

## Screen — Project detail & schedule (management, desktop)

File: `project-detail.png` / `project-detail.html`

| Design decision | Decided by |
|---|---|
| Tabs: overview, equipment plan, planning, crew, finances, files | [JRN-PRJ-RUN-RENTAL-PROJECT](../model/journeys/jrn-prj-run-rental-project.md) |
| Project number + status chip (Confirmed) + dates and location in header | [UC-PRJ-EDIT-PROJECT-DETAILS](../model/use-cases/uc-prj-edit-project-details.md) step 1, [TERM-PROJECT-STATUS](../model/domain/terms/term-project-status.md), [BR-PRJ-PROJECT-NUMBER-ASSIGNMENT](../model/business-rules/br-prj-project-number-assignment.md) |
| Client + contact person chosen from contact database fill address and automatic note | [UC-PRJ-EDIT-PROJECT-DETAILS](../model/use-cases/uc-prj-edit-project-details.md) step 2, [BR-PRJ-PARTIES-FROM-CONTACTS](../model/business-rules/br-prj-parties-from-contacts.md), [BR-PRJ-CONTACT-SELECTION-EFFECTS](../model/business-rules/br-prj-contact-selection-effects.md) |
| Location shown with distance/travel time from warehouse | [UC-PRJ-EDIT-PROJECT-DETAILS](../model/use-cases/uc-prj-edit-project-details.md) step 3 |
| Named periods with start/end, default planning + usage period designation, per-period document display toggle | [UC-PRJ-MANAGE-TIME-SCHEDULE](../model/use-cases/uc-prj-manage-time-schedule.md) steps 2–4, [BR-PRJ-DEFAULT-PERIOD-DESIGNATION](../model/business-rules/br-prj-default-period-designation.md), [BR-PRJ-PERIOD-DOCUMENT-DISPLAY](../model/business-rules/br-prj-period-document-display.md) |
| "Select periods → generate crew and transport functions" note | [UC-PRJ-MANAGE-TIME-SCHEDULE](../model/use-cases/uc-prj-manage-time-schedule.md) alternative flows, [BR-PRJ-FUNCTIONS-FROM-PERIODS](../model/business-rules/br-prj-functions-from-periods.md) |
| Automatic client warning note from the client record | [UC-PRJ-EDIT-PROJECT-DETAILS](../model/use-cases/uc-prj-edit-project-details.md) step 2 |
| Account manager, colour, external reference, project-type defaults as details | [UC-PRJ-EDIT-PROJECT-DETAILS](../model/use-cases/uc-prj-edit-project-details.md) step 1, [BR-PRJ-PROJECT-TYPE-DEFAULTS](../model/business-rules/br-prj-project-type-defaults.md) |

## Screen — Project creation (management, desktop)

File: `project-creation.png` / `project-creation.html`

| Design decision | Decided by |
|---|---|
| Step flow: type & template → start date → preview of copied content → save | [UC-PRJ-CREATE-FROM-TEMPLATE](../model/use-cases/uc-prj-create-from-template.md) main flow |
| Project type and template selection, with blank-project alternative | [UC-PRJ-CREATE-FROM-TEMPLATE](../model/use-cases/uc-prj-create-from-template.md) step 1, alternative flows ([UC-PRJ-CREATE-PROJECT](../model/use-cases/uc-prj-create-project.md)) |
| Start date prefilled when started from a calendar day | [UC-PRJ-CREATE-FROM-TEMPLATE](../model/use-cases/uc-prj-create-from-template.md) step 2, [BR-PRJ-CALENDAR-QUICK-CREATE](../model/business-rules/br-prj-calendar-quick-create.md) |
| Schedule copied from template and shifted to the chosen start date, period times adjustable | [UC-PRJ-CREATE-FROM-TEMPLATE](../model/use-cases/uc-prj-create-from-template.md) step 3, [BR-PRJ-TEMPLATE-START-ANCHOR](../model/business-rules/br-prj-template-start-anchor.md) |
| Preview shows copied schedule, equipment and function groups | [UC-PRJ-CREATE-FROM-TEMPLATE](../model/use-cases/uc-prj-create-from-template.md) step 4, [BR-PRJ-TEMPLATE-CONTENT](../model/business-rules/br-prj-template-content.md) |
| Archived template item note ("copied but marked as no longer existing") | [UC-PRJ-CREATE-FROM-TEMPLATE](../model/use-cases/uc-prj-create-from-template.md) alternative flows, [BR-PRJ-ARCHIVED-TEMPLATE-ITEM](../model/business-rules/br-prj-archived-template-item.md) |
| Save assigns a project number; crew/vehicle assignments start empty | [UC-PRJ-CREATE-FROM-TEMPLATE](../model/use-cases/uc-prj-create-from-template.md) step 5, [BR-PRJ-PROJECT-NUMBER-ASSIGNMENT](../model/business-rules/br-prj-project-number-assignment.md) |
| Saving without a time schedule refused (error card) | [UC-PRJ-CREATE-FROM-TEMPLATE](../model/use-cases/uc-prj-create-from-template.md) failure conditions, [BR-PRJ-SCHEDULE-REQUIRED](../model/business-rules/br-prj-schedule-required.md) |
| Selected template card and selected type use the blue selected token | [docs/design/color-tokens.md](../../design/color-tokens.md) `--raccord-selected` |

## Screen — Issuer profile settings (management, desktop)

File: `issuer-profile.png` / `issuer-profile.html`

| Design decision | Decided by |
|---|---|
| Issuer legal details entered once: legal name, tax identifier, complete postal address | [UC-DEFINE-ISSUER-PROFILE](../model/use-cases/uc-define-issuer-profile.md) step 1, [JRN-SET-UP-INVOICING](../model/journeys/jrn-set-up-invoicing.md) |
| Tax identifier normalized on save | [UC-DEFINE-ISSUER-PROFILE](../model/use-cases/uc-define-issuer-profile.md) step 2, [BR-TAX-ID-NORMALIZATION](../model/business-rules/br-tax-id-normalization.md) |
| Issued invoices keep their legal-details snapshot; profile edits don't rewrite history | [UC-DEFINE-ISSUER-PROFILE](../model/use-cases/uc-define-issuer-profile.md) alternative flows, [BR-PARTY-SNAPSHOT](../model/business-rules/br-party-snapshot.md) |
| Change audited | [UC-DEFINE-ISSUER-PROFILE](../model/use-cases/uc-define-issuer-profile.md) step 3, [BR-AUDITED-ACTIONS](../model/business-rules/br-audited-actions.md) |
| Required-before-first-invoice warning card (missing name/tax id/address refused with reason) | [UC-DEFINE-ISSUER-PROFILE](../model/use-cases/uc-define-issuer-profile.md) failure conditions, [JRN-SET-UP-INVOICING](../model/journeys/jrn-set-up-invoicing.md) flagging note |

## Screen — Financial settings hub (management, desktop)

File: `financial-settings.png` / `financial-settings.html`

| Design decision | Decided by |
|---|---|
| Hub groups all finance preparation: document settings, price factors, ledgers, VAT, invoice moments, payment terms (tab bar) | [JRN-FIN-PREPARE-FINANCE](../model/journeys/jrn-fin-prepare-finance.md) steps |
| Default quote/contract validity in days and sign-after-expiry switch | [UC-FIN-CONFIGURE-FINANCIAL-SETTINGS](../model/use-cases/uc-fin-configure-financial-settings.md) step 1, [BR-FIN-SIGN-AFTER-EXPIRY](../model/business-rules/br-fin-sign-after-expiry.md) |
| Terms PDF upload with per-document-type "sent with" switches (quotes/contracts/invoices) | [UC-FIN-CONFIGURE-FINANCIAL-SETTINGS](../model/use-cases/uc-fin-configure-financial-settings.md) step 2, [BR-FIN-TERMS-ATTACHMENT](../model/business-rules/br-fin-terms-attachment.md) |
| Reusable additional conditions with defaults per project type | [UC-FIN-CONFIGURE-FINANCIAL-SETTINGS](../model/use-cases/uc-fin-configure-financial-settings.md) step 3 |
| Number series for quotes, contracts, invoices and credit notes, shown as prefix + next number | [UC-FIN-CONFIGURE-FINANCIAL-SETTINGS](../model/use-cases/uc-fin-configure-financial-settings.md) step 4, [TERM-INVOICE-SERIES](../model/domain/terms/term-invoice-series.md) |
| Invoice moments with up to three parts, percentage + trigger, defaults per project type | [UC-FIN-CONFIGURE-INVOICE-MOMENTS](../model/use-cases/uc-fin-configure-invoice-moments.md) step 1–2, [BR-FIN-MOMENT-PERCENTAGES](../model/business-rules/br-fin-moment-percentages.md) |
| Payment terms (days + method) with default assigned to customers | [UC-FIN-CONFIGURE-PAYMENT-TERMS](../model/use-cases/uc-fin-configure-payment-terms.md), [BR-FIN-DUE-DATE-FROM-TERM](../model/business-rules/br-fin-due-date-from-term.md) |

## Screens 5–7 — Invoicing queue · Payments & dunning · Quote review (management, desktop)

Files: `invoicing-queue.png/html` · `payments-dunning.png/html` · `quote-review.png/html`

| Design decision | Decided by |
|---|---|
| Queue lists confirmed projects whose invoice-moment parts fall in a selected period | [UC-FIN-WORK-INVOICING-QUEUE](../model/use-cases/uc-fin-work-invoicing-queue.md) main flow |
| Overdue parts highlighted; non-confirmed and already-invoiced projects excluded via eligibility rule | [UC-FIN-WORK-INVOICING-QUEUE](../model/use-cases/uc-fin-work-invoicing-queue.md), [BR-FIN-QUEUE-ELIGIBILITY](../model/business-rules/br-fin-queue-eligibility.md) |
| "Invoice externally" as separate tab (externally invoiced items) | [UC-FIN-WORK-INVOICING-QUEUE](../model/use-cases/uc-fin-work-invoicing-queue.md) alternative flow, [UC-FIN-MARK-INVOICED-EXTERNALLY](../model/use-cases/uc-fin-mark-invoiced-externally.md) |
| Invoice action per entry starts a draft with proposed terms | [UC-FIN-GENERATE-PROJECT-INVOICE](../model/use-cases/uc-fin-generate-project-invoice.md) step 1 |
| Payments recorded with date, method, amount, description; outstanding recalculated | [UC-FIN-RECORD-PAYMENT](../model/use-cases/uc-fin-record-payment.md) main flow |
| Payment that would overpay is refused (outstanding never below zero) | [BR-FIN-OUTSTANDING-AMOUNT](../model/business-rules/br-fin-outstanding-amount.md) |
| Payment status badges Unpaid / Partially paid / Paid with amounts | [TERM-PAYMENT-STATUS](../model/domain/terms/term-payment-status.md) |
| Overdue invoices get a "Remind" action; reminder count + last date shown per invoice | [UC-FIN-SEND-PAYMENT-REMINDER](../model/use-cases/uc-fin-send-payment-reminder.md), [TERM-PAYMENT-REMINDER](../model/domain/terms/term-payment-reminder.md) |
| Quote screen shows number series, expiration from default validity, template + letterhead from project type | [UC-FIN-GENERATE-QUOTE](../model/use-cases/uc-fin-generate-quote.md) step 2 |
| Financial overview per revenue category: estimated / planned / actual cost, revenue, discount, profit | [UC-FIN-REVIEW-PROJECT-FINANCIALS](../model/use-cases/uc-fin-review-project-financials.md) step 2 |
| Profit-amount ↔ margin-percent toggle | [UC-FIN-REVIEW-PROJECT-FINANCIALS](../model/use-cases/uc-fin-review-project-financials.md) step 3 |
| Project documents list shows quotes, contracts and invoices together, with status | [UC-FIN-REVIEW-PROJECT-FINANCIALS](../model/use-cases/uc-fin-review-project-financials.md) step 4 |
| "Lock price" action on the quote | [UC-FIN-LOCK-PROJECT-PRICE](../model/use-cases/uc-fin-lock-project-price.md) |
| "Publish online" opens the customer-facing quote page | [UC-FIN-PUBLISH-ONLINE-QUOTE](../model/use-cases/uc-fin-publish-online-quote.md) |
