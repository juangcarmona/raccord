---
id: TERM-PROJECT-INVOICE
type: domain-term
title: "Project invoice"
status: draft
defined-in: "BC-INVOICING"
synonyms: []
uses-terms:
  - "TERM-PROJECT"
  - "TERM-PARTIAL-INVOICE"
  - "TERM-INVOICE-EXTRA-LINE"
  - "TERM-INVOICE-DEDUCTION"
provenance:
  source: ".private/research/help-center/articles/360013965120.md, .private/research/help-center/articles/360017648580.md, .private/research/help-center/articles/360017648200.md"
  confidence: high
  recovered-from: documentation
---

## Definition

An invoice generated from a project whose content is computed from the project's current prices: the percentage of the project sum, extra invoice lines, deductions of earlier invoices, invoice moment, payment term and due date, VAT scheme, document text, template and letterhead.

## Distinguish From

Standalone invoice (no project); invoice draft built from billable lines (TERM-INVOICE-DRAFT) in the first slice.

## Usage

Generated from the invoicing queue or the project.
