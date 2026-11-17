---
id: CHG-WSP-FIRST-ADMIN-BROWSER
type: product-change
title: First workspace administrator through the browser
status: draft
base-revision: '998ff32'
operations:
  add: []
  modify:
    - UC-WSP-CREATE-WORKSPACE
    - BR-WSP-NEW-USER-DEFAULT-ROLE
    - FR-WSP-013
    - TERM-PREDEFINED-ROLE
  remove: []
---

## Problem

The definition combines first-account creation with subsequent onboarding and states an unconditional Crew default, leaving the first administrator bootstrap ambiguous.

## Intended Product Outcome

An accessible first-run browser journey creates one first administrator with email and password. Four predefined roles are available. Once a user exists, repeat setup is refused without creating another account. Later users keep the Crew default.

## Rationale

Separate first-account bootstrap from subsequent company onboarding without removing its company and financial requirements. The owner reconciliation states: "first-account creation succeeds before company name, company language and phone capture. Those details are outside this first-account screen and must not be introduced as new prerequisites." It further states: "Reconcile the first-Admin exception to BR-WSP-NEW-USER-DEFAULT-ROLE while preserving Crew for subsequent users."

## Affected Product Areas

First-run setup, administrator creation, predefined roles and new-user defaults.

## Open Questions

None.

## Product Acceptance

A fresh installation offers accessible browser setup; successful bootstrap creates the first Admin and makes all four roles available. An existing user makes bootstrap unavailable and a repeat attempt creates no account. Later users default to Crew. Company, sample-data, language, time-zone and financial obligations remain applicable to their later operations.

## Out of Scope

Company-profile UI, sample-data implementation, language or time-zone preferences, invitations, SSO, production data or credentials, delivery and technical design. The wider setup journey and sample-data removal requirement remain unchanged.
