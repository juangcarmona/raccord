---
id: UC-WHS-SOLVE-WITH-ALTERNATIVE
type: use-case
title: "Solve a shortage with an alternative"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-EQUIPMENT"
governed-by:
  - "BR-WHS-ALTERNATIVE-REPLACEMENT"
  - "BR-WHS-ALTERNATIVE-SAVED"
  - "BR-WHS-RESOLUTION-USABILITY"
uses-terms:
  - "TERM-EQUIPMENT-ALTERNATIVE"
  - "TERM-SHORTAGE-RESOLUTION"
provenance:
  source: ".private/research/help-center/articles/16611399161234.md, .private/research/help-center/articles/360017700639.md"
  confidence: high
  recovered-from: documentation
---

## Goal

A short article is replaced by a similar available one.

## Trigger

A shortage exists and substitutes are acceptable.

## Preconditions

A line is in shortage.

## Main Flow

1. The coordinator selects the line and asks to solve its shortage.
2. The coordinator picks a configured alternative or searches the catalog.
3. The coordinator chooses how many units to replace (only the short ones or all) and whether to keep the original price.
4. The product replaces the units and recomputes availability.

## Alternative Flows

- From the line's details, alternatives can be added directly.

## Failure Conditions

- Alternative also short: the new line shows its own shortage.

## Postconditions

The shortage is reduced; a searched alternative is saved for reuse.
