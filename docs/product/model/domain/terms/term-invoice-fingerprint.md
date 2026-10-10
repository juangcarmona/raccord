---
id: TERM-INVOICE-FINGERPRINT
type: domain-term
title: "Invoice fingerprint"
status: draft
defined-in: "BC-INVOICING"
synonyms: []
uses-terms:
  - "TERM-ISSUED-INVOICE"
---

## Definition

A cryptographic digest of the canonical representation of an issued invoice, shown with the invoice and recorded in the audit trail, that reveals any alteration.

## Distinguish From

The chained hash of a tax billing record, which follows its own regulated rules.

## Usage

Verified every time an issued invoice is shown or rendered.
