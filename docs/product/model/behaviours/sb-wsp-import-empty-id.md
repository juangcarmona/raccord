---
id: SB-WSP-IMPORT-EMPTY-ID
type: structured-behaviour
title: "New rows in an exported file are created"
status: draft
illustrates:
  - "BR-WSP-EMPTY-IDENTIFIER-CREATES"
given:
  - "The user exported 500 items with identifiers"
  - "The user added 100 rows without identifier"
when: "The user imports the file matched on record identifiers"
then:
  - "The 500 existing items are updated"
  - "100 new items are created"
uses-terms:
  - "TERM-RECORD-IDENTIFIER"
provenance:
  source: ".private/research/help-center/articles/36084108749842.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Mixed update and create in one import.

## Boundaries

None.
