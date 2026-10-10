---
id: BR-CRW-RECURRING-SHIFT-PLANNING
type: business-rule
title: "Resources chosen at creation are planned on all recurrences"
status: draft
applies-to:
  - "UC-CRW-PLAN-RESOURCE-ON-SHIFT"
  - "UC-CRW-CREATE-SHIFT"
uses-terms:
  - "TERM-SHIFT"
  - "TERM-RECURRENCE-PATTERN"
provenance:
  source: ".private/research/help-center/articles/17592602608018.md, .private/research/help-center/articles/26061527515538.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

Crew members and vehicles selected while creating a recurring shift are planned on every occurrence; afterwards the planner may plan a person on all occurrences or only on future ones.

## Rationale

Recurring work usually has a fixed team.

## Examples

A weekly cleaning shift created with two people has both on every week.

## Exceptions

None.
