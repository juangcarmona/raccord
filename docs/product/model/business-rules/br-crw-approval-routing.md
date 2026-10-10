---
id: BR-CRW-APPROVAL-ROUTING
type: business-rule
title: "Hour types requiring approval start pending"
status: draft
applies-to:
  - "UC-CRW-APPROVE-TIME-REGISTRATIONS"
  - "UC-CRW-CONFIGURE-HOUR-TYPES"
uses-terms:
  - "TERM-HOUR-TYPE"
  - "TERM-TIME-REGISTRATION-STATUS"
provenance:
  source: ".private/research/help-center/articles/27336532785298.md, .private/research/help-center/articles/24569529468562.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

Time of an hour type that requires approval is created as a pending time registration and needs approval; time of a type that does not require approval is recorded directly as a time registration.

## Rationale

Only reviewed hours reach payroll.

## Examples

With approval enabled on worked hours, a new registration shows pending.

## Exceptions

None.
