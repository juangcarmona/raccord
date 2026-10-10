---
id: FR-PRJ-026
type: functional-requirement
title: "Sell task work as services"
status: draft
derived-from:
  - "UC-PRJ-MANAGE-SERVICES"
  - "BR-PRJ-SERVICE-BREAKDOWN"
  - "BR-PRJ-TASK-SINGLE-SERVICE"
  - "BR-PRJ-DEFAULT-SERVICE-COPY"
  - "BR-PRJ-SERVICE-DOCUMENT-SECTION"
verification:
  - scenario-ref: "SB-PRJ-SERVICE-BREAKDOWN-FIGURES"
  - scenario-ref: "SB-PRJ-TASK-SECOND-SERVICE-REFUSED"
uses-terms:
  - "TERM-PRODUCTION-SERVICE"
provenance:
  source: ".private/research/help-center/articles/38423504883858.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST support project services and default services with price, cost rate, VAT, ledgers and linked tasks, compute their breakdown, and print them in their own section of the built-in document templates.

## Rationale

Production work becomes revenue.
