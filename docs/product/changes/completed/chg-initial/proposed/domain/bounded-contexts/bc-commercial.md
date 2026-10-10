---
id: BC-COMMERCIAL
type: bounded-context
title: "Commercial"
status: draft
---

## Responsibility

Turning a planned job into an offer the customer accepts: pricing, discounts, quotes, contracts, signatures and requests.

## Language

Prices, price factors, discounts, quote and contract versions, validity, acceptance.

## Boundaries

Issuing invoices and payments are in Invoicing.

## External Relationships

Reads the project plan; its accepted documents confirm projects.
