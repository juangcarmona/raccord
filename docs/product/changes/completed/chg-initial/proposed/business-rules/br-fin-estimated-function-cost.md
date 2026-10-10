---
id: BR-FIN-ESTIMATED-FUNCTION-COST
type: business-rule
title: "Estimated cost of a crew or transport function"
status: draft
applies-to:
  - "UC-FIN-REVIEW-PROJECT-FINANCIALS"
uses-terms:
  - "TERM-ESTIMATED-COST"
provenance:
  source: ".private/research/help-center/articles/360013264720.md"
  confidence: high
  recovered-from: documentation
---

## Rule

The estimated cost of a crew function is (fixed cost + duration in hours x hourly cost) x function quantity, and of a transport function (fixed cost + distance in kilometres x cost per kilometre) x function quantity; the estimated crew and transport costs of a project are the sums over its functions.

## Rationale

Lets the cost of a job be judged before any person or vehicle is chosen.

## Examples

See SB-FIN-ESTIMATED-CREW-COST.

## Exceptions

None.
