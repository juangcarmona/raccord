---
id: UC-WSP-QUICK-SEARCH
type: use-case
title: "Jump to a record with quick access search"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-WORKSPACE"
governed-by: []
uses-terms:
  - "TERM-QUICK-ACCESS-SEARCH"
provenance:
  source: ".private/research/help-center/articles/360017016060.md"
  confidence: medium
  recovered-from: documentation
---

## Goal

The user opens a record directly without navigating modules.

## Trigger

A colleague mentions a project number or name.

## Preconditions

- The user is signed in.

## Main Flow

1. The user opens quick access with a key.
2. The user types a query; the product searches recently opened records, projects, equipment, contacts, invoices, crew members, tasks and vehicles.
3. The user moves to a result with the arrow keys and opens it.

## Alternative Flows

- None.

## Failure Conditions

- Results the user is not allowed to see are not offered.

## Postconditions

- The record is open.
