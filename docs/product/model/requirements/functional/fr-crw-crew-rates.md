---
id: FR-CRW-CREW-RATES
type: functional-requirement
title: "Banded crew price and cost rates"
status: draft
derived-from:
  - "UC-CRW-MANAGE-CREW-RATES"
  - "UC-CRW-LINK-RATES-TO-CREW-MEMBER"
  - "BR-CRW-RATE-FACTOR-CALCULATION"
  - "BR-CRW-PRICE-RATES-ON-FUNCTIONS-ONLY"
  - "BR-CRW-RATE-CHANGE-NOT-RETROACTIVE"
  - "BR-CRW-ONE-DEFAULT-RATE"
verification:
  - scenario-ref: "SB-CRW-OVERTIME-ONE-HOUR"
  - scenario-ref: "SB-CRW-OVERTIME-THREE-HOURS"
  - scenario-ref: "SB-CRW-RATE-CHANGE-FORWARD-ONLY"
  - scenario-ref: "SB-CRW-DAILY-RATE-OVER-MAXIMUM"
uses-terms:
  - "TERM-CREW-RATE"
  - "TERM-RATE-FACTOR"
provenance:
  source: ".private/research/help-center/articles/7191443180562.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST support global and individual crew rates of kind price or cost made of hour bands with fixed and hourly amounts, link several rates and one default per crew member, and apply rate changes only to new planning.

## Rationale

Correct pricing and costing of labour.
