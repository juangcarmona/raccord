---
id: BR-PRJ-ADDRESS-ROLES
type: business-rule
title: "Each contact address has its own purpose"
status: draft
applies-to:
  - "UC-PRJ-ADD-CONTACT"
uses-terms:
  - "TERM-VISITING-ADDRESS"
  - "TERM-MAILING-ADDRESS"
  - "TERM-BILLING-ADDRESS"
provenance:
  source: ".private/research/help-center/articles/360014254000.md"
  confidence: high
  recovered-from: documentation
---

## Rule

The visiting address drives distance and travel time, the mailing address is printed by default on quotations and contracts, and the billing address on invoices; templates may override the printing defaults.

## Rationale

Large customers use different addresses per purpose.

## Examples

See SB-PRJ-BILLING-ADDRESS-ON-INVOICE.

## Exceptions

None.
