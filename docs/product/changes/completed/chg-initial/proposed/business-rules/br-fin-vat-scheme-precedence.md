---
id: BR-FIN-VAT-SCHEME-PRECEDENCE
type: business-rule
title: "VAT scheme precedence"
status: draft
applies-to:
  - "UC-FIN-SELECT-VAT-SCHEME"
  - "UC-FIN-APPLY-CUSTOMER-DEFAULTS"
uses-terms:
  - "TERM-VAT-SCHEME"
  - "TERM-CUSTOMER-FINANCIAL-DEFAULTS"
provenance:
  source: ".private/research/help-center/articles/360013887460.md, .private/research/help-center/articles/21200846347154.md"
  confidence: medium
  recovered-from: documentation
---

## Rule

A project uses the default VAT scheme unless its client (or location) has a scheme that is applied, and a scheme chosen on the project or on an individual invoice overrides both for that project or invoice; choosing a different scheme on an invoice takes effect when the invoice is regenerated.

## Rationale

Foreign customers and special cases need different taxation.

## Examples

See SB-FIN-FOREIGN-CLIENT-REVERSE-CHARGE.

## Exceptions

None.
