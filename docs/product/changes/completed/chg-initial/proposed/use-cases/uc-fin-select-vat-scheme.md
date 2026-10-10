---
id: UC-FIN-SELECT-VAT-SCHEME
type: use-case
title: "Choose the VAT scheme of a project or invoice"
status: draft
primary-actor: "ACT-BILLING-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-INVOICING"
governed-by:
  - "BR-FIN-VAT-FROM-CLASS-AND-SCHEME"
  - "BR-FIN-VAT-SCHEME-PRECEDENCE"
  - "BR-ISSUED-INVOICE-IMMUTABLE"
uses-terms:
  - "TERM-VAT-SCHEME"
  - "TERM-REVERSE-CHARGE"
provenance:
  source: ".private/research/help-center/articles/360013264720.md, .private/research/help-center/articles/360013887460.md, .private/research/help-center/articles/360017648360.md"
  confidence: high
  recovered-from: documentation
---

## Goal

A project or invoice is taxed according to the customer's situation.

## Trigger

The customer needs a different taxation, such as reverse charge for a foreign business.

## Preconditions

VAT schemes exist.

## Main Flow

1. The administrator selects a VAT scheme in the project financial overview (default: the scheme of the configuration or of the client).
2. The product recalculates VAT of every line from the line's VAT class and the scheme.

## Alternative Flows

For one invoice only: the administrator edits the invoice draft, selects a scheme and regenerates the invoice so the scheme is applied.

## Failure Conditions

The invoice is issued: its VAT can no longer change.

## Postconditions

Documents generated afterwards use the selected scheme.
