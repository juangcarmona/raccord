---
id: SB-WSP-IMPORT-CLEARED-ID
type: structured-behaviour
title: "Accidentally cleared identifier creates a duplicate"
status: draft
illustrates:
  - "BR-WSP-EMPTY-IDENTIFIER-CREATES"
given:
  - "The identifier of an existing item's row was deleted in the spreadsheet"
when: "The user imports the file"
then:
  - "A new item is created for that row"
  - "The original item remains unchanged"
uses-terms:
  - "TERM-RECORD-IDENTIFIER"
provenance:
  source: ".private/research/help-center/articles/36084108749842.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Shows the risk of clearing identifiers.

## Boundaries

None.
