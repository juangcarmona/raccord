---
id: SB-CRW-WORKED-HOURS-NET-OF-BREAK-AND-TRAVEL
type: structured-behaviour
title: "Worked hours exclude break and travel"
status: draft
illustrates:
  - "BR-CRW-BREAK-AND-TRAVEL-EXCLUDED"
when: "The crew member saves a registration 08:00-18:00 with a 30-minute break and 1 hour travel"
then:
  - "The worked hours are 8.5"
  - "Travel time is shown separately as 1 hour"
uses-terms:
  - "TERM-TIME-REGISTRATION"
provenance:
  source: ".private/research/help-center/articles/360015498439.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Defines net worked time.

## Boundaries

None.
