---
id: FR-CRW-ACCOUNT-INVITATIONS
type: functional-requirement
title: "Invite crew members to log in"
status: draft
derived-from:
  - "UC-CRW-MANAGE-CREW-LOGIN-ACCESS"
  - "BR-CRW-ACCOUNT-INVITATION-ON-CREATION"
  - "BR-CRW-ACCOUNT-INVITATION-EXPIRY"
  - "BR-CRW-LOGIN-PERMISSION"
verification:
  - scenario-ref: "SB-CRW-ACCOUNT-INVITATION-EXPIRED"
  - scenario-ref: "SB-CRW-IMPORT-WITHOUT-LOGIN"
uses-terms:
  - "TERM-ACCOUNT-INVITATION-STATUS"
provenance:
  source: ".private/research/help-center/articles/360013323900.md, .private/research/help-center/articles/360014366299.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST send account invitations to crew members whose login is allowed, expire them after 30 days, allow resending, and prevent login when permission is off while keeping history.

## Rationale

Crew need their own access.
