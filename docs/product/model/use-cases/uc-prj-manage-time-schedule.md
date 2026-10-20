---
id: UC-PRJ-MANAGE-TIME-SCHEDULE
type: use-case
title: "Maintain a project time schedule"
status: active
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-PROJECTS"
governed-by:
  - "BR-PRJ-DEFAULT-PERIODS"
  - "BR-PRJ-DEFAULT-PERIOD-DESIGNATION"
  - "BR-PRJ-USAGE-WITHIN-PLANNING"
  - "BR-PRJ-LINKED-PERIOD-PROPAGATION"
  - "BR-PRJ-PERIOD-DOCUMENT-DISPLAY"
  - "BR-PRJ-FUNCTIONS-FROM-PERIODS"
uses-terms:
  - "TERM-TIME-SCHEDULE"
  - "TERM-TIME-PERIOD"
  - "TERM-PLANNING-PERIOD"
  - "TERM-USAGE-PERIOD"
provenance:
  source: ".private/research/help-center/articles/360013599680.md, .private/research/help-center/articles/360017700339.md, .private/research/help-center/articles/13992531637522.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The project's periods reflect the real phases of the job and drive linked planning.

## Trigger

A project is being set up or its phases change.

## Preconditions

The project exists or is being created.

## Main Flow

1. The coordinator reviews the default planning and usage periods.
2. The coordinator adds named periods (load-in, build, show, break, transport, cleaning) with start and end, by entering them or by drawing them on a timeline.
3. The coordinator duplicates periods that repeat, and sorts the schedule manually or chronologically.
4. The coordinator chooses for each period whether it is displayed on internal and on external documents.
5. The product shows for each period what is linked to it and saves.

## Alternative Flows

- The coordinator designates another period as default planning period, usage period, or both (see BR-PRJ-DEFAULT-PERIOD-DESIGNATION).
- The coordinator selects periods and generates crew and transport functions from them in one step.
- The coordinator extends, shortens or moves a period on the timeline.

## Failure Conditions

A usage period not shorter than the planning period is refused or flagged.

## Postconditions

Linked equipment groups and functions use the updated period times.
