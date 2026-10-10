---
id: SB-CRW-LATE-YES-REFUSED
type: structured-behaviour
title: "A yes after the function is full is refused"
status: draft
illustrates:
  - "BR-CRW-FINAL-INVITATION-CLOSES-WHEN-FILLED"
given:
  - "A function needs 2 people"
  - "Two invited crew members already answered yes to a final-planning invitation"
when: "A third crew member answers yes"
then:
  - "The answer is refused with a message that the function is already fully planned"
  - "The remaining invitations are expired"
uses-terms:
  - "TERM-INVITATION-STATUS"
provenance:
  source: ".private/research/help-center/articles/17080436056978.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

No overstaffing.

## Boundaries

None.
