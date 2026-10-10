---
id: SB-PRJ-CLIENT-PAYMENT-TERM-OVERRIDES
type: structured-behaviour
title: "Client payment term overrides the project"
status: draft
illustrates:
  - "BR-PRJ-CLIENT-COMMERCIAL-OVERRIDE"
given:
  - "A project type defaults to 30-day payment term"
  - "Contact Acme has a 14-day payment term"
when: "Acme is chosen as client of a project of that type"
then:
  - "The project's payment term is 14 days"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/360014254000.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Customer agreements prevail.

## Boundaries

None.
