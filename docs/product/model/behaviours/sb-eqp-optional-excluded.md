---
id: SB-EQP-OPTIONAL-EXCLUDED
type: structured-behaviour
title: "Optional line excluded from quote total"
status: draft
illustrates:
  - "BR-WHS-OPTIONAL-EXCLUDED-FROM-TOTAL"
given:
  - "A project has a line marked optional priced 200"
when: "The quote is generated"
then:
  - "The optional line is listed"
  - "Its price is not part of the quote total"
uses-terms:
  - "TERM-OPTIONAL-EQUIPMENT"
provenance:
  source: ".private/research/help-center/articles/360017699899.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Options do not inflate the offer.

## Boundaries

None.
