---
id: FR-EQP-002
type: functional-requirement
title: "Lock equipment type after first planning"
status: draft
derived-from:
  - "BR-EQP-TYPE-FIXED-ONCE-PLANNED"
  - "UC-EQP-CHANGE-ITEM-TYPE"
verification:
  - scenario-ref: "SB-EQP-TYPE-CHANGE-REFUSED"
  - scenario-ref: "SB-EQP-TYPE-CHANGE-ALLOWED"
uses-terms:
  - "TERM-EQUIPMENT-TYPE"
provenance:
  source: ".private/research/help-center/articles/360013718240.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST refuse to change the type of an equipment item that has been planned on any project or project template, and MUST allow it otherwise.

## Rationale

Protects existing projects and documents.
