---
id: BR-WHS-SHORTAGE-DETECTION
type: business-rule
title: "A shortage exists when planned demand exceeds availability"
status: draft
applies-to:
  - "UC-WHS-PLAN-EQUIPMENT"
  - "UC-WHS-REVIEW-SHORTAGES"
  - "BC-EQUIPMENT"
uses-terms:
  - "TERM-SHORTAGE"
  - "TERM-EQUIPMENT-AVAILABILITY"
provenance:
  source: ".private/research/help-center/articles/360013836939.md, .private/research/help-center/articles/360013158280.md, .private/research/help-center/articles/360017700639.md, .private/research/help-center/articles/360021519799.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A planned line is in shortage by the amount its planned quantity exceeds what is available to it at any moment of its planning period; the project, its warehouse cards and the cross-project shortage list show it.

## Rationale

Planners must know before the job which demand is uncovered.

## Examples

See SB-WHS-DOUBLE-BOOKED-SINGLE-UNIT.

## Exceptions

Optional lines are still planned and still count (inferred; not stated).
