---
id: UC-PRJ-CANCEL-PROJECT
type: use-case
title: "Cancel a project"
status: active
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors:
  - "ACT-CREW-MEMBER"
  - "ACT-RACCORD-AUTOMATION"
bounded-context: "BC-PROJECTS"
governed-by:
  - "BR-PRJ-CANCEL-MANUAL-ONLY"
  - "BR-PRJ-CANCELLATION-ACTIONS-APPLICABLE"
  - "BR-PRJ-CANCEL-RELEASES-EQUIPMENT"
  - "BR-PRJ-CANCEL-OPEN-ITEMS-WARNING"
  - "BR-PRJ-CANCEL-PRESERVES-INVOICED-WORK"
uses-terms:
  - "TERM-PROJECT-STATUS-CANCELLED"
  - "TERM-CANCELLATION-ACTION"
provenance:
  source: ".private/research/help-center/articles/28083930405778.md, .private/research/help-center/articles/360013587379.md"
  confidence: high
  recovered-from: documentation
---

## Goal

A project that will not go ahead is cancelled and every party affected is dealt with.

## Trigger

The customer withdraws or the event is called off.

## Preconditions

The project exists and is not cancelled.

## Main Flow

1. The coordinator sets the status to Cancelled.
2. The product lists the applicable cancellation actions: withdraw invitations, complete open tasks, remove planned crew and transport, inform crew members (with optional message).
3. The coordinator chooses actions and confirms, or declines all actions.
4. The product performs the chosen actions, releases the equipment reservations and saves.

## Alternative Flows

None.

## Failure Conditions

Open crew planning or tasks remain after cancelling without actions: the product keeps a warning on the project.

## Postconditions

The project is Cancelled, hidden from operational overviews, still in statistics; its equipment is available to other projects.
