---
id: SB-FIN-DUE-DATE-14-DAYS
type: structured-behaviour
title: "Due date from a 14-day term"
status: draft
illustrates:
  - "BR-FIN-DUE-DATE-FROM-TERM"
given:
  - "The customer's payment term is 14 days"
when: "An invoice dated 1 November is generated"
then:
  - "Its due date is 15 November"
uses-terms:
  - "TERM-DUE-DATE"
provenance:
  source: ".private/research/help-center/articles/360014214060.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Due date arithmetic.

## Boundaries

None.
