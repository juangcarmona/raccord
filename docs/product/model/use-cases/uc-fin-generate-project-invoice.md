---
id: UC-FIN-GENERATE-PROJECT-INVOICE
type: use-case
title: "Generate an invoice for a project"
status: draft
primary-actor: "ACT-BILLING-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-INVOICING"
governed-by:
  - "BR-FIN-INVOICE-SCOPE"
  - "BR-FIN-INVOICE-DEDUCTION"
  - "BR-FIN-EXTRA-LINE-SIGN"
  - "BR-FIN-DUE-DATE-FROM-TERM"
  - "BR-FIN-PAYMENT-TERM-PRECEDENCE"
  - "BR-FIN-MOMENT-PERCENTAGES"
  - "BR-FIN-INVOICE-PERMISSION"
uses-terms:
  - "TERM-PROJECT-INVOICE"
  - "TERM-PARTIAL-INVOICE"
  - "TERM-INVOICE-EXTRA-LINE"
  - "TERM-INVOICE-DEDUCTION"
  - "TERM-INVOICE-DRAFT"
provenance:
  source: ".private/research/help-center/articles/360013965120.md, .private/research/help-center/articles/360017648580.md, .private/research/help-center/articles/360017648200.md, .private/research/help-center/articles/360017700379.md, .private/research/help-center/articles/23969868679058.md"
  confidence: high
  recovered-from: documentation
---

## Goal

An invoice draft for all or part of a project exists with correct amounts, terms and due date.

## Trigger

An invoice moment is due or the project must be billed.

## Preconditions

The project has a priced financial overview; the user has financial access.

## Main Flow

1. The administrator starts an invoice from the queue or the project.
2. The product proposes client, number series, date, payment term, due date, 100% of the project (or the share of the due invoice moment) and the VAT scheme.
3. The administrator optionally enters a percentage, adds extra lines and selects earlier invoices to subtract.
4. The administrator adjusts document text (optionally saved as reusable text), template, letterhead and settings.
5. The product computes the amounts and generates the invoice as a draft with a preview document.

## Alternative Flows

Skipping the options dialog for future invoices. Changing the invoice moment of the project beforehand. The draft is finalized as in UC-ISSUE-INVOICE.

## Failure Conditions

User without financial access: refused.

## Postconditions

An invoice draft exists for the project.
