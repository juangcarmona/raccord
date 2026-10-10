---
id: FR-INVOICE-002
type: functional-requirement
title: "Revise and discard drafts"
status: draft
derived-from:
  - "UC-REVISE-INVOICE-DRAFT"
  - "BR-SINGLE-BILLING"
verification:
  - scenario: "After removing a line from a draft and discarding the draft, every one of its lines is unbilled again"
uses-terms:
  - "TERM-INVOICE-DRAFT"
---

## Requirement

The product MUST let the billing administrator remove lines from an open draft or discard it, returning the affected lines to unbilled.

## Rationale

Partial billing and mistakes must not lose work.
