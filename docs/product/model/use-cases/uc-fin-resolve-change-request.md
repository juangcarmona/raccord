---
id: UC-FIN-RESOLVE-CHANGE-REQUEST
type: use-case
title: "Resolve a quote change request"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-COMMERCIAL"
governed-by: []
uses-terms:
  - "TERM-QUOTE-CHANGE-REQUEST"
provenance:
  source: ".private/research/help-center/articles/29481848461458.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Every change asked by the customer is handled and tracked.

## Trigger

A customer requested changes on an online quote.

## Preconditions

An open change request exists.

## Main Flow

1. The coordinator reviews the request in the project.
2. The coordinator adjusts the project and regenerates the quote.
3. The coordinator marks the request as resolved.

## Alternative Flows

The request is resolved without changes after discussion.

## Failure Conditions

- Resolving a request that is not open, for example already resolved by a colleague, is refused and the coordinator must reload.
- An actor without permission to edit the project is refused and nothing changes.
- If the quote cannot be regenerated, the request stays open and the action can be retried.
- If another user changed the project in the meantime, the change is refused and the coordinator must reload and retry.

## Postconditions

The change request is resolved.
