---
id: UC-WSP-JOIN-WORKSPACE
type: use-case
title: "Join after being invited"
status: draft
primary-actor: "ACT-CREW-MEMBER"
supporting-actors: []
bounded-context: "BC-WORKSPACE"
governed-by:
  - "BR-WSP-BREACHED-PASSWORD-CHECK"
  - "BR-CRW-ACCOUNT-INVITATION-EXPIRY"
uses-terms:
  - "TERM-ACCOUNT-INVITATION-STATUS"
  - "TERM-PERSONAL-PROFILE"
  - "TERM-WORKSPACE"
provenance:
  source: ".private/research/help-center/articles/360018767240.md, .private/research/help-center/articles/360014204180.md, .private/research/help-center/articles/360018840939.md"
  confidence: high
  recovered-from: documentation
---

## Goal

A newly added crew member can sign in and see their planning, tasks and projects.

## Trigger

The person receives the invitation email after an administrator or office user added them with login allowed.

## Preconditions

- The account invitation is pending.

## Main Flow

1. The person opens the invitation and confirms their email address.
2. The person chooses a strong password, or an external sign-in.
3. The product creates the personal profile and opens the application.
4. The person can add the mobile web app to their phone and sign in with the same credentials.

## Alternative Flows

- The person already has a profile with that address: they sign in as usual.

## Failure Conditions

- The invitation expired: the planner must resend it.

## Postconditions

- The person can sign in to the workspace; the invitation is accepted.
