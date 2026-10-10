---
id: FR-EQP-007
type: functional-requirement
title: "Configure kit content lines"
status: draft
derived-from:
  - "UC-EQP-CONFIGURE-DEFAULT-CONTENT"
  - "BR-EQP-CONTENT-BINDING-DEFAULT"
  - "BR-EQP-CONTENT-DOCUMENT-VISIBILITY"
verification:
  - scenario-ref: "SB-EQP-NEW-CONTENT-RESERVED"
  - scenario: "A content line set to hidden on quotes and shown on packing lists appears only on the packing list."
uses-terms:
  - "TERM-DEFAULT-CONTENT"
  - "TERM-CONTENT-STOCK-BINDING"
provenance:
  source: ".private/research/help-center/articles/19895983469330.md, .private/research/help-center/articles/20641311900690.md, .private/research/help-center/articles/360013703140.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST let users set, per default content line of a physical kit, its stock binding, and whether it is shown on quotes and on packing lists.

## Rationale

Each content piece behaves differently.
