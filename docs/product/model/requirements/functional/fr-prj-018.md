---
id: FR-PRJ-018
type: functional-requirement
title: "Manage project templates"
status: draft
derived-from:
  - "UC-PRJ-CREATE-PROJECT-TEMPLATE"
  - "BR-PRJ-TEMPLATE-CONTENT"
  - "BR-PRJ-TEMPLATE-NOT-EDITABLE"
  - "BR-PRJ-TEMPLATE-INDEPENDENT"
  - "BR-PRJ-TEMPLATE-TASKS"
  - "BR-PRJ-ARCHIVED-TEMPLATE-ITEM"
verification:
  - scenario-ref: "SB-PRJ-TEMPLATE-SKIPS-ASSIGNMENTS"
  - scenario: "Deleting a template's source project leaves the template usable"
uses-terms:
  - "TERM-PROJECT-TEMPLATE"
provenance:
  source: ".private/research/help-center/articles/360013367659.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST create templates by copying a project's reusable content, keep them independent of the source, and recreate template tasks with relative deadlines on new projects.

## Rationale

Faster set-up of recurring work.
