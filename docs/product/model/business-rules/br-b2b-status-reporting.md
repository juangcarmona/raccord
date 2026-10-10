---
id: BR-B2B-STATUS-REPORTING
type: business-rule
title: "Received B2B invoices need a status within four days"
status: draft
applies-to:
  - "UC-EXCHANGE-B2B-E-INVOICE"
uses-terms: []
---

## Rule

For every B2B e-invoice the company receives, Raccord asks the billing administrator to accept or reject it and to report full payment with its date, and warns when a status is due within the legal period of four natural days excluding weekends and national holidays.

## Rationale

CON-B2B-E-INVOICE-REGULATION.

## Examples

An invoice received on a Monday must be accepted or rejected by Friday of the same week.

## Exceptions

None.
