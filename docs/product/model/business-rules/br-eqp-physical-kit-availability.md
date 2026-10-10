---
id: BR-EQP-PHYSICAL-KIT-AVAILABILITY
type: business-rule
title: "Physical kit availability is that of an item"
status: draft
applies-to:
  - "UC-EQP-DEFINE-PHYSICAL-KIT"
uses-terms:
  - "TERM-PHYSICAL-KIT"
  - "TERM-EQUIPMENT-AVAILABILITY"
provenance:
  source: ".private/research/help-center/articles/4402870209682.md"
  confidence: high
  recovered-from: documentation
---

## Rule

The availability of a physical kit is computed from the kit's own stock exactly like a single item, without re-checking its content; binding more content than the content item's stock creates a lasting shortage of that content item.

## Rationale

The content is already allocated to the kit.

## Examples

- Five kits each holding one item A while only 3 item A are owned: item A shows minus 2, a permanent shortage.

## Exceptions

None.
