---
id: "FR-DOC-010"
type: functional-requirement
title: "Generate documents with per-document choices"
status: draft
derived-from:
  - "UC-DOC-GENERATE-DOCUMENT"
  - "UC-DOC-REGENERATE-WITH-OTHER-LAYOUT"
  - "BR-DOC-LETTERHEAD-PRECEDENCE"
  - "BR-DOC-COMBINATION-CONTENT-DISPLAY"
verification:
  - scenario-ref: "SB-DOC-COMBINATION-CONTENT-OVERRIDE"
  - scenario-ref: "SB-DOC-REGENERATE-WITH-OTHER-LETTERHEAD"
uses-terms:
  - "TERM-GENERATED-DOCUMENT"
  - "TERM-PERSONAL-TEXT"
provenance:
  source: ".private/research/help-center/articles/360013503420.md, .private/research/help-center/articles/360014055700.md, .private/research/help-center/articles/360017645460.md, .private/research/help-center/articles/360017647860.md, .private/research/help-center/articles/360014260839.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST generate a document from a record using a proposed default template and letterhead that the user can change, with personal text and combination content override.

## Rationale

Defaults for speed, overrides for exceptions.
