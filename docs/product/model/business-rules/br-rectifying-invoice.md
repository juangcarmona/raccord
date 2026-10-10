---
id: BR-RECTIFYING-INVOICE
type: business-rule
title: "Corrections are Spanish rectifying invoices"
status: draft
applies-to:
  - "UC-CORRECT-ISSUED-INVOICE"
uses-terms:
  - "TERM-RECTIFYING-INVOICE"
  - "TERM-INVOICE-SERIES"
  - "TERM-ISSUED-INVOICE"
---

## Rule

An issued invoice is corrected only by a rectifying invoice that references it, is numbered in the rectifying series (R), states the reason and uses one of two methods: by differences (only the change, positive or negative) or by substitution (cancels the original amounts and states the corrected ones). Its total may be negative. The original invoice is never changed.

## Rationale

Spanish invoicing regulation (RD 1619/2012 art. 15) and the VERI*FACTU record types require rectifying invoices in their own series.

## Examples

See SB-RECTIFY-BY-DIFFERENCES and SB-RECTIFY-BY-SUBSTITUTION.

## Exceptions

None.
