---
id: BR-PRJ-TASK-SINGLE-SERVICE
type: business-rule
title: "A task belongs to at most one service"
status: draft
applies-to:
  - "UC-PRJ-MANAGE-SERVICES"
uses-terms:
  - "TERM-TASK"
  - "TERM-PRODUCTION-SERVICE"
provenance:
  source: ".private/research/help-center/articles/38423504883858.md, .private/research/help-center/articles/38421818533522.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A task can be linked to at most one service; shared work is split into separate tasks.

## Rationale

Hours must roll up unambiguously.

## Examples

See SB-PRJ-TASK-SECOND-SERVICE-REFUSED.

## Exceptions

None.
