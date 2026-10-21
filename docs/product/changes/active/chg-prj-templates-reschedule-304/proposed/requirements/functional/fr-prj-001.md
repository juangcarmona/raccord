---
id: FR-PRJ-001
type: functional-requirement
title: "Create projects blank, from templates or by duplication"
status: active
derived-from:
  - "UC-CREATE-PROJECT"
  - "UC-PRJ-CREATE-FROM-TEMPLATE"
  - "UC-PRJ-DUPLICATE-PROJECT"
  - "BR-PRJ-SCHEDULE-REQUIRED"
  - "BR-PRJ-TEMPLATE-START-ANCHOR"
verification:
  - scenario-ref: "SB-PRJ-TEMPLATE-ANCHORED-SCHEDULE"
  - scenario-ref: "SB-PRJ-TEMPLATE-SKIPS-ASSIGNMENTS"
  - scenario: "A new regular project without periods cannot be saved"
uses-terms:
  - "TERM-PROJECT-TEMPLATE"
  - "TERM-TIME-SCHEDULE"
provenance:
  source: ".private/research/help-center/articles/360013590720.md, .private/research/help-center/articles/360013367659.md, .private/research/help-center/articles/29017857097618.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let coordinators create projects blank, from a template anchored at a chosen start date, or by duplicating a project to a new start date, and MUST refuse to save a project that requires a schedule until its periods exist.

## Rationale

Projects are the base of all planning.
