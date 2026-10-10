---
id: FR-WSP-012
type: functional-requirement
title: "Ownership-scoped access"
status: draft
derived-from:
  - "BR-WSP-FULL-ROLE-STILL-RESTRICTABLE"
  - "UC-WSP-MANAGE-ROLE"
  - "BR-WSP-PROJECT-ACCESS-IMPLICATIONS"
verification:
  - scenario-ref: "SB-WSP-OWN-INVOICE-WITHOUT-PROJECT"
  - scenario-ref: "SB-WSP-OWN-INVOICES-ONLY"
uses-terms:
  - "TERM-OWN-RECORD"
provenance:
  source: ".private/research/help-center/articles/22185539659666.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST distinguish own records (by account manager, owner or creator) from all records when applying ownership-scoped permissions.

## Rationale

Common need to limit users to their own work.
