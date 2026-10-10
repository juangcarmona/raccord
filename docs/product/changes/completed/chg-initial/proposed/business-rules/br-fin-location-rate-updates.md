---
id: BR-FIN-LOCATION-RATE-UPDATES
type: business-rule
title: "Location tax rates are refreshed monthly with history"
status: draft
applies-to:
  - "UC-FIN-MAINTAIN-LOCATION-TAX-RATES"
uses-terms:
  - "TERM-LOCATION-TAX-RATE"
provenance:
  source: ".private/research/help-center/articles/21200846347154.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A tax rate linked to a postal code is updated automatically on the first day of each month if the rate changed, mid-month changes take effect at the next monthly update, earlier rates are kept as history, rates remain manually editable, and a notice is shown in the financial overview of affected projects.

## Rationale

Local sales-tax rates change frequently.

## Examples

See SB-FIN-MID-MONTH-RATE-CHANGE.

## Exceptions

None.
