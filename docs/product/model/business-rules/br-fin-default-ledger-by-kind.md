---
id: BR-FIN-DEFAULT-LEDGER-BY-KIND
type: business-rule
title: "Ledger accounts default by kind and can be overridden locally"
status: draft
applies-to:
  - "UC-FIN-MANAGE-LEDGERS"
uses-terms:
  - "TERM-LEDGER-ACCOUNT"
provenance:
  source: ".private/research/help-center/articles/360013878320.md, .private/research/help-center/articles/360013971080.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Equipment, functions and additional costs receive a default ledger according to their kind (rental, sale, crew, transport, insurance, other, default); a ledger set on the item applies everywhere, and one set inside a project applies to that project only.

## Rationale

Revenue must be split by accounting category without manual work.

## Examples

See SB-FIN-PROJECT-LEDGER-OVERRIDE.

## Exceptions

None.
