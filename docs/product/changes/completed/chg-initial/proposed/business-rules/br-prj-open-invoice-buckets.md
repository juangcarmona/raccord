---
id: BR-PRJ-OPEN-INVOICE-BUCKETS
type: business-rule
title: "Open invoice widget buckets"
status: draft
applies-to:
  - "UC-PRJ-CONFIGURE-DASHBOARD"
uses-terms:
  - "TERM-DASHBOARD-WIDGET"
  - "TERM-PAYMENT-STATUS"
provenance:
  source: ".private/research/help-center/articles/360014243139.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Open invoices are unpaid invoices created up to today whose due date has not passed; overdue invoices are unpaid ones past due; to be invoiced are projects without invoice or only partially invoiced whose invoice moment is today or earlier.

## Rationale

Cash follow-up at a glance.

## Examples

An unpaid invoice due yesterday counts as overdue, not open.

## Exceptions

None.
