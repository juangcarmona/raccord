---
id: FR-PRJ-017
type: functional-requirement
title: "Manage project types"
status: active
derived-from:
  - "UC-PRJ-MANAGE-PROJECT-TYPES"
  - "BR-PRJ-PROJECT-TYPE-DEFAULTS"
  - "BR-PRJ-REMEMBER-LAST-TEMPLATE"
verification:
  - scenario: "A project of type 'Sale' receives the type's invoice template and series by default"
uses-terms:
  - "TERM-PROJECT-TYPE"
  - "TERM-PROJECT-CATEGORY"
provenance:
  source: ".private/research/help-center/articles/360013631800.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let administrators define project types with category, colour, document template defaults, letterhead, number series, invoice moment and additional conditions, and apply them to projects.

## Rationale

Right settings per kind of job.
