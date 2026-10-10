---
id: BR-OPERATION-DATE
type: business-rule
title: "Operation date is printed when it differs from the issue date"
status: draft
applies-to:
  - "UC-ISSUE-INVOICE"
uses-terms:
  - "TERM-OPERATION-DATE"
---

## Rule

The operation date of an invoice is the last day of the billed project and is printed only when it differs from the issue date.

## Rationale

Spanish invoices must show the operation date when it is different from the issue date.

## Examples

A shoot ending 18/09/2026 invoiced on 06/10/2026 prints both dates.

## Exceptions

None.
