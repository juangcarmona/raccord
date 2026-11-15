---
id: UC-FIN-SEND-PAYMENT-REMINDER
type: use-case
title: "Send a payment reminder"
status: draft
primary-actor: "ACT-BILLING-ADMINISTRATOR"
supporting-actors:
  - "ACT-CUSTOMER"
bounded-context: "BC-INVOICING"
governed-by:
  - "BR-FIN-REMINDERS-MANUAL"
uses-terms:
  - "TERM-PAYMENT-REMINDER"
  - "TERM-OVERDUE-INVOICE"
provenance:
  source: ".private/research/help-center/articles/17195865170706.md, .private/research/help-center/articles/18560784878610.md"
  confidence: high
  recovered-from: documentation
---

## Goal

An overdue customer is reminded and given a new deadline.

## Trigger

An invoice is past its due date and unpaid.

## Preconditions

The invoice is overdue.

## Main Flow

1. The administrator creates a reminder for the invoice.
2. The administrator chooses reminder template and letterhead and optionally a new due date.
3. The product generates the reminder and updates the reminder count and date.
4. The administrator sends it.

## Alternative Flows

Second and third reminders follow the same flow.

## Failure Conditions

The invoice is not yet due: no reminder is offered.

## Postconditions

The reminder is recorded on the invoice.
