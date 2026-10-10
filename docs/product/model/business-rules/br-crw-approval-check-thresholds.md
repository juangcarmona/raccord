---
id: BR-CRW-APPROVAL-CHECK-THRESHOLDS
type: business-rule
title: "Approval signals grade differences at 30 minutes"
status: draft
applies-to:
  - "UC-CRW-APPROVE-TIME-REGISTRATIONS"
uses-terms:
  - "TERM-APPROVAL-CHECK"
provenance:
  source: ".private/research/help-center/articles/27336532785298.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

Comparisons of registered with scheduled hours and between crew members on the same activity are graded aligned when equal, small when under 30 minutes apart and large when over 30 minutes; not applicable when nothing is scheduled or no colleague registered. Linked activity is graded planned, manual or none.

## Rationale

Focus approvers on real deviations.

## Examples

Registered 8:20 against 8:00 scheduled is a small difference.

## Exceptions

Exactly 30 minutes is not specified.
