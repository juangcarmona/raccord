---
id: BR-CRW-RATE-CHANGE-NOT-RETROACTIVE
type: business-rule
title: "Rate changes apply only to new planning"
status: draft
applies-to:
  - "UC-CRW-MANAGE-CREW-RATES"
uses-terms:
  - "TERM-CREW-RATE"
provenance:
  source: ".private/research/help-center/articles/7191443180562.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

Changing a crew rate affects only functions and crew assignments planned afterwards; existing functions and assignments keep the rate values they had.

## Rationale

Quoted and planned work must not change silently.

## Examples

Raising the technician rate today leaves last week's planned function costs unchanged.

## Exceptions

None.
