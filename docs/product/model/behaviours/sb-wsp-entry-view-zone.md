---
id: SB-WSP-ENTRY-VIEW-ZONE
type: structured-behaviour
title: "Typed times follow the viewing time zone"
status: draft
illustrates:
  - "BR-WSP-ENTRY-IN-VIEWING-ZONE"
given:
  - "Multiple time zones are enabled"
  - "The planner's viewing time zone is Amsterdam"
  - "The project's time zone is New York"
when: "The planner types 09:00 as start of a function"
then:
  - "The function starts at 09:00 Amsterdam time"
  - "It shows as 03:00 for a viewer set to New York"
uses-terms:
  - "TERM-VIEWING-TIME-ZONE"
provenance:
  source: ".private/research/help-center/articles/115003579769.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Prevents misreading entered times.

## Boundaries

Assumes both zones are in summer time.
