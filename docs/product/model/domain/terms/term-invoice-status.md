---
id: TERM-INVOICE-STATUS
type: domain-term
title: "Invoice status"
status: draft
defined-in: "BC-INVOICING"
synonyms: []
uses-terms:
  - "TERM-INVOICE-DRAFT"
  - "TERM-ISSUED-INVOICE"
  - "TERM-PAYMENT-STATUS"
provenance:
  source: ".private/research/help-center/articles/6819935599378.md, .private/research/help-center/articles/360013965120.md"
  confidence: high
  recovered-from: documentation
---

## Definition

Lifecycle of an invoice document.

| State | Meaning |
| --- | --- |
| Draft (preview) | Generated but not final: editable, duplicable, deletable; its document carries a preview mark and it cannot be exported. |
| Issued (finalized) | Numbered and immutable; can be e-mailed, downloaded, exported and paid. |

Transitions: draft -> issued (finalize, see UC-ISSUE-INVOICE); draft -> removed (discard, see UC-REVISE-INVOICE-DRAFT). No transition leaves the issued state; corrections are credit notes. Payment progress is tracked separately in TERM-PAYMENT-STATUS.

## Distinguish From

Payment status.

## Usage

Shown with every invoice.
