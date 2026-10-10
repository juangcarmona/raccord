---
id: SB-INT-RATE-LIMITED
type: structured-behaviour
title: "Polling beyond the limit is refused"
status: draft
illustrates:
  - "BR-INT-RATE-LIMIT-REFUSAL"
when: "an application exceeds the allowed request rate"
then:
  - "further requests receive a forbidden-type refusal"
  - "requests succeed again once the rate drops"
uses-terms:
  - "TERM-API-RATE-LIMIT"
provenance:
  source: ".private/research/help-center/articles/26707946497042.md"
  confidence: medium
  recovered-from: documentation
---

## Intent

Rate limits are enforced on all endpoints.

## Boundaries

The numeric limit is not documented in the source.
