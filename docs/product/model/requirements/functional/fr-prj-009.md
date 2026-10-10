---
id: FR-PRJ-009
type: functional-requirement
title: "Maintain the project status lifecycle"
status: draft
derived-from:
  - "UC-PRJ-CHANGE-PROJECT-STATUS"
  - "UC-PRJ-PROGRESS-STATUS-AUTOMATICALLY"
  - "BR-PRJ-INITIAL-STATUS"
  - "BR-PRJ-QUOTE-SENT-PENDING"
  - "BR-PRJ-SIGNED-QUOTE-CONFIRMED"
  - "BR-PRJ-MANUAL-STATUS-ANY-TIME"
  - "BR-PRJ-FIXED-STATUS-SET"
verification:
  - scenario-ref: "SB-PRJ-BLANK-PROJECT-DRAFT"
  - scenario-ref: "SB-PRJ-TEMPLATE-STATUS"
  - scenario-ref: "SB-PRJ-QUOTE-SENT-PENDING"
  - scenario-ref: "SB-PRJ-SIGNED-QUOTE-CONFIRMS"
uses-terms:
  - "TERM-PROJECT-STATUS"
provenance:
  source: ".private/research/help-center/articles/360013587379.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST maintain the fixed project status set (inquiry, draft, pending, confirmed, in progress, execution done, complete, cancelled) with the documented initial states and automatic transitions. It MUST allow manual status changes within the lifecycle, but a Complete project MUST block operational edits and new planning until explicitly reopened.

## Rationale

Status drives what each area does with a project.
