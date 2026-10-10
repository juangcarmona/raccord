---
id: BR-FIN-DISCOUNT-GROUP-PERCENTAGE
type: business-rule
title: "Discount group percentage of the client applies to items in the group"
status: draft
applies-to:
  - "UC-FIN-CONFIGURE-DISCOUNT-GROUPS"
  - "UC-FIN-APPLY-CUSTOMER-DEFAULTS"
uses-terms:
  - "TERM-DISCOUNT-GROUP"
provenance:
  source: ".private/research/help-center/articles/360013890780.md"
  confidence: high
  recovered-from: documentation
---

## Rule

When a customer is the client of a project, every equipment item that belongs to a discount group receives as line discount the percentage that customer has for that group.

## Rationale

Negotiated conditions differ per customer and per kind of equipment.

## Examples

See SB-FIN-DISCOUNT-GROUP-PER-CUSTOMER.

## Exceptions

Items in no discount group receive no group discount.
