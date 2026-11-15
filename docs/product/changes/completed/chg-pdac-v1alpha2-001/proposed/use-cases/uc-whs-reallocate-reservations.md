---
id: UC-WHS-REALLOCATE-RESERVATIONS
type: use-case
title: "Reallocate reservations between projects"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-EQUIPMENT"
governed-by:
  - "BR-WHS-RESERVATION-PRIORITY"
  - "BR-WHS-MANUAL-RESERVATION"
uses-terms:
  - "TERM-RESERVATION"
provenance:
  source: ".private/research/help-center/articles/360013158280.md, .private/research/help-center/articles/13415175193490.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Scarce stock goes to the project the business prefers.

## Trigger

Several projects are short of the same article in overlapping periods.

## Preconditions

Projects with overlapping lines exist.

## Main Flow

1. The coordinator opens the article's timeline from the project.
2. The coordinator selects a project bar and lowers the reserved quantity of other projects.
3. The coordinator enters the reserved quantity for the chosen project and saves.
4. The product updates reservable stock and shortages of all affected projects.

## Alternative Flows

- Afterwards the deprived project may be solved with an alternative article.

## Failure Conditions

- Reserved quantity above reservable stock: refused until others are lowered.

## Postconditions

Priority is explicit.
