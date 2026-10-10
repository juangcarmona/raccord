---
id: "FR-TRB-007"
type: functional-requirement
title: "Searchable planning lists"
status: draft
derived-from:
  - "BR-TRB-SEARCH-MINIMUM-THREE-CHARACTERS"
  - "BR-TRB-SEARCH-INCLUDES-SHOWN-COLUMNS"
  - "BR-TRB-PLANNED-LINE-NAME-EDITABLE"
  - "BR-TRB-IN-PROJECT-SEARCH-HIGHLIGHT"
verification:
  - scenario-ref: "SB-TRB-TWO-CHARACTER-SEARCH"
  - scenario-ref: "SB-TRB-SEARCH-BY-ADDED-COLUMN"
  - scenario-ref: "SB-TRB-RENAMED-LINE-SEARCH"
  - scenario-ref: "SB-TRB-STEP-THROUGH-MATCHES"
uses-terms:
  - "TERM-PLANNED-EQUIPMENT"
provenance:
  source: ".private/research/help-center/articles/360015031799.md, .private/research/help-center/articles/360015032959.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST search planning lists from three typed characters on, across names and shown columns, MUST match planned lines by their current name, and MUST highlight all matches in a project with navigation between them.

## Rationale

Planners must find resources quickly in large catalogs and projects.
