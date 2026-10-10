---
id: UC-CRW-MANAGE-CREW-LOGIN-ACCESS
type: use-case
title: "Manage a crew member's login access"
status: draft
primary-actor: "ACT-CREW-PLANNER"
supporting-actors: []
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-LOGIN-PERMISSION"
  - "BR-CRW-DEACTIVATE-RATHER-THAN-DELETE"
  - "BR-CRW-ACCOUNT-INVITATION-EXPIRY"
  - "BR-CRW-ACCOUNT-INVITATION-ON-CREATION"
uses-terms:
  - "TERM-ACCOUNT-INVITATION-STATUS"
  - "TERM-CREW-MEMBER"
provenance:
  source: ".private/research/help-center/articles/360013323900.md, .private/research/help-center/articles/360014366299.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Login access matches the person's relation to the company.

## Trigger

Someone joins, leaves, or did not accept the invitation in time.

## Preconditions

The crew member exists.

## Main Flow

1. The planner opens the crew member.
2. The planner switches login permission on or off, or resends the account invitation.
3. The product applies the change; enabling login or resending sends a new invitation.

## Alternative Flows

Bulk enabling login for imported crew sends all pending invitations at once.

## Failure Conditions

- The planner lacks permission to change a crew member's login access: the change is refused and access stays as it was (BR-CRW-LOGIN-PERMISSION).
- The account invitation was not accepted within 30 days: it no longer works and the planner must resend it, which opens a new window (BR-CRW-ACCOUNT-INVITATION-EXPIRY).
- The crew member has no usable email address, or the mail service is unreachable: the invitation is not delivered, the failure is shown and it can be resent.
- Another user changed the same crew member in the meantime: the change is refused and the planner must reload and retry.

## Postconditions

Departed people cannot log in yet keep their history; invited people have a fresh 30-day invitation.
