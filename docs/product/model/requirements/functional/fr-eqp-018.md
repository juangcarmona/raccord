---
id: FR-EQP-018
type: functional-requirement
title: "Compute kit weight and transport volume"
status: draft
derived-from:
  - "BR-EQP-KIT-WEIGHT"
  - "BR-EQP-TRANSPORT-VOLUME-PACKED-PER"
verification:
  - scenario-ref: "SB-EQP-KIT-WEIGHT"
  - scenario-ref: "SB-EQP-PACKED-PER-VOLUME"
uses-terms:
  - "TERM-EMPTY-WEIGHT"
  - "TERM-PACKED-PER"
provenance:
  source: ".private/research/help-center/articles/13141562920338.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST compute physical kit weight as empty weight plus default content weight and the transport volume of a planned quantity using the packed-per quantity rounded up to whole packages.

## Rationale

Transport planning needs realistic totals.
