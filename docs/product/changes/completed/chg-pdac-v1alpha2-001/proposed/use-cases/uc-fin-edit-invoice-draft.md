---
id: UC-FIN-EDIT-INVOICE-DRAFT
type: use-case
title: "Edit and preview an invoice draft"
status: draft
primary-actor: "ACT-BILLING-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-INVOICING"
governed-by:
  - "BR-FIN-DRAFT-PREVIEW-MARK"
  - "BR-FIN-PAYMENT-TERM-PRECEDENCE"
  - "BR-ISSUED-INVOICE-IMMUTABLE"
uses-terms:
  - "TERM-INVOICE-PREVIEW"
  - "TERM-INVOICE-STATUS"
provenance:
  source: ".private/research/help-center/articles/360013965120.md, .private/research/help-center/articles/6819935599378.md, .private/research/help-center/articles/360013887460.md, .private/research/help-center/articles/360014214060.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The draft is correct before it is finalized.

## Trigger

Something in the draft must change.

## Preconditions

The invoice is not finalized.

## Main Flow

1. The administrator edits the draft: due date, payment term, VAT scheme, text or settings.
2. The administrator regenerates; the product shows the preview-marked document.

## Alternative Flows

The administrator deletes the draft (see UC-REVISE-INVOICE-DRAFT).

## Failure Conditions

The invoice is finalized: editing is refused; a credit note is needed.

## Postconditions

The draft reflects the changes.
