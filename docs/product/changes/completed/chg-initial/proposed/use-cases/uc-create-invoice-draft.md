---
id: UC-CREATE-INVOICE-DRAFT
type: use-case
title: "Create an invoice draft from project work"
status: draft
primary-actor: "ACT-BILLING-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-INVOICING"
governed-by:
  - "BR-SINGLE-BILLING"
  - "BR-LINE-ORDER"
  - "BR-VAT-PER-RATE"
  - "BR-AUDITED-ACTIONS"
uses-terms:
  - "TERM-INVOICE-DRAFT"
  - "TERM-VALIDATION-ISSUE"
---

## Goal

Selected unbilled work is gathered into one draft with calculated totals.

## Trigger

Work is ready to be invoiced.

## Preconditions

The project has unbilled lines.

## Main Flow

1. The administrator selects unbilled lines of one project.
2. The product reserves them, copies them into a new draft in project order, in the default or chosen series, and calculates the VAT breakdown and totals.
3. The product shows any validation issue that would prevent issuing today.
4. The creation is audited.

## Alternative Flows

A chosen series does not exist: the draft is not created.

## Failure Conditions

A line that is not unbilled, or was reserved concurrently by another draft, prevents creation; no line is reserved.

## Postconditions

The selected lines are in draft and the draft is open.
