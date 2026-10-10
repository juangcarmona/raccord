---
id: FR-FIN-QUOTE-VERSIONS
type: functional-requirement
title: "Version and regenerate quotes"
status: draft
derived-from:
  - "UC-FIN-REGENERATE-QUOTE"
  - "BR-FIN-REGENERATION-MODES"
  - "BR-FIN-NEW-SIGNATURE-NEW-NUMBER"
verification:
  - scenario-ref: "SB-FIN-NEW-VERSION-ARCHIVES-PREVIOUS"
uses-terms:
  - "TERM-REGENERATION-MODE"
provenance:
  source: ".private/research/help-center/articles/360012594659.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST offer overwrite, new version and new quote when regenerating, keeping earlier versions.

## Rationale

Offers evolve; history must be traceable.
