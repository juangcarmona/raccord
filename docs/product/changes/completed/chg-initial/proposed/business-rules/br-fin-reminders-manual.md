---
id: BR-FIN-REMINDERS-MANUAL
type: business-rule
title: "Reminders are manual and only after the due date"
status: draft
applies-to:
  - "UC-FIN-SEND-PAYMENT-REMINDER"
uses-terms:
  - "TERM-PAYMENT-REMINDER"
  - "TERM-OVERDUE-INVOICE"
provenance:
  source: ".private/research/help-center/articles/17195865170706.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A payment reminder can be generated only for an invoice past its due date, is never sent automatically, may set a new due date, and increments the invoice's reminder count and last-reminder date; further reminders may follow while it remains unpaid.

## Rationale

Chasing customers is a deliberate business decision.

## Examples

See SB-FIN-SECOND-REMINDER.

## Exceptions

None.
