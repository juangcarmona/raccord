---
id: SB-INT-TOKEN-EXPIRES
type: structured-behaviour
title: "Token expires after ten years"
status: draft
illustrates:
  - "BR-INT-TOKEN-VALIDITY-PERIOD"
given:
  - "a token was created on 2026-03-01"
when: "a call presents the token on 2036-03-02"
then:
  - "the call is refused as expired"
  - "the administrator must create a new token"
uses-terms:
  - "TERM-API-TOKEN-VALIDITY"
provenance:
  source: ".private/research/help-center/articles/360013767839.md, .private/research/help-center/articles/30759527977490.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Validity period is ten years for new tokens.

## Boundaries

None.
