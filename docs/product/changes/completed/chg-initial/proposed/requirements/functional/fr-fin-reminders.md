---
id: FR-FIN-REMINDERS
type: functional-requirement
title: "Generate payment reminders"
status: draft
derived-from:
  - "UC-FIN-SEND-PAYMENT-REMINDER"
  - "BR-FIN-REMINDERS-MANUAL"
verification:
  - scenario-ref: "SB-FIN-SECOND-REMINDER"
uses-terms:
  - "TERM-PAYMENT-REMINDER"
provenance:
  source: ".private/research/help-center/articles/17195865170706.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let users generate successive reminders for overdue invoices with a new due date and track their number and last date.

## Rationale

Collecting overdue payments.
