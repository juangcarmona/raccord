---
id: SB-FIN-UNSYNCED-VERSION-HIDDEN
type: structured-behaviour
title: "New version not synchronised stays private"
status: draft
illustrates:
  - "BR-FIN-ONLINE-VERSION-VISIBILITY"
given:
  - "Version 1 of a quote is published"
when: "The coordinator creates a new version without synchronising"
then:
  - "The customer still sees version 1 through the link"
  - "Synchronising shows version 2 to the customer"
uses-terms:
  - "TERM-ONLINE-QUOTE-STATUS"
provenance:
  source: ".private/research/help-center/articles/29481848461458.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Preview before sharing.

## Boundaries

None.
