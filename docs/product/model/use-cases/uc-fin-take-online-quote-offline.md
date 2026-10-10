---
id: UC-FIN-TAKE-ONLINE-QUOTE-OFFLINE
type: use-case
title: "Take an online quote offline"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-COMMERCIAL"
governed-by: []
uses-terms:
  - "TERM-ONLINE-QUOTE-STATUS"
provenance:
  source: ".private/research/help-center/articles/29481848461458.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The customer can no longer open the quote page.

## Trigger

The offer is withdrawn or must be revised in private.

## Preconditions

The quote is published.

## Main Flow

1. The coordinator takes the quote offline.
2. The product makes the link unavailable.

## Alternative Flows

The coordinator later publishes the latest version again.

## Failure Conditions

- Taking offline a quote that is not published is refused with the reason and nothing changes.
- An actor without permission to manage online quotes is refused and the page stays available.
- If the page cannot be withdrawn, the failure is visible and the action can be retried; the link remains usable until it succeeds.
- If another user changed the quote in the meantime, the change is refused and the coordinator must reload and retry.

## Postconditions

The online quote is offline.
