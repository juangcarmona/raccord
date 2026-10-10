---
id: FR-CRW-INVITATIONS
type: functional-requirement
title: "Invite crew with typed invitations"
status: draft
derived-from:
  - "UC-CRW-INVITE-CREW-FOR-FUNCTION"
  - "UC-CRW-INVITE-CREW-FOR-PERIOD"
  - "UC-CRW-SEND-PENDING-INVITATIONS"
  - "UC-CRW-INVITE-CREW-TO-SHIFT"
  - "BR-CRW-PERIOD-INVITATION-TYPES"
  - "BR-CRW-INVITATION-DISPATCH-BATCHING"
verification:
  - scenario-ref: "SB-CRW-INVITATIONS-COMBINED"
  - scenario-ref: "SB-CRW-PERIOD-INVITATION-NO-PLANNING-TYPE"
uses-terms:
  - "TERM-INVITATION"
  - "TERM-INVITATION-TYPE"
provenance:
  source: ".private/research/help-center/articles/17080436056978.md, .private/research/help-center/articles/360014444159.md, .private/research/help-center/articles/17621167421970.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST send availability, final-planning, draft-planning and reservation invitations for functions and shifts, and availability or reservation invitations for periods, with optional due date, reminder and message, by email, on the web and in the mobile web app.

## Rationale

Structured staffing communication.
