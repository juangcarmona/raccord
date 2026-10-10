---
id: FR-PRJ-032
type: functional-requirement
title: "Contact database"
status: draft
derived-from:
  - "UC-PRJ-ADD-CONTACT"
  - "UC-PRJ-ADD-CONTACT-PERSON"
  - "BR-PRJ-ADDRESS-ROLES"
  - "BR-PRJ-INVOICE-RECIPIENT"
  - "BR-PRJ-TRAVEL-ESTIMATE-REFRESH"
  - "BR-PRJ-CONTACT-NUMBER-ASSIGNMENT"
verification:
  - scenario-ref: "SB-PRJ-BILLING-ADDRESS-ON-INVOICE"
  - scenario-ref: "SB-PRJ-INVOICE-TO-ADMINISTRATIVE-CONTACT"
  - scenario-ref: "SB-PRJ-INVOICE-FALLBACK-DEFAULT-CONTACT"
uses-terms:
  - "TERM-CONTACT"
  - "TERM-CONTACT-PERSON"
provenance:
  source: ".private/research/help-center/articles/360014254000.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST keep contacts (company or private individual) with folder, number, visiting, mailing and billing addresses, travel estimate, warning, automatic project note and contact persons with default and administrative designations.

## Rationale

Parties of all projects.
