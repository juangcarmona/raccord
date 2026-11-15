---
id: UC-CRW-RESPOND-TO-INVITATION
type: use-case
title: "Respond to an invitation"
status: draft
primary-actor: "ACT-CREW-MEMBER"
supporting-actors: []
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-AVAILABILITY-INVITATION-EFFECT"
  - "BR-CRW-FINAL-INVITATION-EFFECT"
  - "BR-CRW-FINAL-INVITATION-CLOSES-WHEN-FILLED"
  - "BR-CRW-DRAFT-INVITATION-EFFECT"
  - "BR-CRW-RESERVATION-INVITATION-EFFECT"
  - "BR-CRW-RESERVATION-WITHOUT-DETAILS"
uses-terms:
  - "TERM-INVITATION-STATUS"
  - "TERM-INVITATION-TYPE"
provenance:
  source: ".private/research/help-center/articles/17080436056978.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

The crew member answers and the answer takes effect.

## Trigger

The crew member receives an invitation by email, on the web or in the mobile web app.

## Preconditions

The invitation is open.

## Main Flow

1. The crew member opens the invitation, on the web, by email or in the mobile web app.
2. They answer yes or no.
3. The product records the answer and applies the type's effect: availability, final or draft assignment, or reservation.

## Alternative Flows

Notifications announce new invitations when enabled.

## Failure Conditions

The function is already fully planned: a yes to a final-planning invitation is refused with an explanation.

## Postconditions

The invitation is answered and planning or availability is updated.
