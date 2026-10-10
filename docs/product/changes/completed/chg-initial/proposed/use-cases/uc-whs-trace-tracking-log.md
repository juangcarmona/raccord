---
id: UC-WHS-TRACE-TRACKING-LOG
type: use-case
title: "Trace equipment handling in the tracking log"
status: draft
primary-actor: "ACT-WAREHOUSE-TECHNICIAN"
supporting-actors:
  - "ACT-OPERATIONS-COORDINATOR"
bounded-context: "BC-WAREHOUSE"
governed-by: []
uses-terms:
  - "TERM-WAREHOUSE-TRACKING-LOG"
provenance:
  source: ".private/research/help-center/articles/20860250675474.md"
  confidence: high
  recovered-from: documentation
---

## Goal

It is known who did what with an article and when.

## Trigger

An article is missing or disputed.

## Preconditions

None.

## Main Flow

1. The user opens the log directly or from an article, case, project or crew member.
2. The user combines filters on actions, projects, articles and crew, plus date.
3. The product lists matching actions.

## Alternative Flows

- Search filter values not shown among the first ten.

## Failure Conditions

- More than 5,000 rows: narrow filters.

## Postconditions

Nothing changes.
