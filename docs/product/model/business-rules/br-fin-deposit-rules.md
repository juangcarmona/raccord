---
id: BR-FIN-DEPOSIT-RULES
type: business-rule
title: "A refundable deposit is a fixed amount with a tracked status"
status: draft
applies-to:
  - "UC-FIN-MANAGE-REFUNDABLE-DEPOSIT"
uses-terms:
  - "TERM-REFUNDABLE-DEPOSIT"
  - "TERM-DEPOSIT-STATUS"
  - "TERM-CREDIT-NOTE"
provenance:
  source: ".private/research/help-center/articles/360013264720.md"
  confidence: high
  recovered-from: documentation
---

## Rule

A refundable deposit is always a fixed amount; once set it carries a deposit status starting at not paid; it is not printed on documents unless the template includes it, and returning it to the customer is documented with a credit note.

## Rationale

A deposit is security, not revenue.

## Examples

A 500.00 deposit is marked paid when received and returned after the gear comes back.

## Exceptions

None.
