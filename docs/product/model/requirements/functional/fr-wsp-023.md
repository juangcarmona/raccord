---
id: FR-WSP-023
type: functional-requirement
title: "Identifier-matched imports"
status: draft
derived-from:
  - "UC-WSP-UPDATE-BY-IMPORT"
  - "BR-WSP-RECORD-IDENTIFIER-IMMUTABLE"
  - "BR-WSP-EMPTY-IDENTIFIER-CREATES"
verification:
  - scenario-ref: "SB-WSP-IMPORT-EMPTY-ID"
  - scenario-ref: "SB-WSP-IMPORT-CLEARED-ID"
uses-terms:
  - "TERM-RECORD-IDENTIFIER"
provenance:
  source: ".private/research/help-center/articles/36084108749842.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST include immutable record identifiers in exports and, on import, update the record matching each row's identifier and create rows without identifier as new records.

## Rationale

Reliable mass updates.
