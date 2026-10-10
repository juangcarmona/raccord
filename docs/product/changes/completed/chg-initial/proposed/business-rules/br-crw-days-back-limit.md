---
id: BR-CRW-DAYS-BACK-LIMIT
type: business-rule
title: "Own hours are editable only a configured number of days back"
status: draft
applies-to:
  - "UC-CRW-REGISTER-HOURS-MANUALLY"
  - "UC-CRW-REGISTER-HOURS-FROM-PLANNED-ACTIVITIES"
uses-terms:
  - "TERM-TIME-REGISTRATION-RIGHTS"
provenance:
  source: ".private/research/help-center/articles/360015498839.md, .private/research/help-center/articles/360015498439.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

Users can view, edit or remove their own registrations, and see activities to register, only within the configured number of past days.

## Rationale

Late changes undermine payroll.

## Examples

With 7 days back, an activity of 9 days ago is not offered.

## Exceptions

None.
