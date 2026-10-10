---
id: TERM-PAYMENT-STATUS
type: domain-term
title: "Payment status"
status: draft
defined-in: "BC-INVOICING"
synonyms: []
uses-terms:
  - "TERM-OUTSTANDING-AMOUNT"
  - "TERM-OVERDUE-INVOICE"
provenance:
  source: ".private/research/help-center/articles/18560784878610.md, .private/research/help-center/articles/360017700379.md, .private/research/help-center/articles/18536074243474.md, .private/research/help-center/articles/360013977700.md, .private/research/help-center/articles/360013492560.md"
  confidence: medium
  recovered-from: documentation
---

## Definition

Lifecycle of payment for an issued invoice.

| State | Meaning |
| --- | --- |
| Unpaid | No payment recorded. |
| Partially paid | Payments recorded, outstanding amount above zero. |
| Paid | Outstanding amount is zero. |

Overdue (TERM-OVERDUE-INVOICE) qualifies an unpaid or partially paid invoice past its due date. Transitions: unpaid -> partially paid (partial payment); unpaid or partially paid -> paid (payment fulfilling the outstanding amount); removing or correcting a payment moves back accordingly.

## Distinguish From

Invoice status (draft/issued); deposit status.

## Usage

Shown in invoice lists with totals paid for a selection.
