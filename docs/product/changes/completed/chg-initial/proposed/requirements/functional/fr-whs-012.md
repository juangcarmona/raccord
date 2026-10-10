---
id: FR-WHS-012
type: functional-requirement
title: "Quick availability check"
status: draft
derived-from:
  - "UC-WHS-CHECK-AVAILABILITY"
verification:
  - scenario: "Typing 'pio', choosing a mixer and 1 to 3 August shows 2 available"
uses-terms:
  - "TERM-AVAILABILITY-FORECAST"
provenance:
  source: ".private/research/help-center/articles/360013158280.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST offer, from anywhere, an availability check by article and period, returning the free quantity, negative when short.

## Rationale

Answer customers instantly.
