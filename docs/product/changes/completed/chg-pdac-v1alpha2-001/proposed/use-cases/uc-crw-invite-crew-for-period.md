---
id: UC-CRW-INVITE-CREW-FOR-PERIOD
type: use-case
title: "Invite crew members for a time period"
status: draft
primary-actor: "ACT-CREW-PLANNER"
supporting-actors:
  - "ACT-CREW-MEMBER"
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-PERIOD-INVITATION-TYPES"
  - "BR-CRW-AVAILABILITY-INVITATION-EFFECT"
  - "BR-CRW-RESERVATION-INVITATION-EFFECT"
uses-terms:
  - "TERM-INVITATION"
  - "TERM-INVITATION-TYPE"
provenance:
  source: ".private/research/help-center/articles/17080436056978.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Availability or a reservation is asked for a period without a function.

## Trigger

Gathering capacity ahead of time.

## Preconditions

Crew exist.

## Main Flow

1. The planner selects crew and a period.
2. The planner chooses availability or reservation.
3. The planner adds optional due date, reminder and message.
4. The product sends the invitations.

## Alternative Flows

None.

## Failure Conditions

- The planner lacks permission to invite crew: the invitations are refused and none are sent.
- A type other than availability or reservation is requested for a period without a function: it is refused, since only those two types exist for periods (BR-CRW-PERIOD-INVITATION-TYPES).
- No crew member or no valid period is selected, or a crew member cannot receive invitations: the invitation is refused with the reason.
- The mail service is unreachable: sending fails visibly and can be retried.

## Postconditions

Invitations are open.
