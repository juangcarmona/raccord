---
id: SB-FIN-PROJECT-LEDGER-OVERRIDE
type: structured-behaviour
title: "Ledger changed inside one project"
status: draft
illustrates:
  - "BR-FIN-DEFAULT-LEDGER-BY-KIND"
given:
  - "A rental item uses the rental ledger by default"
when: "The coordinator assigns ledger \"Events\" to the item within project P"
then:
  - "Project P books the item on \"Events\""
  - "Other projects book it on \"Rental\""
uses-terms:
  - "TERM-LEDGER-ACCOUNT"
provenance:
  source: ".private/research/help-center/articles/360013878320.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Local overrides.

## Boundaries

None.
