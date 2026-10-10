---
id: QR-LOCALIZATION
type: quality-requirement
title: "Spanish and English interface"
status: draft
quality-attribute: "localizability"
applies-to:
  - "JRN-BILL-FINISHED-PROJECT"
  - "JRN-SET-UP-INVOICING"
verification:
  - scenario: "Switching the interface to English shows English labels and validation messages, and dates, amounts and VAT rates in English formats"
  - scenario: "A calendar date such as 2026-05-01 is shown as the same day in every viewer time zone"
uses-terms: []
---

## Requirement

The interface MUST be translatable, ship at least in Spanish and English, let each workspace and each person choose their language, format dates, money and percentages per locale, and never shift calendar dates across time zones.

## Measurement

Automated tests of formatting; review of message catalogs for completeness (every message in both languages).
