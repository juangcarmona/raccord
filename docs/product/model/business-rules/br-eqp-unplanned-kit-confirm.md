---
id: BR-EQP-UNPLANNED-KIT-CONFIRM
type: business-rule
title: "Unplanned kits with content need confirmation"
status: draft
applies-to:
  - "UC-WHS-ADD-ADDITIONAL-EQUIPMENT"
uses-terms:
  - "TERM-PHYSICAL-KIT"
provenance:
  source: ".private/research/help-center/articles/20641705175058.md, .private/research/help-center/articles/20641412767378.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Scanning a kit with content that is not planned on the project asks for confirmation before adding it and its content; scanning content before its kit therefore leads to content being added twice.

## Rationale

The product never adds several items without the user knowing.

## Examples

- Scanning three cables, then their case, adds the cables twice; scanning the case first avoids this.

## Exceptions

None.
