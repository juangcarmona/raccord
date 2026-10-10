---
id: SB-CRW-RATE-ARCHIVE-BLOCKED
type: structured-behaviour
title: "Archiving a rate used on future work is refused"
status: draft
illustrates:
  - "BR-CRW-RATE-ARCHIVE-PRECONDITION"
  - "UC-CRW-ARCHIVE-CREW-RATE"
given:
  - "A rate is used on a function next month"
when: "The administrator archives the rate"
then:
  - "Archiving is refused until the future function no longer uses the rate"
uses-terms:
  - "TERM-CREW-RATE-STATUS"
provenance:
  source: ".private/research/help-center/articles/7191443180562.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

History and future planning stay consistent.

## Boundaries

None.
