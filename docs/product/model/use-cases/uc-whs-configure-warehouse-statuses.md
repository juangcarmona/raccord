---
id: UC-WHS-CONFIGURE-WAREHOUSE-STATUSES
type: use-case
title: "Configure custom warehouse statuses"
status: draft
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-WAREHOUSE"
governed-by:
  - "BR-WHS-LOCKED-STATUSES"
  - "BR-WHS-CUSTOM-STATUS-OUT"
uses-terms:
  - "TERM-CUSTOM-WAREHOUSE-STATUS"
  - "TERM-WAREHOUSE-STATUS"
provenance:
  source: ".private/research/help-center/articles/15239120279442.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Warehouse statuses mirror the company's handling steps.

## Trigger

A new handling step is introduced.

## Preconditions

None.

## Main Flow

1. The administrator adds a status, names it and drags it into order.
2. The product offers it as a booking target.

## Alternative Flows

- Reorder existing custom statuses; lists already in a status keep it.

## Failure Conditions

- Delete a used status or move a locked one: refused.

## Postconditions

Status set updated.
