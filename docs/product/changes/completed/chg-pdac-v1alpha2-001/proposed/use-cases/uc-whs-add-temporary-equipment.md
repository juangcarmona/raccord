---
id: UC-WHS-ADD-TEMPORARY-EQUIPMENT
type: use-case
title: "Add temporary equipment"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-EQUIPMENT"
governed-by:
  - "BR-WHS-TEMPORARY-IS-SHORT"
uses-terms:
  - "TERM-TEMPORARY-EQUIPMENT"
provenance:
  source: ".private/research/help-center/articles/17005282714898.md"
  confidence: high
  recovered-from: documentation
---

## Goal

A one-off article is planned without entering it in the catalog.

## Trigger

The job needs something the company does not own and will not reuse.

## Preconditions

A project exists.

## Main Flow

1. The coordinator creates a temporary article with its details and group.
2. The product adds it and shows it entirely as a shortage.

## Alternative Flows

- Save to catalog: the coordinator later saves the temporary article into a chosen catalog folder.

## Failure Conditions

- A temporary article with missing required details is refused with the reason and nothing is added.
- A temporary article always shows entirely as a shortage, because it has no stock (BR-WHS-TEMPORARY-IS-SHORT).
- Saving to the catalog without a chosen folder, or with a name already used there, is refused with the reason.
- An actor without permission to plan equipment is refused and nothing changes.
- If another user changed the project in the meantime, the change is refused and the coordinator must reload and retry.

## Postconditions

The line exists and shows its shortage.
