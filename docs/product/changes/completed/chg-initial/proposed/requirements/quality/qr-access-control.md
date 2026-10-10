---
id: QR-ACCESS-CONTROL
type: quality-requirement
title: "Only authorized users act on records"
status: draft
quality-attribute: "security"
applies-to:
  - "JRN-BILL-FINISHED-PROJECT"
  - "JRN-SET-UP-INVOICING"
verification:
  - scenario: "An unauthenticated request for any record is refused"
  - scenario: "Audit events name the signed-in user"
uses-terms: []
---

## Requirement

Every access to product data MUST require an authenticated user whose role permits the action.

## Measurement

Automated tests per role. Not yet implemented: until then installations must not be exposed publicly.
