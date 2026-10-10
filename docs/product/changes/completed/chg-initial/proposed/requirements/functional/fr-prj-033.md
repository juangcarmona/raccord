---
id: FR-PRJ-033
type: functional-requirement
title: "Contact commercial defaults"
status: draft
derived-from:
  - "UC-PRJ-SET-CONTACT-COMMERCIAL-DEFAULTS"
  - "BR-PRJ-CLIENT-COMMERCIAL-OVERRIDE"
verification:
  - scenario-ref: "SB-PRJ-CLIENT-PAYMENT-TERM-OVERRIDES"
uses-terms:
  - "TERM-CONTACT-COMMERCIAL-DEFAULTS"
provenance:
  source: ".private/research/help-center/articles/360014254000.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST store invoice moment, payment term, VAT scheme, insurance percentage and discount groups on contacts and apply them, overriding project values, when the contact is the client.

## Rationale

Agreed terms applied consistently.
