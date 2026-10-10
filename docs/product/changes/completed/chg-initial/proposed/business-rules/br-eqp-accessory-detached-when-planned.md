---
id: BR-EQP-ACCESSORY-DETACHED-WHEN-PLANNED
type: business-rule
title: "Accessories become independent lines"
status: draft
applies-to:
  - "UC-EQP-CONFIGURE-ACCESSORIES"
uses-terms:
  - "TERM-ACCESSORY"
  - "TERM-PLANNED-EQUIPMENT"
provenance:
  source: ".private/research/help-center/articles/360013703140.md, .private/research/help-center/articles/4402870209682.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Once added to a project, an accessory is an ordinary line with no remaining link to its item; its stock, availability, weight and volume are computed separately.

## Rationale

Accessories are a planning shortcut, not a structure.

## Examples

- Removing the moving head from the project leaves the rain cover line in place.

## Exceptions

None.
