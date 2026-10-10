---
id: QR-DETERMINISM
type: quality-requirement
title: "Issued invoices are reproducible"
status: draft
quality-attribute: "determinism"
applies-to:
  - "BC-INVOICING"
verification:
  - scenario: "The same issued invoice yields the same fingerprint and byte-identical document on any server, locale and time zone"
uses-terms: []
---

## Requirement

Calculations, issued content, fingerprints and documents MUST be deterministic.

## Measurement

Produce the representation and document twice and on two platforms; compare bytes. Threshold: identical.
