---
id: FR-CRW-DEFAULT-FUNCTION-UPDATE
type: functional-requirement
title: "Update functions to defaults selectively"
status: draft
derived-from:
  - "UC-CRW-UPDATE-FUNCTIONS-TO-DEFAULTS"
  - "BR-CRW-DEFAULT-FUNCTION-COPY"
verification:
  - scenario-ref: "SB-CRW-UPDATE-TO-DEFAULT-SELECTED-FIELDS"
  - scenario-ref: "SB-CRW-DEFAULT-CHANGE-NOT-PROPAGATED"
uses-terms:
  - "TERM-DEFAULT-FUNCTION"
provenance:
  source: ".private/research/help-center/articles/34615607831314.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST keep project functions unchanged when defaults change and MUST let planners update chosen fields of chosen functions to the current defaults.

## Rationale

Stable quotes with controlled refresh.
