---
id: UC-PRJ-BROWSE-PROJECTS
type: use-case
title: "Browse and filter projects"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-PROJECTS"
governed-by:
  - "BR-PRJ-OVERVIEW-DAY-SCOPE"
  - "BR-PRJ-STATUS-GROUP-PRESENTATION"
  - "BR-PRJ-CALENDAR-COLOUR-BY-TYPE"
  - "BR-PRJ-COLUMN-SORT-LAYERS"
  - "BR-PRJ-TAG-FILTER-MODES"
  - "BR-TRB-OVERVIEW-PERIOD-OVERLAP"
uses-terms:
  - "TERM-PROJECT-OVERVIEW"
  - "TERM-OVERVIEW-DAY-SCOPE"
  - "TERM-PROJECT-DAY-KIND"
  - "TERM-PROJECT-STATUS-GROUP"
  - "TERM-PROJECT-PROGRESS-INDICATOR"
provenance:
  source: ".private/research/help-center/articles/360012594219.md, .private/research/help-center/articles/26916975812370.md, .private/research/help-center/articles/360012581679.md, .private/research/help-center/articles/39016808810258.md, .private/research/help-center/articles/360013587379.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The coordinator finds the projects that need attention.

## Trigger

Daily planning, follow-up, office screen.

## Preconditions

The user can see projects.

## Main Flow

1. The user chooses list, per-day, calendar or timeline mode.
2. The user filters by period, tags, project status, custom filters and (outside list mode) day kinds, and applies a preset or saved view.
3. The user adds, hides, sorts (multi-level), groups, resizes and pins columns.
4. The user opens a project's details panel.

## Alternative Flows

- The user limits shown days to the usage period.
- From the list the user opens a timeline for selected projects.

## Failure Conditions

- A user without permission to see projects sees none, and no project data is shown.
- Filters matching nothing show an empty result.
- Projects without any planning period appear only when the without-date selection is chosen (BR-TRB-OVERVIEW-PERIOD-OVERLAP).
- A saved view or preset that refers to a removed column, tag or status falls back to the remaining criteria and tells the user what was ignored.

## Postconditions

None (read-only); views may be saved.
