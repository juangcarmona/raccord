---
id: FR-CRW-HOUR-TYPES
type: functional-requirement
title: "Configurable hour types"
status: draft
derived-from:
  - "UC-CRW-CONFIGURE-HOUR-TYPES"
  - "BR-CRW-APPROVAL-ROUTING"
verification:
  - scenario: "A custom training hour type with approval required is offered when registering hours and its registrations start pending"
  - scenario: "Hours registered with a type that needs no approval are recorded directly"
uses-terms:
  - "TERM-HOUR-TYPE"
provenance:
  source: ".private/research/help-center/articles/24569529468562.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST provide default hour types and custom hour types with name, colour, payroll code and approval requirement.

## Rationale

Company-specific policy for how worked time is classified, approved and paid.
