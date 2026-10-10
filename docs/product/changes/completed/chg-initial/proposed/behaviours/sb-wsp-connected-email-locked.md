---
id: SB-WSP-CONNECTED-EMAIL-LOCKED
type: structured-behaviour
title: "Administrator cannot edit a connected user's email"
status: draft
illustrates:
  - "BR-WSP-PROFILE-EMAIL-SELF-SERVICE"
given:
  - "A crew member has a verified login"
when: "An administrator tries to change that crew member's login email"
then:
  - "The administrator cannot change it"
  - "The person can change it from their own profile"
uses-terms:
  - "TERM-USER-ACCOUNT"
provenance:
  source: ".private/research/help-center/articles/360017645240.md, .private/research/help-center/articles/360014997360.md"
  confidence: high
  recovered-from: documentation
---

## Intent

The verified identity is owned by the person.

## Boundaries

None.
