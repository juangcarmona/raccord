---
id: BR-FIN-VAT-FROM-CLASS-AND-SCHEME
type: business-rule
title: "VAT rate = scheme percentage for the line's class"
status: draft
applies-to:
  - "UC-FIN-CONFIGURE-VAT"
  - "UC-FIN-SELECT-VAT-SCHEME"
uses-terms:
  - "TERM-VAT-CLASS"
  - "TERM-VAT-SCHEME"
  - "TERM-VAT-SCHEME-KIND"
  - "TERM-REVERSE-CHARGE"
provenance:
  source: ".private/research/help-center/articles/360013887460.md, .private/research/help-center/articles/360017648360.md, .private/research/help-center/articles/360017648540.md, .private/research/help-center/articles/360015935839.md"
  confidence: high
  recovered-from: documentation
---

## Rule

The VAT percentage of a project or invoice line is the percentage that the applicable VAT scheme assigns to the line's VAT class; a fixed-rate scheme applies its single rate to every line regardless of class, and a reverse-charge scheme applies 0%.

## Rationale

Rates change with law and customer; classes stay with the item.

## Examples

See SB-FIN-SCHEME-MAPS-CLASSES.

## Exceptions

None.
