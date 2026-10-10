---
id: BR-WHS-LOSS-REDUCES-STOCK
type: business-rule
title: "Losses reduce stock; finds restore it"
status: draft
applies-to:
  - "UC-WHS-MARK-LOST"
  - "UC-WHS-MARK-FOUND"
  - "BC-EQUIPMENT"
uses-terms:
  - "TERM-LOST-EQUIPMENT"
  - "TERM-STOCK-MOVEMENT"
provenance:
  source: ".private/research/help-center/articles/17531415206418.md, .private/research/help-center/articles/360014392200.md, .private/research/help-center/articles/13901986840082.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Marking units lost reduces stock by the lost quantity; marking them found, with the find date, counts them back into availability.

## Rationale

Availability must reflect what physically exists.

## Examples

See SB-WHS-FOUND-RESTORES.

## Exceptions

None.
