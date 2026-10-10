---
id: BR-PRJ-TASK-STATUS-ENDPOINTS
type: business-rule
title: "To do and Completed always exist"
status: draft
applies-to:
  - "UC-PRJ-CONFIGURE-TASK-STATUSES"
uses-terms:
  - "TERM-TASK-STATUS-TO-DO"
  - "TERM-TASK-STATUS-COMPLETED"
provenance:
  source: ".private/research/help-center/articles/38421530397202.md"
  confidence: high
  recovered-from: documentation
---

## Rule

The statuses To do and Completed cannot be deleted; they may be renamed and recoloured and always frame the workflow.

## Rationale

Every workflow needs a start and an end, and calendar sync relies on them.

## Examples

Renaming Completed to Done is allowed; deleting it is not.

## Exceptions

None.
