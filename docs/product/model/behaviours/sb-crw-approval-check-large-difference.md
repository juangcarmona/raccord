---
id: SB-CRW-APPROVAL-CHECK-LARGE-DIFFERENCE
type: structured-behaviour
title: "Registered time two hours over schedule"
status: draft
illustrates:
  - "BR-CRW-APPROVAL-CHECK-THRESHOLDS"
given:
  - "A crew member was scheduled 4 hours and registered 6 hours"
when: "The approver reviews the registration"
then:
  - "The registered-versus-scheduled signal shows a large difference"
uses-terms:
  - "TERM-APPROVAL-CHECK"
provenance:
  source: ".private/research/help-center/articles/27336532785298.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Large deviations stand out.

## Boundaries

None.
