---
id: UC-PRJ-VIEW-EQUIPMENT-TOTALS
type: use-case
title: "Review physical and price totals of planned equipment"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-PROJECTS"
governed-by:
  - "BR-PRJ-EQUIPMENT-TOTALS"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/24621478894354.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The coordinator knows total weight, volume, power and rental value of the planned equipment.

## Trigger

Transport or power planning needs totals.

## Preconditions

Items have weight, volume and power recorded.

## Main Flow

1. The coordinator opens the details of a planned item.
2. The product shows totals of weight, volume, power usage, power, rental price and discounted rental price for the item's group and project.

## Alternative Flows

None.

## Failure Conditions

Items without data contribute nothing.

## Postconditions

None (read-only).
