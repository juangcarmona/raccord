---
id: FR-FIN-CREW-EXPENSES
type: functional-requirement
title: "Manage crew expenses"
status: draft
derived-from:
  - "UC-FIN-MANAGE-CREW-EXPENSES"
  - "BR-FIN-CREW-EXPENSE-INHERITANCE"
  - "BR-FIN-CREW-EXPENSE-CATEGORIES"
verification:
  - scenario-ref: "SB-FIN-CREW-EXPENSE-OVERRIDE"
uses-terms:
  - "TERM-CREW-EXPENSE"
provenance:
  source: ".private/research/help-center/articles/31405085792146.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST support cost and price of four fixed crew expense categories on default functions, planned functions and planned crew members with local overrides.

## Rationale

Crew-related costs are significant and often re-invoiced.
