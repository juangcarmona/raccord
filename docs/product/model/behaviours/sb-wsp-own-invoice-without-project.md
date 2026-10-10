---
id: SB-WSP-OWN-INVOICE-WITHOUT-PROJECT
type: structured-behaviour
title: "Standalone invoice belongs to its creator"
status: draft
illustrates:
  - "BR-WSP-FULL-ROLE-STILL-RESTRICTABLE"
given:
  - "A user may edit own invoices only"
when: "The user opens an invoice they created that is not linked to any project"
then:
  - "The invoice counts as the user's own and the own-invoice level applies"
uses-terms:
  - "TERM-OWN-RECORD"
provenance:
  source: ".private/research/help-center/articles/22185539659666.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Defines ownership when no account manager exists.

## Boundaries

None.
