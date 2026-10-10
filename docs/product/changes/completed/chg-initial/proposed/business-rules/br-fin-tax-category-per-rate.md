---
id: BR-FIN-TAX-CATEGORY-PER-RATE
type: business-rule
title: "Each VAT rate states its tax category code"
status: draft
applies-to:
  - "UC-FIN-CONFIGURE-E-INVOICING"
uses-terms:
  - "TERM-TAX-CATEGORY-CODE"
provenance:
  source: ".private/research/help-center/articles/360014032939.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Every rate of every VAT scheme used in e-invoices must carry its standard tax category code.

## Rationale

Receivers interpret tax treatment from the code.

## Examples

The standard rate maps to the standard-rate category, reverse charge to the reverse-charge category.

## Exceptions

None.
