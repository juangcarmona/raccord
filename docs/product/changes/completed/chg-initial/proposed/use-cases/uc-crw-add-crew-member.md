---
id: UC-CRW-ADD-CREW-MEMBER
type: use-case
title: "Add a crew member"
status: draft
primary-actor: "ACT-CREW-PLANNER"
supporting-actors:
  - "ACT-CREW-MEMBER"
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-CREW-MEMBER-MINIMUM-DATA"
  - "BR-CRW-ACCOUNT-INVITATION-ON-CREATION"
  - "BR-CRW-AGENCY-MULTIPLE-BOOKING"
  - "BR-CRW-PLANNABLE-FLAG"
uses-terms:
  - "TERM-CREW-MEMBER"
  - "TERM-AGENCY-CREW-RECORD"
  - "TERM-ACCOUNT-INVITATION-STATUS"
  - "TERM-SKILL-TAG"
provenance:
  source: ".private/research/help-center/articles/360013323900.md, .private/research/help-center/articles/360014360520.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

A new person exists as crew and can be planned and invited to log in.

## Trigger

The company takes on a new employee or freelancer.

## Preconditions

The planner may manage crew members.

## Main Flow

1. The planner enters at least first name, user role and email.
2. The planner completes contact, address, description, folder, skill tags, planning flags and administrative data as known.
3. The product saves the crew member.
4. The product sends the person an account invitation.

## Alternative Flows

- An agency record is created and flagged as plannable several times at once; it is then used for hired people.
- Login permission is left off; no invitation is sent (see UC-CRW-MANAGE-CREW-LOGIN-ACCESS).

## Failure Conditions

Missing first name, role or email: the record is not saved and the missing data is indicated.

## Postconditions

The crew member exists in its folder; the account invitation is pending.
