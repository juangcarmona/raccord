---
id: JRN-SET-UP-INVOICING
type: journey
title: "Set up invoicing"
status: draft
primary-actor: "ACT-BILLING-ADMINISTRATOR"
steps:
  - use-case: "UC-DEFINE-ISSUER-PROFILE"
  - use-case: "UC-REGISTER-CUSTOMER"
---

## Intended Outcome

The installation can issue invoices for its customers.

## Entry Conditions

Raccord is installed.

## Journey Narrative

The billing administrator enters the issuer's legal details once and registers customers as they appear, completing tax details before the first invoice.

## Variants and Branches

Customers registered without tax details are flagged until completed.

## Completion Conditions

The issuer profile exists and at least one customer is invoiceable.
