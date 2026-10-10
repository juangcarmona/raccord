---
id: TERM-DEPOSIT-STATUS
type: domain-term
title: "Deposit status"
status: draft
defined-in: "BC-COMMERCIAL"
synonyms: []
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/360013264720.md"
  confidence: medium
  recovered-from: documentation
---

## Definition

Lifecycle of a refundable deposit.

| State | Meaning |
| --- | --- |
| Not paid | A deposit amount is set but the customer has not paid it (initial state once an amount exists). |
| Paid | The customer has paid the deposit. |
| Returned | The deposit has been given back. |

Transitions: not paid -> paid -> returned; the status is set manually and can be corrected in either direction. Without an amount there is no status.

## Distinguish From

Payment status of an invoice.

## Usage

Optional variable on quote and invoice templates.
