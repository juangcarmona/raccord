---
id: JRN-FIN-STAGED-BILLING
type: journey
title: "Bill a project in stages and collect payment"
status: draft
primary-actor: "ACT-BILLING-ADMINISTRATOR"
steps:
  - use-case: "UC-FIN-WORK-INVOICING-QUEUE"
  - use-case: "UC-FIN-GENERATE-PROJECT-INVOICE"
  - use-case: "UC-ISSUE-INVOICE"
  - use-case: "UC-FIN-EMAIL-INVOICE"
  - use-case: "UC-FIN-RECORD-PAYMENT"
  - use-case: "UC-FIN-SEND-PAYMENT-REMINDER"
  - use-case: "UC-FIN-RUN-ACCOUNTING-EXPORT"
provenance:
  source: ".private/research/help-center/articles/360013628279.md, .private/research/help-center/articles/360013965120.md, .private/research/help-center/articles/18560784878610.md, .private/research/help-center/articles/17195865170706.md, .private/research/help-center/articles/360017700379.md"
  confidence: medium
  recovered-from: documentation
---

## Intended Outcome

Every invoice moment of a confirmed project is invoiced, paid and booked.

## Entry Conditions

The project is confirmed and has an invoice moment.

## Journey Narrative

The down-payment part appears in the queue; the administrator generates a partial invoice, finalizes and sends it, and records payment. After the event the final part appears; the final invoice deducts the down payment. Overdue invoices are chased with reminders; finalized invoices are exported to the accountant as a file.

## Variants and Branches

Invoicing done externally; corrections by credit note.

## Completion Conditions

All parts invoiced, outstanding amounts zero and invoices exported.
