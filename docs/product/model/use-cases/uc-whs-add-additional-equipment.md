---
id: UC-WHS-ADD-ADDITIONAL-EQUIPMENT
type: use-case
title: "Add additional equipment while booking"
status: draft
primary-actor: "ACT-WAREHOUSE-TECHNICIAN"
supporting-actors: []
bounded-context: "BC-WAREHOUSE"
governed-by:
  - "BR-WHS-UNPLANNED-SCAN-ADDS"
uses-terms:
  - "TERM-ADDITIONAL-EQUIPMENT"
provenance:
  source: ".private/research/help-center/articles/13901986840082.md, .private/research/help-center/articles/360015601560.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Extra gear taken along is recorded on the job.

## Trigger

Something unplanned is needed or scanned.

## Preconditions

A list is being booked.

## Main Flow

1. The technician searches an article, sets quantity, price inclusion and times (default project times or custom).
2. The product adds it in a new equipment group and books it.

## Alternative Flows

- Triggered by scanning an unplanned code.

## Failure Conditions

- An article that cannot be found, or a quantity that is zero or not a number, is refused with the reason and nothing is added.
- An unplanned scanned code is never booked silently; without confirmation, nothing is added (BR-WHS-UNPLANNED-SCAN-ADDS).
- Times outside what the project allows are refused with the reason.
- An actor without permission to book is refused and nothing changes.
- If another user changed the list in the meantime, the addition is refused and the technician must reload and retry.

## Postconditions

The project includes the extra lines.
