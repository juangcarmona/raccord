---
id: BR-CRW-DEFAULT-RATE-ON-ASSIGNMENT
type: business-rule
title: "Planning applies the crew member's default rate unless another is chosen"
status: draft
applies-to:
  - "UC-CRW-PLAN-RESOURCE-ON-FUNCTION"
  - "UC-CRW-CHANGE-ASSIGNMENT-DETAILS"
uses-terms:
  - "TERM-CREW-RATE"
  - "TERM-CREW-ASSIGNMENT"
provenance:
  source: ".private/research/help-center/articles/360013323900.md, .private/research/help-center/articles/360014594620.md, .private/research/help-center/articles/360014687739.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

When a crew member is planned, their default rate is applied to the assignment; the planner may instead pick another of the person's linked rates or enter a custom rate for that assignment.

## Rationale

People with several skills are paid differently per role.

## Examples

A technician who is also a driver is planned as driver with the driver rate.

## Exceptions

None.
