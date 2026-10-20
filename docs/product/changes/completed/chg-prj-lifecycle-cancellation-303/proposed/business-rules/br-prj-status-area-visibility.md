---
id: BR-PRJ-STATUS-AREA-VISIBILITY
type: business-rule
title: "Status decides where a project appears"
status: active
applies-to:
  - "UC-PRJ-CHANGE-PROJECT-STATUS"
  - "BC-PROJECTS"
uses-terms:
  - "TERM-PROJECT-STATUS"
  - "TERM-PROJECT-STATUS-GROUP"
provenance:
  source: ".private/research/help-center/articles/360013587379.md, .private/research/help-center/articles/23361953107346.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Warehouse handling shows only projects in the Confirmed group, with equipment bookable from Confirmed onwards; invoicing lists only confirmed projects; crew planning shows cancelled projects only while crew is still planned on them; cancelled projects disappear from operational overviews but remain in statistics.

## Rationale

Each area acts only on projects that are relevant to it.

## Examples

See SB-PRJ-DRAFT-NOT-IN-WAREHOUSE and SB-PRJ-CANCELLED-WITH-CREW-IN-PLANNER.

## Exceptions

None.
