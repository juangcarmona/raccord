---
id: UC-CRW-ENTER-CREW-AVAILABILITY
type: use-case
title: "Enter availability for crew members"
status: draft
primary-actor: "ACT-CREW-PLANNER"
supporting-actors: []
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-AVAILABILITY-RIGHTS"
  - "BR-CRW-SELF-PRESELECTED"
  - "BR-CRW-RECURRENCE-SERIES"
uses-terms:
  - "TERM-AVAILABILITY-ENTRY"
  - "TERM-RECURRENCE-PATTERN"
provenance:
  source: ".private/research/help-center/articles/360014685859.md, .private/research/help-center/articles/23917302179218.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Known availability of crew is recorded.

## Trigger

The planner learns someone's availability.

## Preconditions

The planner has the right to enter it for others.

## Main Flow

1. The planner selects crew members and a period.
2. The planner marks them available, unavailable or unknown, optionally with recurrence and remark.
3. The product records the entries.

## Alternative Flows

All preselected crew are added at once.

## Failure Conditions

Without the right the entry is refused.

## Postconditions

Availability states reflect the entries.
