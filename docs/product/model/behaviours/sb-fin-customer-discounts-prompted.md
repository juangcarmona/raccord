---
id: SB-FIN-CUSTOMER-DISCOUNTS-PROMPTED
type: structured-behaviour
title: "Selecting a client proposes its discounts"
status: draft
illustrates:
  - "BR-FIN-CUSTOMER-DEFAULTS-ON-SELECTION"
given:
  - "A customer has a 5% crew discount"
when: "The coordinator selects the customer as client and confirms the proposal"
then:
  - "The project crew category discount becomes 5%"
uses-terms:
  - "TERM-CUSTOMER-FINANCIAL-DEFAULTS"
provenance:
  source: ".private/research/help-center/articles/360013886600.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Defaults require confirmation.

## Boundaries

Declining leaves discounts unchanged.
