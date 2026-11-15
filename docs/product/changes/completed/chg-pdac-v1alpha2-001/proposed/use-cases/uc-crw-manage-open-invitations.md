---
id: UC-CRW-MANAGE-OPEN-INVITATIONS
type: use-case
title: "Follow up open invitations"
status: draft
primary-actor: "ACT-CREW-PLANNER"
supporting-actors: []
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-PLANNER-ANSWERS-ON-BEHALF"
uses-terms:
  - "TERM-INVITATION-STATUS"
provenance:
  source: ".private/research/help-center/articles/360013323900.md, .private/research/help-center/articles/360014625560.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Unanswered invitations are resolved.

## Trigger

A crew member answers outside the product or does not answer.

## Preconditions

Open invitations exist.

## Main Flow

1. The planner reviews a crew member's open invitations.
2. The planner accepts or declines on their behalf, withdraws, or sends a reminder.
3. The product updates the invitation.

## Alternative Flows

Planning views filter on open, answered and expired invitations.

## Failure Conditions

- The planner lacks permission to answer invitations for crew: the action is refused and the invitation stays open (BR-CRW-PLANNER-ANSWERS-ON-BEHALF).
- The invitation was already answered, withdrawn or expired: accepting, declining or withdrawing it again is refused.
- Another user or the crew member answered the same invitation in the meantime: the action is refused and the planner must reload and retry.
- A reminder cannot be delivered because the mail service is unreachable: the failure is shown and it can be resent.

## Postconditions

Invitations are answered, withdrawn or reminded.
