---
id: QR-ACCESSIBILITY
type: quality-requirement
title: "Accessible user interface"
status: draft
quality-attribute: "accessibility"
applies-to:
  - "JRN-BILL-FINISHED-PROJECT"
  - "JRN-SET-UP-INVOICING"
verification:
  - scenario: "Every control in the billing journey can be reached and operated with a keyboard and is announced with a meaningful name by a screen reader"
uses-terms: []
---

## Requirement

The user interface MUST meet WCAG 2.2 level AA.

## Measurement

Automated accessibility linting on every change plus a manual audit with assistive technology before the first release.
