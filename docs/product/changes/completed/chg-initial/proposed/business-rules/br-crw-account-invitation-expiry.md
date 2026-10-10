---
id: BR-CRW-ACCOUNT-INVITATION-EXPIRY
type: business-rule
title: "Account invitations expire after 30 days"
status: draft
applies-to:
  - "UC-CRW-MANAGE-CREW-LOGIN-ACCESS"
  - "UC-CRW-SET-UP-OWN-PROFILE"
uses-terms:
  - "TERM-ACCOUNT-INVITATION-STATUS"
provenance:
  source: ".private/research/help-center/articles/360013323900.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

An account invitation that has not been accepted within 30 days expires; the planner can resend it, which opens a new 30-day window.

## Rationale

Limits the lifetime of login links.

## Examples

Invited on 1 March and not accepted: on 31 March the invitation no longer works and is resent.

## Exceptions

None.
