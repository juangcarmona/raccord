---
id: "TERM-GENERATED-DOCUMENT"
type: domain-term
title: "Generated document"
status: draft
defined-in: "BC-DOCUMENTS"
synonyms: []
uses-terms:
  - "TERM-DOCUMENT-TEMPLATE"
  - "TERM-LETTERHEAD"
  - "TERM-PERSONAL-TEXT"
  - "TERM-ATTACHED-FILE"
  - "TERM-OUTGOING-EMAIL"
  - "TERM-INVOICE-DOCUMENT"
provenance:
  source: ".private/research/help-center/articles/360014055700.md, .private/research/help-center/articles/360017645460.md, .private/research/help-center/articles/360013979460.md, .private/research/help-center/articles/360017700539.md, .private/research/help-center/articles/360014260839.md, .private/research/help-center/articles/360017647860.md, .private/research/help-center/articles/360014193840.md"
  confidence: high
  recovered-from: documentation
---

## Definition

A document produced by applying a document template to a source record (project, invoice, vehicle, crew member, articles). Generation inputs:
- template and letterhead (proposed from defaults, changeable);
- a personal text (TERM-PERSONAL-TEXT);
- whether the content of combinations is shown or hidden, overriding the combination default;
- for equipment slips, the articles to include.
The result can be previewed, regenerated with other inputs, downloaded, saved to the record's files (TERM-ATTACHED-FILE), e-mailed (TERM-OUTGOING-EMAIL) and, when the document type allows it, signed digitally.

## Distinguish From

Invoice document (TERM-INVOICE-DOCUMENT): the fixed rendering of an issued invoice in the invoicing context. Attached file: the stored copy once a generated document is saved.

## Usage

Produced from many places in the product; it is a snapshot that does not follow later changes to its source until regenerated.
