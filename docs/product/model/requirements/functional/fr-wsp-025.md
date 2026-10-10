---
id: FR-WSP-025
type: functional-requirement
title: "First set-up with removable sample data"
status: draft
derived-from:
  - "UC-WSP-CREATE-WORKSPACE"
  - "UC-WSP-EMPTY-SAMPLE-DATA"
  - "BR-WSP-EMPTY-DATA-SCOPE"
  - "BR-WSP-EMPTY-BEFORE-PRODUCTION"
verification:
  - scenario-ref: "SB-WSP-EMPTY-KEEPS-CREW"
uses-terms:
  - "TERM-SAMPLE-DATA"
provenance:
  source: ".private/research/help-center/articles/360015603179.md, .private/research/help-center/articles/360013982380.md"
  confidence: medium
  recovered-from: documentation
---

## Requirement

The product MUST set up a new installation with sample data and let administrators remove it in one irreversible operation before production use, keeping crew, roles, settings and document templates.

## Rationale

Fast exploration and clean start.
