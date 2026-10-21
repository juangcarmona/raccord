---
id: BR-FIN-DEFAULT-VAT-CLASS
type: business-rule
title: "New items receive the default VAT class"
status: active
applies-to:
  - "UC-FIN-CONFIGURE-VAT"
uses-terms:
  - "TERM-VAT-CLASS"
provenance:
  source: ".private/research/help-center/articles/360013887460.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Newly created equipment, functions and contacts receive the default VAT class or scheme from the financial settings; each item's class can then be changed individually or in bulk. A new Spanish installation uses its standard-rate class and a 21% scheme as the initial editable defaults, verified against the Agencia Tributaria on 2026-10-21. This default does not determine the correct treatment for every operation.

## Rationale

Avoids untaxed items by omission.

## Examples

A new speaker starts with the configured default class; the administrator reviews or changes its class based on the applicable treatment.

## Exceptions

None.
