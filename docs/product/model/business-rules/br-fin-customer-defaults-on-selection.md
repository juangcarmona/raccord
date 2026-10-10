---
id: BR-FIN-CUSTOMER-DEFAULTS-ON-SELECTION
type: business-rule
title: "Customer financial defaults are proposed when the client is chosen"
status: draft
applies-to:
  - "UC-FIN-APPLY-CUSTOMER-DEFAULTS"
uses-terms:
  - "TERM-CUSTOMER-FINANCIAL-DEFAULTS"
provenance:
  source: ".private/research/help-center/articles/360013886600.md, .private/research/help-center/articles/360013628279.md, .private/research/help-center/articles/360013887460.md, .private/research/help-center/articles/21200846347154.md"
  confidence: high
  recovered-from: documentation
---

## Rule

When a contact is selected as client (or location) of a project, the product asks whether to apply that contact's discounts, invoice moment, payment term and VAT scheme or tax rate, and copies them only if the user agrees.

## Rationale

Defaults save work but must not silently change an existing price.

## Examples

See SB-FIN-CUSTOMER-DISCOUNTS-PROMPTED.

## Exceptions

None.
