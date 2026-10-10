---
id: FR-WHS-031
type: functional-requirement
title: "Early returns and delays"
status: draft
derived-from:
  - "BR-WHS-EARLY-RETURN-RELEASES"
  - "BR-WHS-DELAY-EXTENDS-PLANNING-ONLY"
  - "BR-WHS-BOOK-OUT-TAKES-RESERVATION"
  - "UC-WHS-MARK-DELAYED"
verification:
  - scenario-ref: "SB-WHS-EARLY-RETURN-RELEASES"
  - scenario-ref: "SB-WHS-MARK-DELAYED-EXTENDS"
  - scenario-ref: "SB-WHS-BOOK-OUT-STEALS-RESERVATION"
uses-terms:
  - "TERM-DELAYED-EQUIPMENT"
  - "TERM-EARLY-RETURN"
provenance:
  source: ".private/research/help-center/articles/360012699859.md, .private/research/help-center/articles/23280754011154.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST release reservations on early returns, extend planning periods of equipment groups marked delayed to the expected return moment without changing usage, and move reservations when reserved units are booked out elsewhere.

## Rationale

Truthful availability.
