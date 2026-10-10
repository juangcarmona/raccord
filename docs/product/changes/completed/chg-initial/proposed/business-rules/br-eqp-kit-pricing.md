---
id: BR-EQP-KIT-PRICING
type: business-rule
title: "Kit pricing"
status: draft
applies-to:
  - "UC-EQP-DEFINE-PHYSICAL-KIT"
  - "UC-EQP-DEFINE-VIRTUAL-KIT"
  - "UC-WHS-ADJUST-KIT-CONTENT"
uses-terms:
  - "TERM-PHYSICAL-KIT"
  - "TERM-VIRTUAL-KIT"
provenance:
  source: ".private/research/help-center/articles/4402870209682.md, .private/research/help-center/articles/360017699799.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A physical kit has its own price regardless of its content (changeable per project); a virtual kit's price is either a fixed amount or the sum derived from its content, and after its content is changed on a project the planner may keep the kit price or recalculate it from the new content.

## Rationale

Cases are priced as a product; flexible sets may be priced as a bundle or by parts.

## Examples

- A virtual kit priced 100 whose content totals 10 + 20 + 80 stays at 100 unless recalculated to 110.

## Exceptions

None.
