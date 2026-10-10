---
id: FR-ISSUER-001
type: functional-requirement
title: "Maintain the issuer profile"
status: draft
derived-from:
  - "UC-DEFINE-ISSUER-PROFILE"
  - "BR-PARTY-SNAPSHOT"
verification:
  - scenario: "The issuer profile is saved with name, tax identifier and complete address and is shown on the next issued invoice"
  - scenario: "An invoice issued before the profile changed still shows the previous details"
uses-terms:
  - "TERM-ISSUER"
---

## Requirement

The product MUST let the billing administrator define and update the issuer's legal name, tax identifier and complete postal address, and MUST apply changes only to invoices issued afterwards.

## Rationale

Invoices must identify the issuer as it was at issue time.
