---
id: BR-CRW-CREW-MEMBER-MINIMUM-DATA
type: business-rule
title: "A crew member needs a first name, a user role and an email"
status: draft
applies-to:
  - "UC-CRW-ADD-CREW-MEMBER"
uses-terms:
  - "TERM-CREW-MEMBER"
provenance:
  source: ".private/research/help-center/articles/360013323900.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

A crew member record can only be saved with at least a first name, a user role and an email address; the email is the address the person uses to log in.

## Rationale

The role decides what the person may do and the email is needed to invite and identify them.

## Examples

Adding a technician with only a last name is refused until a first name, role and email are given.

## Exceptions

None.
