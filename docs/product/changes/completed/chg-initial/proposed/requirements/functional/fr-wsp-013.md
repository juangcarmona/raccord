---
id: FR-WSP-013
type: functional-requirement
title: "Role assignment and review"
status: draft
derived-from:
  - "UC-WSP-ASSIGN-ROLE"
  - "UC-WSP-REVIEW-ROLE-ASSIGNMENTS"
  - "BR-WSP-NEW-USER-DEFAULT-ROLE"
verification:
  - scenario: "A new crew member without explicit role receives the Crew role"
  - scenario: "Filtering crew members by role lists exactly the holders of that role"
uses-terms:
  - "TERM-USER-ROLE"
provenance:
  source: ".private/research/help-center/articles/360012611160.md, .private/research/help-center/articles/360014360700.md, .private/research/help-center/articles/360015898239.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let authorized users assign one role per user, give new users the Crew role, and list users per role.

## Rationale

Access administration.
