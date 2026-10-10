---
id: FR-PRJ-010
type: functional-requirement
title: "Apply status effects across areas"
status: draft
derived-from:
  - "BR-PRJ-STATUS-AREA-VISIBILITY"
  - "BR-WHS-STATUS-INDEPENDENCE"
  - "BR-PRJ-WAREHOUSE-STATUS-PROMPT"
verification:
  - scenario-ref: "SB-PRJ-DRAFT-NOT-IN-WAREHOUSE"
  - scenario-ref: "SB-PRJ-CANCELLED-WITH-CREW-IN-PLANNER"
  - scenario-ref: "SB-PRJ-ALL-ITEMS-MOVED-PROMPT"
  - scenario-ref: "SB-PRJ-ITEM-LEFT-BEHIND-NO-PROMPT"
  - scenario-ref: "SB-PRJ-EARLY-RETURN-PARTIAL"
uses-terms:
  - "TERM-PROJECT-STATUS"
provenance:
  source: ".private/research/help-center/articles/360013587379.md, .private/research/help-center/articles/23361953107346.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST show projects in warehouse handling, invoicing and crew planning according to their status, MUST keep equipment statuses independent from project status, and MUST prompt for a project status change when all equipment reaches the next stage.

## Rationale

Each area acts on the right projects.
