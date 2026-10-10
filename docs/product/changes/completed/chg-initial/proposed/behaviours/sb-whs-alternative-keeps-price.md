---
id: SB-WHS-ALTERNATIVE-KEEPS-PRICE
type: structured-behaviour
title: "Replacing short units by an alternative at the original price"
status: draft
illustrates:
  - "BR-WHS-ALTERNATIVE-REPLACEMENT"
  - "UC-WHS-SOLVE-WITH-ALTERNATIVE"
given:
  - "A line plans 6 speakers at 50 per unit with a shortage of 2"
  - "Speaker model B is an alternative priced 60"
when: "The coordinator replaces only the short units by B keeping the original price"
then:
  - "The original line has 4 units"
  - "A line of 2 units of B is priced 50 per unit"
  - "Neither line is short"
uses-terms:
  - "TERM-EQUIPMENT-ALTERNATIVE"
provenance:
  source: ".private/research/help-center/articles/16611399161234.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Price policy is explicit on replacement.

## Boundaries

None.
