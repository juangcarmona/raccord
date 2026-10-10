---
id: SB-CRW-RATE-CHANGE-FORWARD-ONLY
type: structured-behaviour
title: "A rate change does not alter existing planning"
status: draft
illustrates:
  - "BR-CRW-RATE-CHANGE-NOT-RETROACTIVE"
given:
  - "A crew member with the rate is planned on a function"
when: "The administrator raises the rate's hourly amount"
then:
  - "The existing assignment keeps the old amount"
  - "A crew member planned afterwards gets the new amount"
uses-terms:
  - "TERM-CREW-RATE"
provenance:
  source: ".private/research/help-center/articles/7191443180562.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

No silent re-costing.

## Boundaries

None.
