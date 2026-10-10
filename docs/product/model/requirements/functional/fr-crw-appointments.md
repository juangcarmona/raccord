---
id: FR-CRW-APPOINTMENTS
type: functional-requirement
title: "Manage appointments"
status: draft
derived-from:
  - "UC-CRW-ADD-APPOINTMENT"
  - "UC-CRW-EDIT-APPOINTMENT"
  - "BR-CRW-RECURRENCE-SERIES"
  - "BR-CRW-RECURRING-EDIT-SCOPE"
  - "BR-CRW-SELF-PRESELECTED"
verification:
  - scenario: "Creating a weekly appointment on Mondays and Wednesdays until 12 February for two colleagues places it in both schedules on each of those days"
  - scenario: "Deleting only future occurrences of a recurring appointment keeps past ones"
uses-terms:
  - "TERM-APPOINTMENT"
  - "TERM-RECURRENCE-PATTERN"
provenance:
  source: ".private/research/help-center/articles/20884029308306.md, .private/research/help-center/articles/23976860498834.md, .private/research/help-center/articles/23917302179218.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST let users create, edit and delete one-off and recurring appointments for themselves and others, asking which occurrences a change affects.

## Rationale

Commitments outside projects.
