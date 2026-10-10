---
id: SB-CRW-NEW-REGISTRATION-PENDING
type: structured-behaviour
title: "New worked hours start pending when approval is on"
status: draft
illustrates:
  - "BR-CRW-APPROVAL-ROUTING"
  - "BR-CRW-PENDING-NOT-IN-TOTAL"
given:
  - "The worked hour type requires approval"
when: "A crew member registers 6 hours"
then:
  - "The registration is pending"
  - "It is not included in the total worked time"
  - "It counts in the pending hours"
uses-terms:
  - "TERM-TIME-REGISTRATION-STATUS"
provenance:
  source: ".private/research/help-center/articles/27336532785298.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Approval gate.

## Boundaries

None.
