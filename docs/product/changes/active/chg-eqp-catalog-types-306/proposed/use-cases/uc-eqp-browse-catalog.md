---
id: UC-EQP-BROWSE-CATALOG
type: use-case
title: "Browse the equipment catalog"
status: draft
primary-actor: "ACT-EQUIPMENT-MANAGER"
supporting-actors: []
bounded-context: "BC-EQUIPMENT"
governed-by: []
uses-terms:
    - "TERM-EQUIPMENT-ITEM"
provenance:
    source: "Issue #306"
    confidence: high
    recovered-from: inference
---

## Goal

The equipment manager can navigate every item in the catalog without loading the whole catalog at once.

## Trigger

The equipment manager opens the Equipment catalog or continues to another page.

## Preconditions

- The equipment manager may view the catalog.

## Main Flow

1. The manager opens the catalog.
2. The product returns a bounded page of items in a stable order and indicates whether another page exists.
3. The manager follows the next-page action.
4. The product returns the next page without repeating or omitting items as long as the catalog has not changed.
5. The manager continues until the product indicates there is no next page.

## Alternative Flows

- 2a. The catalog contains no items: the product displays an empty catalog state.
- 3a. No next page exists: the next-page action is not offered.

## Failure Conditions

- An invalid or expired continuation token is rejected and the manager can restart browsing from the first page.

## Postconditions

- The manager can reach all catalog items through bounded pages.
