---
id: SB-EQP-UPDATE-MATCH-ON-NAME
type: structured-behaviour
title: "Duplicate names break matching"
status: draft
illustrates:
  - "BR-EQP-IMPORT-MATCH-UNIQUE"
given:
  - "Two catalog items share the same name"
when: "An update import matches on name"
then:
  - "The rows cannot be matched unambiguously"
  - "An error or a duplicate item results"
uses-terms:
  - "TERM-IMPORT-MATCH-COLUMN"
provenance:
  source: ".private/research/help-center/articles/360014293220.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Recommends unique keys such as record identifiers.

## Boundaries

None.
