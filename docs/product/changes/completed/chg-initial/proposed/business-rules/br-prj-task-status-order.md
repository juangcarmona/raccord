---
id: BR-PRJ-TASK-STATUS-ORDER
type: business-rule
title: "Status order is board column order"
status: draft
applies-to:
  - "UC-PRJ-CONFIGURE-TASK-STATUSES"
uses-terms:
  - "TERM-TASK-STATUS"
provenance:
  source: ".private/research/help-center/articles/38421530397202.md"
  confidence: high
  recovered-from: documentation
---

## Rule

The configured order of task statuses is the order of board columns and defines the 'next' status.

## Rationale

Boards should mirror the real flow.

## Examples

Moving Review before In progress changes the columns for everyone.

## Exceptions

None.
