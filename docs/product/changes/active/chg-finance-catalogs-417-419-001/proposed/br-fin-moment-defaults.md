---
id: BR-FIN-MOMENT-DEFAULTS
type: business-rule
title: "Invoice moment defaults come from customer or project type"
status: active
applies-to:
  - "UC-FIN-CONFIGURE-INVOICE-MOMENTS"
  - "UC-FIN-APPLY-CUSTOMER-DEFAULTS"
uses-terms:
  - "TERM-INVOICE-MOMENT"
  - "TERM-CUSTOMER-FINANCIAL-DEFAULTS"
provenance:
  source: ".private/research/help-center/articles/360013628279.md, .private/research/help-center/articles/360013965120.md, .private/research/help-center/articles/360017648200.md, .private/research/help-center/articles/360017647300.md"
  confidence: medium
  recovered-from: documentation
---

## Rule

A project takes the default invoice moment of its project type, or that of its client when the user applies the client's defaults, and the user can change it per project.

## Rationale

Recurring arrangements should not be re-entered.

## Examples

A client with 100% upfront overrides the project type's invoice-afterwards moment once applied.

## Exceptions

None.
