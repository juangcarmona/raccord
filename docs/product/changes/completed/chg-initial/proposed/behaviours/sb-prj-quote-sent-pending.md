---
id: SB-PRJ-QUOTE-SENT-PENDING
type: structured-behaviour
title: "Sending a quote makes a draft project pending"
status: draft
illustrates:
  - "BR-PRJ-QUOTE-SENT-PENDING"
given:
  - "A project is in Draft"
when: "The coordinator sends a quotation to the customer"
then:
  - "The project status becomes Pending"
uses-terms:
  - "TERM-PROJECT-STATUS-PENDING"
provenance:
  source: ".private/research/help-center/articles/360013587379.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Automatic transition on sending.

## Boundaries

None.
