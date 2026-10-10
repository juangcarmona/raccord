---
id: "TERM-DOCUMENT-TYPE"
type: domain-term
title: "Document type"
status: draft
defined-in: "BC-DOCUMENTS"
synonyms: []
uses-terms:
  - "TERM-INVOICE-DOCUMENT"
  - "TERM-PACKING-SLIP"
  - "TERM-QUOTE"
  - "TERM-CONTRACT"
  - "TERM-PAYMENT-REMINDER"
provenance:
  source: ".private/research/help-center/articles/360013503420.md, .private/research/help-center/articles/23413689614226.md, .private/research/help-center/articles/360017645780.md, .private/research/help-center/articles/31719450942354.md, .private/research/help-center/articles/29923797058194.md"
  confidence: high
  recovered-from: documentation
---

## Definition

The kind of document a template produces. It decides where in the product the template can be used, what content it carries, and which letterheads may be chosen.

| Document type | Produced from |
| --- | --- |
| Quotation, contract, invoice | the financial side of a project |
| Packing slip (also used for call sheets) | a project |
| Equipment slip (equipment labels and tags) | catalogue articles |
| Combination slip | a kit or case |
| Vehicle slip | a vehicle |
| Reminder | an overdue invoice |
| Crew member slip | a crew member |

Quotation, contract and invoice are the financial document types; the others are operational.

## Distinguish From

Template: the type is the classification, the template is the layout of that type. Invoice document (TERM-INVOICE-DOCUMENT): the rendering of one issued invoice, which is an instance of the invoice type.

## Usage

Filters which template is offered when a document is generated from a record.
