---
id: UC-CRW-INVITE-CREW-FOR-FUNCTION
type: use-case
title: "Invite crew members for a function"
status: draft
primary-actor: "ACT-CREW-PLANNER"
supporting-actors:
  - "ACT-CREW-MEMBER"
  - "ACT-RACCORD-AUTOMATION"
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-INVITATION-DISPATCH-BATCHING"
  - "BR-CRW-INVITATION-DISCLOSURE"
uses-terms:
  - "TERM-INVITATION"
  - "TERM-INVITATION-TYPE"
provenance:
  source: ".private/research/help-center/articles/17080436056978.md, .private/research/help-center/articles/360014444159.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Selected crew are asked about one or more functions.

## Trigger

A function needs people.

## Preconditions

Functions exist; crew members can receive invitations.

## Main Flow

1. The planner selects functions and crew members, possibly marking several invitations on the timeline before sending.
2. The planner chooses the invitation type.
3. Optionally the planner sets a due date, reminder, message, salutation and greeting, and adjusts the information shown.
4. The product sends the invitations.

## Alternative Flows

Invitations prepared on the planning timeline are sent together later (UC-CRW-SEND-PENDING-INVITATIONS).

## Failure Conditions

- The planner lacks permission to invite crew: the invitations are refused and none are sent.
- A selected crew member cannot receive invitations, for example without a usable email address: that invitation is refused with the reason and the others proceed.
- No function, crew member or invitation type is selected: sending is refused with the reason and nothing is sent.
- The mail service is unreachable: sending fails visibly and can be retried; delivery within 15 minutes is not guaranteed in that case (BR-CRW-INVITATION-DISPATCH-BATCHING).

## Postconditions

Invitations are open; invited crew show as invited.
