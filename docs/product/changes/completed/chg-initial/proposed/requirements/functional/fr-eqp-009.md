---
id: FR-EQP-009
type: functional-requirement
title: "Support nested kits up to ten levels"
status: draft
derived-from:
  - "UC-EQP-NEST-KITS"
  - "BR-EQP-NESTING-LIMIT"
  - "CON-EQP-NESTING-DEPTH"
verification:
  - scenario-ref: "SB-EQP-NESTING-ELEVEN"
uses-terms:
  - "TERM-NESTED-KIT"
provenance:
  source: ".private/research/help-center/articles/19934846796306.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST allow kits as content of kits in default content and planned content, up to 10 levels.

## Rationale

Modular setups.
