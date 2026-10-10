---
id: FR-CRW-CREW-RECORDS
type: functional-requirement
title: "Maintain crew member records"
status: draft
derived-from:
  - "UC-CRW-ADD-CREW-MEMBER"
  - "UC-CRW-EDIT-CREW-MEMBERS"
  - "BR-CRW-CREW-MEMBER-MINIMUM-DATA"
  - "BR-CRW-PROFILE-OWNERSHIP"
verification:
  - scenario-ref: "SB-CRW-SELF-MAINTAINED-PROFILE-LOCKED"
  - scenario: "A crew member saved with first name, role and email appears in the chosen folder with its skill tags"
uses-terms:
  - "TERM-CREW-MEMBER"
provenance:
  source: ".private/research/help-center/articles/360013323900.md, .private/research/help-center/articles/360014360520.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST let planners create, edit and bulk-edit crew members with contact, administrative, planning and skill data, requiring first name, user role and email, and MUST respect profile ownership.

## Rationale

Crew records are the basis of all staffing.
