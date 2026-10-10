---
id: UC-REVISE-INVOICE-DRAFT
type: use-case
title: "Revise or discard an invoice draft"
status: draft
primary-actor: "ACT-BILLING-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-INVOICING"
governed-by:
  - "BR-SINGLE-BILLING"
  - "BR-ISSUED-INVOICE-IMMUTABLE"
  - "BR-AUDITED-ACTIONS"
uses-terms:
  - "TERM-INVOICE-DRAFT"
---

## Goal

A draft contains exactly the work to be invoiced now.

## Trigger

Some drafted work belongs to a later invoice, or the draft is no longer wanted.

## Preconditions

The draft is open.

## Main Flow

1. The administrator removes a line from the draft.
2. The product returns its billable line to unbilled and recalculates the draft.
3. The removal is audited.

## Alternative Flows

Discard: the administrator discards the draft; all its lines become unbilled and the discard is audited.

## Failure Conditions

Revising or discarding an issued draft is refused.

## Postconditions

Removed lines are available for another draft.
