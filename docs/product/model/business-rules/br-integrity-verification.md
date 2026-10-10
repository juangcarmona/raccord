---
id: BR-INTEGRITY-VERIFICATION
type: business-rule
title: "Altered issued invoices are never served"
status: draft
applies-to:
  - "UC-OBTAIN-INVOICE-DOCUMENT"
  - "UC-ISSUE-INVOICE"
uses-terms:
  - "TERM-INVOICE-FINGERPRINT"
---

## Rule

Whenever an issued invoice is shown or rendered, its fingerprint is verified; if the stored invoice no longer matches its fingerprint it is not shown, a critical integrity failure is reported, and nothing is regenerated from it.

## Rationale

Detects alteration that bypassed the product, such as direct database edits.

## Examples

See SB-TAMPERED-INVOICE-REFUSED.

## Exceptions

None.
