---
id: SB-WSP-OWN-INVOICES-ONLY
type: structured-behaviour
title: "Office user restricted to own invoices"
status: draft
illustrates:
  - "BR-WSP-FULL-ROLE-STILL-RESTRICTABLE"
given:
  - "An account manager's office-type role allows view, edit and delete on own invoices and view only on other invoices"
when: "The account manager tries to change an invoice of a project managed by a colleague"
then:
  - "The invoice can be viewed but not changed"
uses-terms:
  - "TERM-OWN-RECORD"
provenance:
  source: ".private/research/help-center/articles/360012611160.md, .private/research/help-center/articles/22185539659666.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Broad roles can be fenced to their own records.

## Boundaries

None.
