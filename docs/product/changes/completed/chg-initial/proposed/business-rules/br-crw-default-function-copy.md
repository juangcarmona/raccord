---
id: BR-CRW-DEFAULT-FUNCTION-COPY
type: business-rule
title: "Project functions are copies of default functions"
status: draft
applies-to:
  - "UC-CRW-UPDATE-FUNCTIONS-TO-DEFAULTS"
  - "UC-CRW-ADD-FUNCTION-TO-PROJECT"
uses-terms:
  - "TERM-DEFAULT-FUNCTION"
provenance:
  source: ".private/research/help-center/articles/34615607831314.md, .private/research/help-center/articles/14232209807506.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

Adding a default function to a project copies its values; later changes to the default do not reach existing project functions (including those in duplicated projects or project templates) until the planner explicitly updates selected functions and fields to the default.

## Rationale

Quoted projects must stay stable.

## Examples

See SB-CRW-UPDATE-TO-DEFAULT-SELECTED-FIELDS.

## Exceptions

None.
