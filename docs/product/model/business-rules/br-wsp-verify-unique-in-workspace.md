---
id: BR-WSP-VERIFY-UNIQUE-IN-WORKSPACE
type: business-rule
title: "Verification checks the address is unique"
status: draft
applies-to: []
uses-terms:
  - "TERM-USER-ACCOUNT"
provenance:
  source: ".private/research/help-center/articles/36290845440018.md, .private/research/help-center/articles/36290993235730.md"
  confidence: high
  recovered-from: documentation
---

## Rule

When a user starts email verification, the product checks that the entered address (typed twice to confirm) is not used by another user account before sending the link; duplicates are flagged to administrators.

## Rationale

Two people cannot share one verified identity.

## Examples

- Two crew members registered with the same address both receive warnings and must be corrected.

## Exceptions

None.
