---
id: "FR-TRB-012"
type: functional-requirement
title: "Password self-service safeguards"
status: draft
derived-from:
  - "BR-TRB-RESET-NEEDS-UNIQUE-EMAIL"
  - "BR-TRB-PASSWORD-CHANGE-NEEDS-CURRENT"
verification:
  - scenario-ref: "SB-TRB-RESET-DUPLICATE-EMAIL"
  - scenario-ref: "SB-TRB-PASSWORD-CHANGE-WRONG-CURRENT"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/360014048499.md"
  confidence: medium
  recovered-from: documentation
---

## Requirement

The product MUST require the current password to change a password and MUST complete e-mail password resets only for addresses that identify a single user.

## Rationale

Protects accounts against takeover and ambiguous resets.
