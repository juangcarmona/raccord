---
id: SB-INT-TOKEN-ROLE-LIMIT
type: structured-behaviour
title: "A token cannot read finance its owner cannot see"
status: draft
illustrates:
  - "BR-INT-TOKEN-INHERITS-OWNER-ROLE"
given:
  - "a token was created by a user whose role has no financial visibility"
when: "the external application requests the invoice collection"
then:
  - "the request is refused for lack of permission"
  - "no invoice data is returned"
uses-terms:
  - "TERM-API-TOKEN"
provenance:
  source: ".private/research/help-center/articles/360013767839.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Shows that tokens inherit the creator's role.

## Boundaries

Does not define the exact refusal code.
