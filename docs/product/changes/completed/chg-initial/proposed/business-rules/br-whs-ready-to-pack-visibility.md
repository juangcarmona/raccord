---
id: BR-WHS-READY-TO-PACK-VISIBILITY
type: business-rule
title: "The warehouse sees only projects handed over"
status: draft
applies-to:
  - "UC-WHS-MARK-READY-TO-PACK"
  - "UC-WHS-REVIEW-WAREHOUSE-BOARD"
  - "BC-WAREHOUSE"
uses-terms:
  - "TERM-READY-TO-PACK"
  - "TERM-PACKING-LIST"
provenance:
  source: ".private/research/help-center/articles/38542583895186.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Packing lists of a project appear on the warehouse board only once the project is marked ready to pack, except for users whose role grants visibility of all projects regardless of the flag.

## Rationale

Prevents packing against a plan still being finalised.

## Examples

See SB-WHS-NOT-READY-HIDDEN.

## Exceptions

None.
