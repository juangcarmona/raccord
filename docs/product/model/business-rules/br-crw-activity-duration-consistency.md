---
id: BR-CRW-ACTIVITY-DURATION-CONSISTENCY
type: business-rule
title: "Changed times require consistent activity durations"
status: draft
applies-to:
  - "UC-CRW-REGISTER-HOURS-FROM-PLANNED-ACTIVITIES"
  - "UC-CRW-EXPORT-PAYROLL-DATA"
uses-terms:
  - "TERM-REGISTRATION-ACTIVITY"
provenance:
  source: ".private/research/help-center/articles/360015498439.md, .private/research/help-center/articles/23814779870610.md"
  confidence: "medium"
  recovered-from: "documentation"
---

## Rule

When the start or end of a registration changes, its activity durations must be adjusted to match the new time and break, or another activity added; time not covered by activities counts as hours without activities.

## Rationale

Linked costs and exports rely on activities.

## Examples

Registration 09:00-18:00 with one 4-hour activity has 5 hours without activities.

## Exceptions

None.
