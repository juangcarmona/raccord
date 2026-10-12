---
id: UC-REGISTER-CUSTOMER
type: use-case
title: "Register a customer"
status: draft
primary-actor: "ACT-BILLING-ADMINISTRATOR"
supporting-actors:
  - "ACT-OPERATIONS-COORDINATOR"
bounded-context: "BC-CONTACTS"
governed-by:
  - "BR-TAX-ID-NORMALIZATION"
uses-terms:
  - "TERM-CUSTOMER"
---

## Goal

A customer exists to which projects and invoices can belong.

## Trigger

A new customer commissions work.

## Preconditions

None.

## Main Flow

1. The administrator enters the legal name and, when known, tax identifier, billing address and billing e-mail.
2. The product saves the customer and shows whether it is ready to be invoiced.

## Alternative Flows

Details are unknown: the customer is registered with the legal name only and shown as not yet invoiceable.

## Failure Conditions

A missing legal name is refused.

## Postconditions

The customer can be chosen for projects.
