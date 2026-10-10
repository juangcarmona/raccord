---
id: SB-WHS-CUSTOM-STATUS-UNAVAILABLE
type: structured-behaviour
title: "Gear in a custom status is unavailable"
status: draft
illustrates:
  - "BR-WHS-CUSTOM-STATUS-OUT"
given:
  - "10 units are owned"
  - "A job's 3 units are booked into the custom status 'In transit' on their way back"
when: "A planner checks which units are in the warehouse"
then:
  - "The 3 units are counted as out of the warehouse"
  - "The project's financial state is unchanged"
uses-terms:
  - "TERM-CUSTOM-WAREHOUSE-STATUS"
provenance:
  source: ".private/research/help-center/articles/15239120279442.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Custom statuses are out of the warehouse and financially neutral.

## Boundaries

Units in a custom status remain unavailable until they are returned to an available warehouse status; the planning period does not make physical stock available.
