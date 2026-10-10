---
id: BR-INT-ROLE-CHANGE-NEEDS-NEW-SESSION
type: business-rule
title: "Role permission changes apply on session renewal"
status: draft
applies-to:
  - "BC-WORKSPACE"
uses-terms:
  - "TERM-USER-ROLE"
provenance:
  source: ".private/research/help-center/articles/35485155508370.md"
  confidence: medium
  recovered-from: documentation
---

## Rule

A change to a role's permissions takes effect for a signed-in user after a page refresh, and reliably after signing out and in again.

## Rationale

Permissions are loaded with the session.

## Examples

After the permission is granted, the option appears only once the user signs in again.

## Exceptions

None.
