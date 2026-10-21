---
id: UC-PRJ-CREATE-FROM-TEMPLATE
type: use-case
title: "Create a project from a template"
status: active
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-PROJECTS"
governed-by:
  - "BR-PRJ-SCHEDULE-REQUIRED"
  - "BR-PRJ-TEMPLATE-START-ANCHOR"
  - "BR-PRJ-TEMPLATE-CONTENT"
  - "BR-PRJ-INITIAL-STATUS"
  - "BR-PRJ-PROJECT-NUMBER-ASSIGNMENT"
  - "BR-PRJ-ARCHIVED-TEMPLATE-ITEM"
  - "BR-PRJ-CALENDAR-QUICK-CREATE"
uses-terms:
  - "TERM-PROJECT-TEMPLATE"
  - "TERM-PROJECT-TYPE"
  - "TERM-TIME-SCHEDULE"
  - "TERM-PROJECT-STATUS"
provenance:
  source: ".private/research/help-center/articles/360013590720.md, .private/research/help-center/articles/360013367659.md, .private/research/help-center/articles/360012594219.md, .private/research/help-center/articles/360013084979.md, .private/research/help-center/articles/360013587379.md, .private/research/help-center/articles/24626249194386.md"
  confidence: high
  recovered-from: documentation
---

## Goal

A new project starts with the content of a stored template, anchored at a chosen start date.

## Trigger

A job resembling a recurring kind of work is commissioned; or the coordinator picks a day in the project calendar or the personal schedule while talking to a customer.

## Preconditions

At least one project template exists. The coordinator may create projects.

## Main Flow

1. The coordinator starts a new project and chooses a project type and a template.
2. The coordinator chooses the start date (prefilled when started from a calendar day).
3. The product copies all periods of the template's schedule, shifted so that the schedule starts on the chosen date, and lets the coordinator adjust start and end times of each period.
4. The product previews the copied schedule, equipment and function groups.
5. The coordinator saves; the product creates the project with the template's client, location, notes, tasks, equipment, functions, additional costs, financial details and starting status, and assigns a project number.

## Alternative Flows

- Blank project chosen instead of a template: see UC-CREATE-PROJECT; the project starts in Draft with default planning and usage periods.
- The template contains an equipment item archived since: the item is copied but presented as no longer existing.

## Failure Conditions

Saving without a time schedule (for a category that requires one) is refused.

## Postconditions

A project exists with its status set from the template; crew and vehicle assignments are empty.
