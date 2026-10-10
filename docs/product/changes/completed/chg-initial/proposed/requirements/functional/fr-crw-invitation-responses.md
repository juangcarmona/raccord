---
id: FR-CRW-INVITATION-RESPONSES
type: functional-requirement
title: "Apply invitation responses"
status: draft
derived-from:
  - "UC-CRW-RESPOND-TO-INVITATION"
  - "UC-CRW-MANAGE-OPEN-INVITATIONS"
  - "BR-CRW-FINAL-INVITATION-EFFECT"
  - "BR-CRW-FINAL-INVITATION-CLOSES-WHEN-FILLED"
  - "BR-CRW-DRAFT-INVITATION-EFFECT"
  - "BR-CRW-AVAILABILITY-INVITATION-EFFECT"
  - "BR-CRW-PLANNER-ANSWERS-ON-BEHALF"
verification:
  - scenario-ref: "SB-CRW-FINAL-INVITATION-ACCEPTED"
  - scenario-ref: "SB-CRW-LATE-YES-REFUSED"
  - scenario-ref: "SB-CRW-DRAFT-OVERSUBSCRIBED"
  - scenario-ref: "SB-CRW-AVAILABILITY-INVITATION-NO"
uses-terms:
  - "TERM-INVITATION-STATUS"
provenance:
  source: ".private/research/help-center/articles/17080436056978.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST apply each invitation type's effect on response, close final-planning invitations when the function is full, and let planners answer, withdraw or remind on behalf of crew.

## Rationale

Responses turn into planning without retyping.
