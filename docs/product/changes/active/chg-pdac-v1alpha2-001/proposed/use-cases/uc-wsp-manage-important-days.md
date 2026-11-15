---
id: UC-WSP-MANAGE-IMPORTANT-DAYS
type: use-case
title: "Mark important days"
status: draft
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-WORKSPACE"
governed-by:
  - "BR-WSP-IMPORTANT-DAYS-NON-BLOCKING"
uses-terms:
  - "TERM-IMPORTANT-DAY"
provenance:
  source: ".private/research/help-center/articles/19386134798866.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Everyone sees public holidays and company events while planning.

## Trigger

A holiday or company event is coming up.

## Preconditions

- The administrator has configuration access.

## Main Flow

1. The administrator adds a day with a name and date.
2. Optionally the administrator makes it repeat every year.
3. The administrator saves; the day is marked in schedules, planning timelines, project details and the schedule widget.

## Alternative Flows

- None.

## Failure Conditions

- The user lacks configuration access: the day is refused and nothing is saved.
- The name or date is missing or invalid: it is refused with the reason and nothing is saved.
- Another administrator changed the same day in the meantime: the save is refused and the user must reload and retry.
- A day marked in a period never blocks planning; it is only highlighted (BR-WSP-IMPORTANT-DAYS-NON-BLOCKING).

## Postconditions

- The important day is visible to all users.
