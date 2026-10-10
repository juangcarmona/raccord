---
id: BR-WHS-DEFECT-LOSS-TIMING
type: business-rule
title: "Losses are reported before returned"
status: draft
applies-to:
  - "UC-WHS-MARK-LOST"
uses-terms:
  - "TERM-LOST-EQUIPMENT"
provenance:
  source: ".private/research/help-center/articles/13901986840082.md, .private/research/help-center/articles/17531415206418.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Equipment can be reported lost while it is in a status before returned; the report optionally books it back immediately (default: it stays where it is) so it is marked returned only when physically back.

## Rationale

Keeps tracking honest.

## Examples

A missing item found on unloading is reported and booked back at once.

## Exceptions

None.
