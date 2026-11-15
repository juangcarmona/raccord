---
id: UC-CRW-SEND-PENDING-INVITATIONS
type: use-case
title: "Send prepared invitations in one go"
status: draft
primary-actor: "ACT-CREW-PLANNER"
supporting-actors:
  - "ACT-RACCORD-AUTOMATION"
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-INVITATION-DISPATCH-BATCHING"
  - "BR-CRW-INVITATION-DISCLOSURE"
uses-terms:
  - "TERM-INVITATION-STATUS"
provenance:
  source: ".private/research/help-center/articles/360014444159.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

All prepared invitations go out together.

## Trigger

The planner finished preparing invitations.

## Preconditions

Unsent invitations exist; the product shows their count.

## Main Flow

1. The planner selects projects or shifts and, optionally, specific crew members.
2. The planner chooses the invitation type and optional texts.
3. The product sends the invitations.

## Alternative Flows

None.

## Failure Conditions

- The planner lacks permission to send invitations: none are sent and the prepared ones stay unsent.
- No unsent invitations match the chosen projects, shifts or crew members: nothing is sent and the planner is told so.
- The mail service is unreachable: sending fails visibly, the invitations stay unsent and sending can be retried (BR-CRW-INVITATION-DISPATCH-BATCHING).
- A crew member who cannot receive invitations is skipped with the reason while the others are sent.

## Postconditions

Prepared invitations become open.
