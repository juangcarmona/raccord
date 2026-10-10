---
id: BR-FIN-LEDGER-IMPORT-CREATES
type: business-rule
title: "Importing equipment with an unknown ledger creates it"
status: draft
applies-to:
  - "UC-FIN-MANAGE-LEDGERS"
uses-terms:
  - "TERM-LEDGER-ACCOUNT"
provenance:
  source: ".private/research/help-center/articles/360013878320.md"
  confidence: high
  recovered-from: documentation
---

## Rule

When equipment is imported with a ledger name that does not exist, a new ledger is created and assigned to that equipment.

## Rationale

Smooth migration from existing equipment lists.

## Examples

Importing items with ledger "Staging" creates that ledger.

## Exceptions

None.
