---
id: FR-PRJ-012
type: functional-requirement
title: "Delete projects with safeguards"
status: draft
derived-from:
  - "UC-PRJ-DELETE-PROJECT"
  - "BR-PRJ-DELETE-BOOKED-WARNING"
  - "BR-PRJ-DELETION-PERMANENT"
verification:
  - scenario: "Deleting a project with booked-out equipment shows a warning first"
uses-terms:
  - "TERM-PROJECT"
provenance:
  source: ".private/research/help-center/articles/360013587379.md, .private/research/help-center/articles/360013367659.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST warn before deleting a project that has equipment booked out and MUST treat deletion as permanent.

## Rationale

Prevents losing track of equipment.
