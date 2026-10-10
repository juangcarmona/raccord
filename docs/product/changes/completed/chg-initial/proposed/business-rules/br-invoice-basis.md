---
id: BR-INVOICE-BASIS
type: business-rule
title: "Invoices bill selected lines or a project advance"
status: draft
applies-to:
  - "UC-CREATE-INVOICE-DRAFT"
  - "UC-FIN-GENERATE-PROJECT-INVOICE"
uses-terms:
  - "TERM-INVOICE-DRAFT"
  - "TERM-BILLABLE-LINE"
  - "TERM-INVOICE-DEDUCTION"
---

## Rule

An invoice draft either bills selected unbilled lines of a project (BR-SINGLE-BILLING) or bills an advance: a percentage of the project's current total. Every issued advance of a project is deducted automatically from the next invoices of that project until the advances are consumed, so all invoices of a project together never exceed what was billed.

## Rationale

Rental and event work is often paid partly in advance; line billing keeps item-level precision for the final invoice.

## Examples

See SB-ADVANCE-DEDUCTED-ON-FINAL.

## Exceptions

Rectifying invoices are not reduced by advances.
