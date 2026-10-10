---
id: FR-INT-004
type: functional-requirement
title: "Version and deprecate the interface with transitions"
status: draft
derived-from:
  - "UC-INT-ADAPT-TO-API-CHANGE"
  - "BR-INT-DEPRECATION-TRANSITION"
verification:
  - scenario-ref: SB-INT-FIELD-TRANSITION
uses-terms:
  - "TERM-API-VERSION"
  - "TERM-API-DEPRECATION"
provenance:
  source: ".private/research/help-center/articles/26707946497042.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST publish numbered interface versions with notes and a machine-readable specification, and MUST serve moved fields in both places until the announced end of their transition period.

## Rationale

Integrations can evolve without sudden breakage.
