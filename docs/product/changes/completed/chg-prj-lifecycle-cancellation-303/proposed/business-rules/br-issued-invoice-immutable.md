---
id: BR-ISSUED-INVOICE-IMMUTABLE
type: business-rule
title: "Issued invoices never change"
status: active
applies-to:
  - "BC-INVOICING"
uses-terms:
  - "TERM-ISSUED-INVOICE"
  - "TERM-RECTIFYING-INVOICE"
---

## Rule

An issued invoice, and the draft it came from, can never be changed, issued again or deleted by any means the product offers; corrections are new documents that reference it.

## Rationale

Spanish rules require integrity and inalterability of invoicing records.

## Examples

Discarding an issued draft is refused.

## Exceptions

None.
