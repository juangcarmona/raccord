---
id: BR-WHS-PACKING-LIST-PER-DATE
type: business-rule
title: "One packing list per pickup or return moment"
status: draft
applies-to:
  - "UC-WHS-REVIEW-WAREHOUSE-BOARD"
  - "UC-WHS-SET-LINE-TIMES"
  - "BC-WAREHOUSE"
uses-terms:
  - "TERM-PACKING-LIST"
  - "TERM-PLANNING-PERIOD"
provenance:
  source: ".private/research/help-center/articles/38542583895186.md"
  confidence: high
  recovered-from: documentation
---

## Rule

For each project the product forms one packing list per distinct start of planning period (outgoing) and per distinct end of planning period (returning) among its lines; each list contains exactly the lines leaving or returning at that moment.

## Rationale

The crew should see only what moves at that moment.

## Examples

See SB-WHS-TWO-PICKUP-DATES-TWO-LISTS.

## Exceptions

None.
