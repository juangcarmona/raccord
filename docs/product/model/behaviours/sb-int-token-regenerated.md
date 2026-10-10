---
id: SB-INT-TOKEN-REGENERATED
type: structured-behaviour
title: "Old token stops working after regeneration"
status: draft
illustrates:
  - "BR-INT-REGENERATION-REVOKES-TOKEN"
  - "UC-INT-REGENERATE-API-TOKEN"
given:
  - "an integration uses the current token"
when: "the administrator regenerates the token"
then:
  - "calls with the old token are refused"
  - "calls with the new token succeed"
uses-terms:
  - "TERM-API-TOKEN-VALIDITY"
provenance:
  source: ".private/research/help-center/articles/360013767839.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Regeneration is immediate revocation.

## Boundaries

None.
