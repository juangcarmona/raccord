---
id: QR-INT-TOKEN-LIFETIME
type: quality-requirement
title: "Token lifetime"
status: draft
quality-attribute: "security"
applies-to:
  - "UC-INT-ENABLE-API-ACCESS"
verification:
  - scenario-ref: SB-INT-TOKEN-EXPIRES
uses-terms:
  - "TERM-API-TOKEN"
provenance:
  source: ".private/research/help-center/articles/360013767839.md, .private/research/help-center/articles/30759527977490.md"
  confidence: high
  recovered-from: documentation
---

## Requirement

New API tokens MUST remain valid for ten years and no longer.

## Measurement

Compare creation and refusal dates of a token: refusal starts exactly ten years after creation.
