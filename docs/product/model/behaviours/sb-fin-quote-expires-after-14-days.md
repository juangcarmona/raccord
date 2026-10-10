---
id: SB-FIN-QUOTE-EXPIRES-AFTER-14-DAYS
type: structured-behaviour
title: "Default validity of 14 days"
status: draft
illustrates:
  - "BR-FIN-QUOTE-VALIDITY-DEFAULT"
given:
  - "No custom default validity is configured"
when: "A quote is generated on 1 March"
then:
  - "Its expiration date is 15 March"
  - "From 16 March the project matches the expired-quotes filter while unsigned"
uses-terms:
  - "TERM-QUOTE-VALIDITY"
  - "TERM-EXPIRED-QUOTE"
provenance:
  source: ".private/research/help-center/articles/360012594659.md"
  confidence: medium
  recovered-from: documentation
---

## Intent

Default validity.

## Boundaries

Exact day counting convention is assumed.
