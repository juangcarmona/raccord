---
id: BR-CRW-OVERTIME-FROM-REGISTERED-HOURS
type: business-rule
title: "Overtime is calculated only from registered hours"
status: draft
applies-to:
  - "UC-CRW-RECONCILE-PLANNED-AND-REGISTERED-TIME"
  - "UC-CRW-REGISTER-HOURS-MANUALLY"
uses-terms:
  - "TERM-OVERTIME"
  - "TERM-TIME-REGISTRATION"
provenance:
  source: ".private/research/help-center/articles/7191443180562.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

Overtime cost is calculated only when a crew member registers more hours on a function than planned; the extra hours are priced with the crew rate attached to that assignment.

## Rationale

Planning shows intent; registration shows reality.

## Examples

Planned 10 hours, registered 11: the eleventh hour is costed at the overtime band.

## Exceptions

None.
