---
id: FR-CRW-RESOURCE-IMPORT
type: functional-requirement
title: "Import crew and vehicles"
status: draft
derived-from:
  - "UC-CRW-IMPORT-CREW-MEMBERS"
  - "UC-CRW-IMPORT-VEHICLES"
  - "BR-CRW-IMPORT-MATCH-COLUMN"
  - "BR-CRW-IMPORT-FOLDER-PLACEMENT"
  - "BR-CRW-IMPORT-TAG"
  - "BR-CRW-IMPORT-ROW-RESULT"
  - "BR-CRW-IMPORT-EXCLUSIONS"
verification:
  - scenario-ref: "SB-CRW-IMPORT-DEFAULT-ROLE"
  - scenario-ref: "SB-CRW-IMPORT-NO-FOLDER-COLUMN"
  - scenario-ref: "SB-CRW-IMPORT-UNDO-BY-TAG"
uses-terms:
  - "TERM-RESOURCE-IMPORT"
provenance:
  source: ".private/research/help-center/articles/360014366299.md, .private/research/help-center/articles/360014307939.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST import crew members and vehicles in create, update and skip-existing modes with column mapping, match columns, root folder placement, preview, import tagging and a per-row result.

## Rationale

Fast onboarding.
