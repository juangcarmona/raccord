---
id: SB-CRW-RECONCILIATION-EXCEEDS
type: structured-behaviour
title: "Registered longer than planned"
status: draft
illustrates:
  - "BR-CRW-RECONCILIATION-STATUS-DERIVATION"
  - "UC-CRW-RECONCILE-PLANNED-AND-REGISTERED-TIME"
given:
  - "A crew member was planned 8 hours on a past function"
  - "They registered 10 hours"
when: "The user views the activity"
then:
  - "Status is exceeds time"
  - "Difference time is 2 hours"
  - "Difference cost is the actual minus planned cost"
uses-terms:
  - "TERM-RECONCILIATION-STATUS"
provenance:
  source: ".private/research/help-center/articles/23814779870610.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Overrun visibility.

## Boundaries

None.
