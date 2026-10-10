---
id: UC-FIN-WORK-INVOICING-QUEUE
type: use-case
title: "Work through the invoicing queue"
status: draft
primary-actor: "ACT-BILLING-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-INVOICING"
governed-by:
  - "BR-FIN-QUEUE-ELIGIBILITY"
  - "BR-FIN-MOMENT-TRIGGER-DATES"
uses-terms:
  - "TERM-INVOICING-QUEUE"
  - "TERM-QUEUE-ENTRY-STATUS"
provenance:
  source: ".private/research/help-center/articles/360013965120.md, .private/research/help-center/articles/360013628279.md, .private/research/help-center/articles/360017700359.md, .private/research/help-center/articles/25948227210258.md"
  confidence: high
  recovered-from: documentation
---

## Goal

No confirmed project misses an invoice moment.

## Trigger

Regular billing routine.

## Preconditions

Projects have invoice moments.

## Main Flow

1. The administrator selects a period.
2. The product lists confirmed projects whose invoice moment parts fall in it, with the share to invoice, highlighting overdue parts.
3. The administrator starts an invoice from an entry.

## Alternative Flows

The administrator switches to the list of externally invoiced items. Projects can also be checked individually through their invoicing progress indicator.

## Failure Conditions

- A period with no qualifying entries shows an empty queue.
- Projects that are not confirmed, or whose parts are already invoiced or marked invoiced externally, are not listed (BR-FIN-QUEUE-ELIGIBILITY).
- A project with no invoice moment or no planning dates cannot be placed in the queue (BR-FIN-MOMENT-TRIGGER-DATES).
- A user without invoicing permission is refused and sees nothing.
- If another user already invoiced the entry, starting an invoice from it is refused and the administrator must reload.

## Postconditions

Invoiced parts leave the queue.
