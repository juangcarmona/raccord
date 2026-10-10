---
id: BR-PRJ-TASK-CALENDAR-SYNC
type: business-rule
title: "Calendar sync of tasks knows two states"
status: draft
applies-to:
  - "BC-PROJECTS"
uses-terms:
  - "TERM-TASK-STATUS"
provenance:
  source: ".private/research/help-center/articles/360013703139.md, .private/research/help-center/articles/38421530397202.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Tasks synchronised to an external calendar carry only completed or not completed (every status other than Completed is not completed), and only tasks assigned to the user, to all crew, or private to the user are synchronised.

## Rationale

External calendars have no custom workflows.

## Examples

See SB-PRJ-REVIEW-STATUS-SYNCS-OPEN.

## Exceptions

None.
