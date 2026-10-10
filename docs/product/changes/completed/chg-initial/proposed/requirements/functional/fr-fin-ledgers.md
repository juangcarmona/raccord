---
id: FR-FIN-LEDGERS
type: functional-requirement
title: "Book revenue and costs to ledger accounts"
status: draft
derived-from:
  - "UC-FIN-MANAGE-LEDGERS"
  - "BR-FIN-DEFAULT-LEDGER-BY-KIND"
verification:
  - scenario-ref: "SB-FIN-PROJECT-LEDGER-OVERRIDE"
uses-terms:
  - "TERM-LEDGER-ACCOUNT"
provenance:
  source: ".private/research/help-center/articles/360013878320.md, .private/research/help-center/articles/33280698329618.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST assign ledgers to items by kind with item and project overrides.

## Rationale

Accounting needs categorised amounts.
