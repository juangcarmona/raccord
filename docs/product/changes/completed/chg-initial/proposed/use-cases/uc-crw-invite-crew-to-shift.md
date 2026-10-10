---
id: UC-CRW-INVITE-CREW-TO-SHIFT
type: use-case
title: "Invite crew members to a shift"
status: draft
primary-actor: "ACT-CREW-PLANNER"
supporting-actors:
  - "ACT-CREW-MEMBER"
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-INVITATION-DISPATCH-BATCHING"
uses-terms:
  - "TERM-INVITATION"
  - "TERM-SHIFT"
provenance:
  source: ".private/research/help-center/articles/17621167421970.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Crew are asked to work a shift.

## Trigger

A shift needs people.

## Preconditions

The shift exists.

## Main Flow

1. The planner selects the shift and crew members and marks the invitations.
2. The planner sends them.

## Alternative Flows

None.

## Failure Conditions

- The planner lacks permission to invite crew: the invitations are refused and none are sent.
- No crew member is selected, or a selected one cannot receive invitations: that invitation is refused with the reason.
- The mail service is unreachable: sending fails visibly and can be retried, and delivery within 15 minutes is not met in the meantime (BR-CRW-INVITATION-DISPATCH-BATCHING).

## Postconditions

Invitations are open.
