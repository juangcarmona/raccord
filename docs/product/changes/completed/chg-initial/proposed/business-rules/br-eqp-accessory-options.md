---
id: BR-EQP-ACCESSORY-OPTIONS
type: business-rule
title: "Accessory options govern planning"
status: draft
applies-to:
  - "UC-EQP-CONFIGURE-ACCESSORIES"
  - "UC-WHS-PLAN-EQUIPMENT"
uses-terms:
  - "TERM-ACCESSORY"
provenance:
  source: ".private/research/help-center/articles/360013703140.md, .private/research/help-center/articles/4402870209682.md"
  confidence: high
  recovered-from: documentation
---

## Rule

When an item with accessories is planned: an automatic accessory is added without asking, a non-automatic one is offered for confirmation; an accessory marked skip-if-present is not added when already planned; one marked always-as-new-line gets its own line, otherwise its quantity merges into the existing line; one marked free is added at no charge.

## Rationale

Each company decides how insistent accessory suggestions are.

## Examples

- A rain cover set automatic and free is added silently at zero price with each moving head.

## Exceptions

None.
