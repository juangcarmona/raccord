---
id: BR-FIN-FINANCIAL-VISIBILITY
type: business-rule
title: "Roles without financial visibility see no financial details"
status: draft
applies-to:
  - "UC-FIN-RESTRICT-FINANCIAL-VISIBILITY"
  - "BC-WORKSPACE"
uses-terms:
  - "TERM-FINANCIAL-DETAILS"
provenance:
  source: ".private/research/help-center/articles/4409044768658.md, .private/research/help-center/articles/360017645140.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Users whose role lacks financial visibility see no financial details anywhere in the product, cannot add additional costs, but can still plan equipment and crew, whose financial effects are recalculated in the background; access to projects otherwise implies access to their financial overview.

## Rationale

Staff should not see purchase prices and margins they do not need.

## Examples

See SB-FIN-HIDDEN-FINANCIALS-STILL-UPDATE.

## Exceptions

None.
