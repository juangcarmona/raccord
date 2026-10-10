---
id: UC-WSP-ASSIGN-ROLE
type: use-case
title: "Assign a role to a crew member"
status: draft
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-WORKSPACE"
governed-by:
  - "BR-WSP-ONE-ROLE-PER-USER"
  - "BR-WSP-NEW-USER-DEFAULT-ROLE"
  - "BR-WSP-SSO-COMPLIANCE"
  - "BR-CRW-LOGIN-PERMISSION"
uses-terms:
  - "TERM-USER-ROLE"
  - "TERM-USER-ACCOUNT"
  - "TERM-CREW-MEMBER"
provenance:
  source: ".private/research/help-center/articles/360014360700.md, .private/research/help-center/articles/360012611160.md, .private/research/help-center/articles/360015898239.md, .private/research/help-center/articles/360017697219.md"
  confidence: high
  recovered-from: documentation
---

## Goal

A crew member has the access appropriate to their job.

## Trigger

A crew member is added or their responsibilities change.

## Preconditions

- Roles are configured.
- The administrator may change user roles.

## Main Flow

1. The administrator opens or adds the crew member.
2. The administrator selects a role on the login information and saves.
3. For a new user, the login details are communicated (the account invitation is sent); a new user holds the Crew role until another is chosen.

## Alternative Flows

- Handing over administration: the administrator gives another person the Admin role and changes their own role in the same step.

## Failure Conditions

- The role enforces single sign-on and the person is not compliant: the assignment is refused or the person cannot sign in.

## Postconditions

- The user acts with the new role's permissions.
