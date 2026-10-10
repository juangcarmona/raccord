---
id: BR-WHS-TEMPORARY-IS-SHORT
type: business-rule
title: "Temporary equipment is always short"
status: draft
applies-to:
  - "UC-WHS-ADD-TEMPORARY-EQUIPMENT"
  - "BC-EQUIPMENT"
uses-terms:
  - "TERM-TEMPORARY-EQUIPMENT"
  - "TERM-SHORTAGE"
provenance:
  source: ".private/research/help-center/articles/17005282714898.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A temporary equipment line has no owned stock, so its whole planned quantity is shown as a shortage which tells the planner it must be obtained elsewhere.

## Rationale

One-off articles have no stock of their own.

## Examples

See SB-WHS-TEMPORARY-ITEM-SHORT.

## Exceptions

None.
