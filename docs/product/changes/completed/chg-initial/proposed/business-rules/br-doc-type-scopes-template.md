---
id: "BR-DOC-TYPE-SCOPES-TEMPLATE"
type: business-rule
title: "A template's document type decides where it is used and what it contains"
status: draft
applies-to:
  - "UC-DOC-CONFIGURE-TEMPLATE-SETTINGS"
  - "UC-DOC-GENERATE-DOCUMENT"
uses-terms:
  - "TERM-DOCUMENT-TYPE"
  - "TERM-DOCUMENT-TEMPLATE"
provenance:
  source: ".private/research/help-center/articles/360013503420.md, .private/research/help-center/articles/360017645780.md, .private/research/help-center/articles/360014232559.md, .private/research/help-center/articles/17707866444178.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A document template is offered only when generating documents of its own document type, and its content is the data defined for that type.

## Rationale

Each kind of source record carries different data; a template must only use data its source provides.

## Examples

- A packing slip template is not offered when generating an invoice.
- Equipment slips list equipment; crew member slips list crew planning.

## Exceptions

None.
