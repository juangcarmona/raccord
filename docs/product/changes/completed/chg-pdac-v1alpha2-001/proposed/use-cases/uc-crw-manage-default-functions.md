---
id: UC-CRW-MANAGE-DEFAULT-FUNCTIONS
type: use-case
title: "Define default crew and transport functions"
status: draft
primary-actor: "ACT-CREW-PLANNER"
supporting-actors:
  - "ACT-OPERATIONS-COORDINATOR"
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-DEFAULT-FUNCTION-PERMISSION"
  - "BR-CRW-FUNCTION-KIND-PRICING"
  - "BR-CRW-HOURLY-AND-FIXED-AS-SEPARATE-FUNCTIONS"
  - "BR-CRW-FUNCTION-NAMES-BY-AUDIENCE"
uses-terms:
  - "TERM-DEFAULT-FUNCTION"
  - "TERM-FUNCTION-KIND"
provenance:
  source: ".private/research/help-center/articles/14232209807506.md, .private/research/help-center/articles/360013223940.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Reusable functions with standard names, times, prices and costs exist.

## Trigger

Setting up or revising the catalogue.

## Preconditions

The user may manage default functions.

## Main Flow

1. The user creates a crew or transport default function.
2. The user sets external and internal name, folder, plannable flag, break, price, cost, VAT class, ledger account, include-in-price, distance, travel time, tags and remarks.
3. The product saves it.

## Alternative Flows

Editing an existing default function.

## Failure Conditions

Without the right the catalogue is read-only.

## Postconditions

The default function can be added to projects.
