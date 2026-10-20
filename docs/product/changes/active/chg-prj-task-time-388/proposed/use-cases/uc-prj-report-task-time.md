---
id: UC-PRJ-REPORT-TASK-TIME
type: use-case
title: "Compare planned and tracked task time"
status: active
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-PROJECTS"
governed-by:
  - "BR-PRJ-SERVICE-BREAKDOWN"
  - "BR-PRJ-UNAPPROVED-HOURS-EXPORT-WARNING"
uses-terms:
  - "TERM-TRACKED-TIME"
  - "TERM-TASK-PLANNED-TIME"
provenance:
  source: ".private/research/help-center/articles/38423389508242.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Estimates are checked against reality to price future work.

## Trigger

A project ends or a quote is prepared.

## Preconditions

Tasks have planned and tracked time.

## Main Flow

1. The user applies the tracked-hours view on a task list, reads the service breakdown, or exports tracked time.
2. The product shows planned versus tracked hours; exports warn when they include unapproved hours.

## Alternative Flows

- Export from a team board contains only the team's work.

## Failure Conditions

- Tasks without planned or tracked time show no figures; the report states which data is missing.
- Exports include unapproved hours and warn when they do (BR-PRJ-UNAPPROVED-HOURS-EXPORT-WARNING).
- A service without a cost per hour shows no cost or profit (BR-PRJ-SERVICE-BREAKDOWN).
- A user without access to the tasks or financial details does not see them; the export contains only what the user may see.
- If the file cannot be produced, the export fails visibly and can be retried.

## Postconditions

None (read-only).
