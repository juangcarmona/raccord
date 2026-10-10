---
id: BR-CRW-PENDING-NOT-IN-TOTAL
type: business-rule
title: "Pending hours are not counted as worked"
status: draft
applies-to:
  - "UC-CRW-APPROVE-TIME-REGISTRATIONS"
uses-terms:
  - "TERM-TIME-REGISTRATION-STATUS"
provenance:
  source: ".private/research/help-center/articles/27336532785298.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

Only approved registrations count in the total worked time; pending hours are shown separately as hours still to approve.

## Rationale

Totals must be reliable.

## Examples

6 approved and 2 pending hours: worked 6, pending 2.

## Exceptions

None.
