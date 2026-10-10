---
id: "BR-DOC-LETTERHEAD-TYPE-AVAILABILITY"
type: business-rule
title: "A letterhead is offered only for its document types"
status: draft
applies-to:
  - "UC-DOC-ADD-LETTERHEAD"
  - "UC-DOC-GENERATE-DOCUMENT"
uses-terms:
  - "TERM-LETTERHEAD"
provenance:
  source: ".private/research/help-center/articles/360014055700.md, .private/research/help-center/articles/360017648260.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A letterhead can be chosen for a generated document only when the document's type is among the types selected on the letterhead.

## Rationale

Label-sized or supplier-specific backgrounds must not be offered for unrelated documents.

## Examples

- A 3 x 3 cm label letterhead enabled only for equipment slips is not offered when generating a quotation.

## Exceptions

None.
