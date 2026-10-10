---
id: UC-PRJ-TAG-RECORDS
type: use-case
title: "Tag records"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-WORKSPACE"
governed-by:
  - "BR-PRJ-TAG-RULES"
  - "BR-PRJ-TAG-FILTER-MODES"
uses-terms:
  - "TERM-TAG"
provenance:
  source: ".private/research/help-center/articles/360013931999.md, .private/research/help-center/articles/360014254000.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Records are grouped across folders by labels.

## Trigger

A grouping such as region, priority or skill is needed.

## Preconditions

The user may manage tags in the module.

## Main Flow

1. The user selects records and adds tags (no spaces).
2. The product stores them; users filter by tags matching any or all selected.

## Alternative Flows

- Deleting tags from the tag selector.

## Failure Conditions

Tag typed but not confirmed is not added.

## Postconditions

Records carry the tags.
