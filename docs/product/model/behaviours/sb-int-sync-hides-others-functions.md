---
id: SB-INT-SYNC-HIDES-OTHERS-FUNCTIONS
type: structured-behaviour
title: "Restricted user sees only own functions"
status: draft
illustrates:
  - "BR-INT-SYNC-RESPECTS-ROLE"
given:
  - "the user's role does not allow viewing other people's planning"
when: "the user subscribes to the all-functions stream"
then:
  - "only functions the user's role may see are shown"
uses-terms:
  - "TERM-CALENDAR-STREAM"
provenance:
  source: ".private/research/help-center/articles/360014047779.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Role filtering.

## Boundaries

None.
