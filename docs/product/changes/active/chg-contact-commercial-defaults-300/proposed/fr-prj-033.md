---
id: FR-PRJ-033
type: functional-requirement
title: "Contact commercial defaults initialize new projects"
status: active
derived-from:
  - "UC-PRJ-SET-CONTACT-COMMERCIAL-DEFAULTS"
  - "BR-PRJ-CLIENT-COMMERCIAL-OVERRIDE"
verification:
  - scenario-ref: "SB-PRJ-CLIENT-PAYMENT-TERM-OVERRIDES"
uses-terms:
  - "TERM-CONTACT-COMMERCIAL-DEFAULTS"
provenance:
  source: ".private/research/help-center/articles/360014254000.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let authorized users store the supported commercial defaults on a contact. When that contact is selected as the client of a new project, the product MUST copy each set default into the project's initial commercial settings. Later edits to contact defaults MUST NOT change existing or running projects.

## Rationale

Agreed terms initialize new work while each existing project retains the values it was created with.
