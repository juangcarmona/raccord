---
id: FR-CUSTOMER-001
type: functional-requirement
title: "Register customers and show invoice readiness"
status: active
derived-from:
  - "UC-REGISTER-CUSTOMER"
  - "BR-TAX-ID-NORMALIZATION"
verification:
  - scenario: "A customer registered with only a legal name is listed as not ready to invoice"
  - scenario: "A tax identifier entered as \" a-00000000 \" is shown as A00000000"
uses-terms:
  - "TERM-CUSTOMER"
  - "TERM-TAX-ID"
---

## Requirement

The product MUST allow registering a customer with only a legal name, MUST normalize tax identifiers, and MUST show whether each customer has the details required to be invoiced.

## Rationale

Customers are often known before their tax details.
