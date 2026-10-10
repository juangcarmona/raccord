---
id: QR-WSP-PASSWORD-CHECK-PRIVACY
type: quality-requirement
title: "Breach check never discloses passwords"
status: draft
quality-attribute: "privacy"
applies-to:
  - "UC-WSP-SIGN-IN"
  - "UC-WSP-MANAGE-PASSWORD"
verification:
  - scenario-ref: "SB-WSP-BREACHED-PASSWORD"
  - scenario: "Inspecting outgoing traffic of a breach check shows only a short hash prefix and never the password or its full hash"
uses-terms:
  - "TERM-BREACHED-PASSWORD-WARNING"
provenance:
  source: ".private/research/help-center/articles/31560381502226.md"
  confidence: medium
  recovered-from: documentation
---

## Requirement

The breached-password check MUST NOT transmit the password or any value from which it can be derived outside the product.

## Measurement

Traffic inspection of the check: only a hash prefix of limited length leaves the product.
