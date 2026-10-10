---
id: SB-CRW-BELOW-MINIMUM-PAID
type: structured-behaviour
title: "Registration below minimum paid hours"
status: draft
illustrates:
  - "BR-CRW-MINIMUM-PAID-HOURS-CHECK"
given:
  - "Minimum paid hours is 4"
when: "The approver reviews a 3-hour registration"
then:
  - "The minimum-paid-hours signal flags it"
uses-terms:
  - "TERM-MINIMUM-PAID-HOURS"
provenance:
  source: ".private/research/help-center/articles/27336532785298.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Legal minimum check.

## Boundaries

None.
