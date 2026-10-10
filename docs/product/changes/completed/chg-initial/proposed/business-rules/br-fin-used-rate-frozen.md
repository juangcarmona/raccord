---
id: BR-FIN-USED-RATE-FROZEN
type: business-rule
title: "A used VAT rate cannot be changed"
status: draft
applies-to:
  - "UC-FIN-CONFIGURE-VAT"
uses-terms:
  - "TERM-VAT-SCHEME"
provenance:
  source: ".private/research/help-center/articles/360017648360.md, .private/research/help-center/articles/360013887460.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Once a VAT rate has been used it cannot be modified; a rate change is handled by creating a new scheme and making it the default, keeping old schemes so past projects remain accurately taxed.

## Rationale

Historic documents must keep their tax basis.

## Examples

See SB-FIN-NEW-YEAR-VAT-SCHEME.

## Exceptions

None.
