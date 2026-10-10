---
id: BR-PRJ-TASK-NOT-INVOICEABLE
type: business-rule
title: "Tasks are charged only through services"
status: draft
applies-to:
  - "UC-PRJ-MANAGE-SERVICES"
uses-terms:
  - "TERM-TASK"
  - "TERM-PRODUCTION-SERVICE"
provenance:
  source: ".private/research/help-center/articles/38421818533522.md, .private/research/help-center/articles/38423504883858.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A task has no price and cannot be invoiced on its own; its hours become internal cost, and to charge for the work the task must be linked to a service.

## Rationale

Separates effort from sale.

## Examples

Editing footage is billed by linking the edit task to a 'Recap video' service.

## Exceptions

None.
