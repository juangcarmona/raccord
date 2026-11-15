---
id: UC-WHS-REFRESH-PLANNED-DATA
type: use-case
title: "Refresh planned equipment from the catalog"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-EQUIPMENT"
governed-by:
  - "BR-WHS-PLANNED-SNAPSHOT"
  - "BR-WHS-REFRESH-VIRTUAL-CONTENT"
uses-terms:
  - "TERM-PLANNED-EQUIPMENT"
provenance:
  source: ".private/research/help-center/articles/360013765880.md, .private/research/help-center/articles/23230026077586.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Outdated planned data (prices, names) is aligned with the catalog.

## Trigger

A project was duplicated or the catalog changed during a long tour.

## Preconditions

Lines exist.

## Main Flow

1. The coordinator selects lines and asks to update equipment data.
2. The product overwrites the selected lines' copied data with current catalog data.

## Alternative Flows

- Virtual combinations: optionally realign content to the catalog definition, removing obsolete content lines.
- Recompute a virtual combination's price from its current content.

## Failure Conditions

- Refreshing with no lines selected is refused with the reason.
- A line whose article no longer exists in the catalog is left unchanged and reported (BR-WHS-PLANNED-SNAPSHOT).
- Realigning a virtual combination removes lines no longer in its catalog definition from the project; this is stated before it applies (BR-WHS-REFRESH-VIRTUAL-CONTENT).
- An actor without permission to edit the plan is refused and nothing changes.
- If another user changed the lines in the meantime, the refresh is refused and the coordinator must reload and retry.

## Postconditions

Selected lines match the catalog.
