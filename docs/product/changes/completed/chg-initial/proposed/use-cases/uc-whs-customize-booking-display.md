---
id: UC-WHS-CUSTOMIZE-BOOKING-DISPLAY
type: use-case
title: "Customise how equipment is shown while booking"
status: draft
primary-actor: "ACT-WAREHOUSE-TECHNICIAN"
supporting-actors: []
bounded-context: "BC-WAREHOUSE"
governed-by: []
uses-terms:
  - "TERM-PACKING-LIST"
provenance:
  source: ".private/research/help-center/articles/24058469404818.md, .private/research/help-center/articles/14004547874834.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Lists are shown in the order the crew walks the shelves.

## Trigger

A technician prepares to book.

## Preconditions

None.

## Main Flow

1. The technician chooses grouping (project, equipment group, warehouse position, folder, none), sorting (name, project order, code, warehouse position, folder) and display (only virtual-combination content; combine identical lines).

## Alternative Flows

- None.

## Failure Conditions

- A grouping or sorting that is not available for the list (for example by warehouse position when none is recorded) is refused with the reason and the current display stays.
- A list with no lines shows empty under any display choice.
- A user without access to the list does not see it.

## Postconditions

Display preferences applied.
