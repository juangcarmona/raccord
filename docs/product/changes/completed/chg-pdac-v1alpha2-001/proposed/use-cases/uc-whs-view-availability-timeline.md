---
id: UC-WHS-VIEW-AVAILABILITY-TIMELINE
type: use-case
title: "Inspect availability over time"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-EQUIPMENT"
governed-by:
  - "BR-WHS-STATUS-AVAILABILITY-EFFECT"
  - "BR-WHS-AVAILABILITY-BY-PLANNING-PERIOD"
uses-terms:
  - "TERM-AVAILABILITY-FORECAST"
  - "TERM-RESERVABLE-STOCK"
  - "TERM-RESERVATION-STATE"
provenance:
  source: ".private/research/help-center/articles/13369374381714.md, .private/research/help-center/articles/360013158280.md, .private/research/help-center/articles/360021519799.md, .private/research/help-center/articles/360013730520.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The coordinator understands, for chosen articles, what consumes stock when.

## Trigger

Planning decisions or shortage analysis.

## Preconditions

Articles exist.

## Main Flow

1. The coordinator selects single articles or combinations and opens their timeline.
2. The product shows the forecast line, the reservable stock line, each project's bars distinguishing planning from usage periods and pending, partly reserved and fully reserved states, stock increases and decreases, free space, fully planned and overbooked intervals.

## Alternative Flows

- From inside a project, the current project is highlighted and its bars can be clicked to change reservations.

## Failure Conditions

- A user without access to the articles sees only what the user may see; others are not shown.
- An article with no planned usage shows only its stock.
- Changing a reservation from a project bar without permission to edit the project is refused and nothing changes.
- Cancelled projects show no consumption (BR-WHS-STATUS-AVAILABILITY-EFFECT).

## Postconditions

Nothing is changed unless a reservation is edited.
