---
id: SB-PRJ-TASK-SECOND-SERVICE-REFUSED
type: structured-behaviour
title: "A task cannot join a second service"
status: draft
illustrates:
  - "BR-PRJ-TASK-SINGLE-SERVICE"
given:
  - "Task 'drawing' is linked to service 'Stage design'"
when: "A user links it to service 'Graphics'"
then:
  - "The task is linked to exactly one service"
  - "The attempted second link is refused"
  - "Its hours continue to roll up into the original service"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/38423504883858.md"
  confidence: medium
  recovered-from: documentation
---

## Intent

Unambiguous hour roll-up.

## Boundaries

The existing service link is preserved; the attempted second link is refused.
