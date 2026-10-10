---
id: UC-FIN-MANAGE-CREW-EXPENSES
type: use-case
title: "Manage crew expenses"
status: draft
primary-actor: "ACT-CREW-PLANNER"
supporting-actors: []
bounded-context: "BC-COMMERCIAL"
governed-by:
  - "BR-FIN-CREW-EXPENSE-CATEGORIES"
  - "BR-FIN-CREW-EXPENSE-INHERITANCE"
  - "BR-FIN-CREW-EXPENSE-MARGIN"
uses-terms:
  - "TERM-CREW-EXPENSE"
provenance:
  source: ".private/research/help-center/articles/31405085792146.md, .private/research/help-center/articles/31369051432082.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Meals, travel, lodging and other crew costs are planned, priced and shown in project financials.

## Trigger

Crew work causes costs beyond labour.

## Preconditions

Functions exist.

## Main Flow

1. The planner sets cost and price per expense category on default functions.
2. When functions are planned, the product copies their expenses.
3. The planner adjusts expenses of planned functions in bulk or individually for this project.
4. The planner overrides expenses for a specific planned crew member.
5. The product shows totals and margin and includes them in the financial overview.

## Alternative Flows

None.

## Failure Conditions

Custom categories cannot be created.

## Postconditions

The project carries its crew expenses.
