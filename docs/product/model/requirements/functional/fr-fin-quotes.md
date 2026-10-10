---
id: FR-FIN-QUOTES
type: functional-requirement
title: "Generate quotes and contracts from the plan"
status: draft
derived-from:
  - "UC-FIN-GENERATE-QUOTE"
  - "BR-FIN-QUOTE-SNAPSHOTS-PLAN"
  - "BR-FIN-QUOTE-CONTRACT-SAME"
  - "BR-FIN-QUOTE-VALIDITY-DEFAULT"
verification:
  - scenario-ref: "SB-FIN-PLAN-CHANGE-NOT-IN-QUOTE"
  - scenario-ref: "SB-FIN-QUOTE-EXPIRES-AFTER-14-DAYS"
uses-terms:
  - "TERM-QUOTE"
  - "TERM-CONTRACT"
provenance:
  source: ".private/research/help-center/articles/360012594659.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST generate numbered quotes and contracts from the project plan with validity, template, letterhead, downloadable as PDF and word-processor files.

## Rationale

Offers are the start of every sale.
