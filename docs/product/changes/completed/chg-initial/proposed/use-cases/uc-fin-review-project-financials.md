---
id: UC-FIN-REVIEW-PROJECT-FINANCIALS
type: use-case
title: "Review the financial results of a project"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-COMMERCIAL"
governed-by:
  - "BR-FIN-ESTIMATED-FUNCTION-COST"
  - "BR-FIN-SALE-ITEM-COST"
  - "BR-FIN-PLANNED-COST-SOURCES"
  - "BR-FIN-ACTUAL-COST-SOURCES"
  - "BR-FIN-REVENUE-AND-PROFIT"
  - "BR-FIN-FINANCIAL-VISIBILITY"
uses-terms:
  - "TERM-PROJECT-FINANCIAL-OVERVIEW"
  - "TERM-ESTIMATED-COST"
  - "TERM-PLANNED-COST"
  - "TERM-ACTUAL-COST"
  - "TERM-PROJECT-PROFIT"
provenance:
  source: ".private/research/help-center/articles/360013264720.md, .private/research/help-center/articles/18431827595666.md, .private/research/help-center/articles/31405085792146.md, .private/research/help-center/articles/360015729040.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The coordinator knows the expected, planned and actual cost, revenue, profit and amount invoiced of a project per category.

## Trigger

A quote must be priced, the plan has changed, or the job has finished.

## Preconditions

The project has planned equipment, functions or costs.

## Main Flow

1. The coordinator opens the project financial overview.
2. The product shows, per revenue category, estimated, planned and actual cost, revenue, discount, profit and total, plus crew expense breakdowns.
3. The coordinator switches between profit as an amount and margin as a percentage.
4. The product shows how much has already been invoiced and the project's quotes, contracts and invoices.

## Alternative Flows

None.

## Failure Conditions

The user lacks financial visibility: the overview is not available.

## Postconditions

Nothing changes; the coordinator has the figures.
