---
id: UC-CRW-SET-UP-OWN-PROFILE
type: use-case
title: "Set up and maintain one's crew profile"
status: draft
primary-actor: "ACT-CREW-MEMBER"
supporting-actors: []
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-ACCOUNT-INVITATION-EXPIRY"
  - "BR-CRW-PROFILE-OWNERSHIP"
uses-terms:
  - "TERM-ACCOUNT-INVITATION-STATUS"
  - "TERM-PROFILE-OWNERSHIP"
provenance:
  source: ".private/research/help-center/articles/360013323900.md"
  confidence: "medium"
  recovered-from: "documentation"
---

## Goal

The crew member can log in and keeps their own details current.

## Trigger

The crew member receives an account invitation, or wants to update details.

## Preconditions

An account invitation is pending and not expired, or the person already has an account.

## Main Flow

1. The crew member opens the invitation and sets up their account.
2. If their profile is self-maintained, they enter personal and administrative details and a photo.
3. The product saves the details and the invitation becomes accepted.

## Alternative Flows

The profile is not self-maintained: personal details are read-only for the crew member.

## Failure Conditions

The invitation expired: the account cannot be set up until the planner resends it.

## Postconditions

The crew member can log in, see their schedule and work in the modules their role allows.
