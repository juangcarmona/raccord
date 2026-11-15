---
id: UC-WHS-CHECK-AVAILABILITY
type: use-case
title: "Check availability quickly"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-EQUIPMENT"
governed-by:
  - "BR-WHS-AVAILABILITY-BY-PLANNING-PERIOD"
uses-terms:
  - "TERM-EQUIPMENT-AVAILABILITY"
  - "TERM-AVAILABILITY-FORECAST"
provenance:
  source: ".private/research/help-center/articles/360013158280.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The coordinator knows at once how many units are free for a period.

## Trigger

A customer asks on the phone whether something is available.

## Preconditions

None.

## Main Flow

1. The coordinator opens the quick availability check from anywhere and types at least three letters of the article name.
2. The coordinator picks the article and period.
3. The product shows the free quantity, negative when short.

## Alternative Flows

- None.

## Failure Conditions

- Fewer than three characters: no search is made.

## Postconditions

Nothing is changed.
