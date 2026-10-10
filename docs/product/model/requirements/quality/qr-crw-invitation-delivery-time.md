---
id: QR-CRW-INVITATION-DELIVERY-TIME
type: quality-requirement
title: "Invitations are delivered within 15 minutes"
status: draft
quality-attribute: "timeliness"
applies-to:
  - "UC-CRW-SEND-PENDING-INVITATIONS"
  - "UC-CRW-INVITE-CREW-FOR-FUNCTION"
verification:
  - scenario-ref: "SB-CRW-INVITATIONS-COMBINED"
uses-terms:
  - "TERM-INVITATION"
provenance:
  source: ".private/research/help-center/articles/17080436056978.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

Sent invitations MUST reach recipients within 15 minutes, combining invitations to the same person within that window.

## Measurement

Time between sending and email dispatch, measured over invitations sent; threshold 15 minutes for every invitation.
