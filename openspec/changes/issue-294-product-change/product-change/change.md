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
unaffected:
  - id: "FR-WSP-025"
    cause: "UC-WSP-CREATE-WORKSPACE"
    relationship: "derived-from"
    digest: "sha256:700640fa49deb96f6923e40e642c68d62503d6e8db712e16c49aad40fbcd64c7"
    cause-digest: "sha256:bad3a5a4eae32e934a6ace45458a66c14eb33fbcfa02c2c1c22c7c3b922cc0fe"
    reason: "Later company onboarding still supplies sample data and administrators still remove it before production use; first-account bootstrap does not implement or remove sample-data behavior."
  - id: "UC-WSP-ASSIGN-ROLE"
    cause: "BR-WSP-NEW-USER-DEFAULT-ROLE"
    relationship: "governed-by"
    digest: "sha256:7225266488941b418540e36431ed75cb596efdb9cddef5109aa52f65a9db9b37"
    cause-digest: "sha256:225d068645b0edea78a38030b719e72c188c78fcb75337160dde005db87e6c9c"
    reason: "An existing administrator still assigns roles to later crew members and later new users still default to Crew; the first-account Admin exception does not change this role-assignment operation."
  - id: "JRN-WSP-SET-UP-WORKSPACE"
    cause: "UC-WSP-CREATE-WORKSPACE"
    relationship: "steps[].use-case"
    digest: "sha256:e5a03909059bff96da07389ff0214635e74d4085fa6aab2b46c48cba4b88ccc9"
    cause-digest: "sha256:bad3a5a4eae32e934a6ace45458a66c14eb33fbcfa02c2c1c22c7c3b922cc0fe"
    reason: "The full installation-to-go-live journey and all later company, language, time-zone, numbering, sample-data and role configuration steps remain unchanged; only its existing creation use case distinguishes account bootstrap from subsequent onboarding."
  - id: "SB-WSP-FREELANCER-APP-SCOPE"
    cause: "TERM-PREDEFINED-ROLE"
    relationship: "uses-terms"
    digest: "sha256:baae06c1fc5770e90852dc0aa173126c84e2c26c2b4d256a5375c423bf8fce13"
    cause-digest: "sha256:3e6f316bdfdb4e2c76eede9cbcf2db751b3806adad7f5397c15d27ffdff7fe16"
    reason: "This scenario still starts with a person holding Crew and retains planned-project-only access and hidden finances; assigning Admin to the initial account does not change Crew permissions or channel behavior."
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
