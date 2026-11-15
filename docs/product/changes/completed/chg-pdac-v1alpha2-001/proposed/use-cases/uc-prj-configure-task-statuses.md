---
id: UC-PRJ-CONFIGURE-TASK-STATUSES
type: use-case
title: "Configure the task status workflow"
status: draft
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-PROJECTS"
governed-by:
  - "BR-PRJ-TASK-STATUS-ENDPOINTS"
  - "BR-PRJ-TASK-STATUS-DELETION"
  - "BR-PRJ-TASK-STATUS-ORDER"
  - "BR-PRJ-TASK-STATUS-MIGRATION"
uses-terms:
  - "TERM-TASK-STATUS"
provenance:
  source: ".private/research/help-center/articles/38421530397202.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Task statuses match how work moves through the company.

## Trigger

Setting up structured task work.

## Preconditions

Administrator rights.

## Main Flow

1. The administrator adds statuses with name and colour, renames or recolours the fixed To do and Completed, and reorders statuses.
2. The administrator deletes a status, choosing where its tasks move.
3. The product saves the company-wide workflow.

## Alternative Flows

None.

## Failure Conditions

To do and Completed cannot be deleted.

## Postconditions

Boards show columns in the configured order.
