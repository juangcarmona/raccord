---
id: BR-EQP-ACCESSORIES-NOT-INHERITED
type: business-rule
title: "Accessories are not inherited by kits"
status: draft
applies-to:
  - "UC-EQP-CONFIGURE-ACCESSORIES"
uses-terms:
  - "TERM-ACCESSORY"
  - "TERM-VIRTUAL-KIT"
provenance:
  source: ".private/research/help-center/articles/360013703140.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Accessories belong to the item on which they are defined; a kit containing that item does not acquire its accessories.

## Rationale

The kit may already contain the accessory, which would then be planned twice.

## Examples

- A microphone accessory of a media player is not added when planning a DJ set kit that contains the player and already a microphone.

## Exceptions

None.
