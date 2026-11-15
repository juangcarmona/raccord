---
id: UC-WHS-MARK-READY-TO-PACK
type: use-case
title: "Hand a project over to the warehouse"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors:
  - "ACT-WAREHOUSE-TECHNICIAN"
bounded-context: "BC-WAREHOUSE"
governed-by:
  - "BR-WHS-READY-TO-PACK-VISIBILITY"
  - "BR-WHS-READY-TO-PACK-LOCK"
  - "BR-WHS-BOOKING-MARKS-READY"
uses-terms:
  - "TERM-READY-TO-PACK"
  - "TERM-PACKING-LIST"
provenance:
  source: ".private/research/help-center/articles/38542583895186.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The warehouse starts packing only when the plan is final.

## Trigger

The coordinator locks the equipment plan.

## Preconditions

The project has planned equipment.

## Main Flow

1. The coordinator switches the project to ready to pack.
2. The product shows its packing lists on the warehouse board immediately.

## Alternative Flows

- Switching back off while nothing is booked hides the lists again.
- A technician books an unmarked project: after warning the product marks it ready.

## Failure Conditions

- Switching off after the first booking: refused.

## Postconditions

Packing lists are visible to the crew.
