---
id: UC-WHS-RESERVE-EQUIPMENT
type: use-case
title: "Reserve equipment for a project"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-EQUIPMENT"
governed-by:
  - "BR-WHS-STATUS-AVAILABILITY-EFFECT"
  - "BR-WHS-AUTO-RESERVE-ON-CONFIRM"
  - "BR-WHS-MANUAL-RESERVATION"
  - "BR-WHS-RESERVATION-PRIORITY"
uses-terms:
  - "TERM-RESERVATION"
  - "TERM-RESERVATION-STATE"
  - "TERM-SOFT-HOLD"
provenance:
  source: ".private/research/help-center/articles/360013158280.md, .private/research/help-center/articles/13415175193490.md, .private/research/help-center/articles/360013730520.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The project secures stock with priority over others.

## Trigger

Confirmation, or a decision to prioritise a not-yet-confirmed project.

## Preconditions

The project's status consumes availability.

## Main Flow

1. The coordinator asks to reserve the equipment of a draft or pending project.
2. The product reserves the available part of each line.
3. Lines show their reservation state.

## Alternative Flows

- Confirmation: the product reserves automatically.
- Re-saving a confirmed project reserves newly freed stock.

## Failure Conditions

- Inquiry or cancelled project: no reservation possible.
- Insufficient reservable stock: the remainder stays in shortage.

## Postconditions

Reserved quantities are protected.
