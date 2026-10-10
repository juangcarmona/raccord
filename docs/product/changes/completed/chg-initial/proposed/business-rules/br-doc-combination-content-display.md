---
id: "BR-DOC-COMBINATION-CONTENT-DISPLAY"
type: business-rule
title: "Combination content follows its default unless overridden at generation"
status: draft
applies-to:
  - "UC-DOC-GENERATE-DOCUMENT"
  - "UC-DOC-HIDE-ITEMS-ON-FINANCIAL-DOCUMENTS"
uses-terms:
  - "TERM-GENERATED-DOCUMENT"
  - "TERM-PHYSICAL-KIT"
  - "TERM-DEFAULT-CONTENT"
provenance:
  source: ".private/research/help-center/articles/360017647860.md, .private/research/help-center/articles/360017647940.md"
  confidence: high
  recovered-from: documentation
---

## Rule

The content of a kit or case is shown or hidden on financial documents according to the combination's default; when generating a document the user may override this and show or hide all combination content for that document.

## Rationale

Kits are priced as a whole; their parts matter to the warehouse more than to the customer.

## Examples

See SB-DOC-COMBINATION-CONTENT-OVERRIDE.

## Exceptions

None.
