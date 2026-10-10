---
id: CON-PRJ-FIXED-PROJECT-STATUSES
type: constraint
title: "Project statuses are not configurable"
status: draft
applies-to:
  - "BC-PROJECTS"
uses-terms:
  - "TERM-PROJECT-STATUS"
provenance:
  source: ".private/research/help-center/articles/360013587379.md"
  confidence: high
  recovered-from: documentation
---

## Constraint

The project status set is fixed by the product; the company cannot add, rename or remove project statuses.

## Rationale

Deliberate: many areas (warehouse, invoicing, availability, reporting) depend on the meaning of each status.

## Consequences

Company-specific stages must be expressed with tags, custom fields or warehouse handling statuses.
