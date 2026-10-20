---
id: BR-PRJ-PERIOD-EDIT-AVAILABILITY
type: business-rule
title: Refuse period edits that make linked equipment unavailable
status: active
applies-to:
  - UC-PRJ-MANAGE-TIME-SCHEDULE
uses-terms:
  - TERM-TIME-PERIOD
  - TERM-EQUIPMENT-AVAILABILITY
  - TERM-RESERVATION
---

## Rule

Before saving an individual project-period edit, the product rechecks availability for all linked equipment reservations affected by the changed period. If any affected reservation becomes unavailable, the edit is refused in full and no period, equipment group, reservation or linked function is changed.

## Rationale

A project schedule and the equipment and crew planning linked to it must not disagree after a rejected edit.

## Examples

When moving a build period would make a linked equipment reservation unavailable, the product reports the conflict and keeps the period and all linked planning at their original times.

## Exceptions

Whole-project shifts follow the separate availability rule in #304. Changes to equipment lines and reservation mechanics remain governed by #310–#311.
