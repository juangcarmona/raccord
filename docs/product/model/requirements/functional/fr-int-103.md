---
id: FR-INT-103
type: functional-requirement
title: "Fill in company details from a registry"
status: draft
derived-from:
  - "UC-INT-LOOK-UP-COMPANY-DETAILS"
verification:
  - id: "COMPANY-REGISTRY-LOOKUP"
    scenario: "Given a registry service available for the country, when the coordinator types a company name or registration number and picks a match, name, address and registration data fill the contact; with no match the details are typed manually, and fields missing from a result stay empty."
uses-terms:
  - "TERM-COMPANY-REGISTRY-LOOKUP"
provenance:
  source: ".private/research/help-center/articles/14042545177618.md, .private/research/help-center/articles/30759527977490.md, .private/research/help-center/articles/360015784860.md"
  confidence: "medium"
  recovered-from: "documentation"
---

## Requirement

The product MUST, when a registry or search service is available for the country, let a coordinator search by company name or registration number while creating a contact, list matching companies and fill the contact's name, address and registration data from the chosen one. The product MUST leave fields empty for manual entry when a result is incomplete and MUST allow fully manual entry when nothing matches.

## Rationale

Correct legal names and addresses matter on invoices, and typing them is slow and error-prone.
