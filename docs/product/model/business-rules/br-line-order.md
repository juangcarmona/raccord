---
id: BR-LINE-ORDER
type: business-rule
title: "Lines keep the order in which work was recorded"
status: draft
applies-to:
  - "UC-RECORD-BILLABLE-WORK"
  - "UC-CREATE-INVOICE-DRAFT"
uses-terms: []
---

## Rule

Billable lines keep the order in which they were recorded on the project, and invoice lines follow that order whatever order they were selected in.

## Rationale

Invoices must read the same way every time and match the project.

## Examples

See SB-DRAFT-FOLLOWS-PROJECT-ORDER.

## Exceptions

None.
