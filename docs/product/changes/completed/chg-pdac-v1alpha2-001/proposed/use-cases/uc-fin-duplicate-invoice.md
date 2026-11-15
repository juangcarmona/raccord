---
id: UC-FIN-DUPLICATE-INVOICE
type: use-case
title: "Duplicate an invoice"
status: draft
primary-actor: "ACT-BILLING-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-INVOICING"
governed-by:
  - "BR-FIN-DUPLICATE-RECOMPUTES"
uses-terms:
  - "TERM-PROJECT-INVOICE"
provenance:
  source: ".private/research/help-center/articles/360013965120.md, .private/research/help-center/articles/6819935599378.md"
  confidence: high
  recovered-from: documentation
---

## Goal

A similar invoice is prepared quickly with the same configuration.

## Trigger

A project is invoiced repeatedly in the same way.

## Preconditions

An invoice exists in the project.

## Main Flow

1. The administrator duplicates the invoice.
2. The product copies document text and settings and computes the content from current project data.

## Alternative Flows

None.

## Failure Conditions

- Duplicating from an invoice that no longer exists in the project is refused with the reason and nothing is created.
- The copy takes its content from current project data, not from the original, so its amounts can differ (BR-FIN-DUPLICATE-RECOMPUTES).
- An actor without permission to create invoices is refused and nothing changes.

## Postconditions

A new invoice draft exists.
