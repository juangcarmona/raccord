---
id: UC-CRW-GENERATE-CALL-SHEET
type: use-case
title: "Generate a call sheet"
status: draft
primary-actor: "ACT-CREW-PLANNER"
supporting-actors: []
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-FUNCTION-NAMES-BY-AUDIENCE"
uses-terms:
  - "TERM-CALL-SHEET"
provenance:
  source: ".private/research/help-center/articles/360014672919.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Team leaders have the whole project's schedule and crew.

## Trigger

A job approaches.

## Preconditions

The project has planning.

## Main Flow

1. The planner chooses template, letterhead, time zone, text and advanced options.
2. The product generates the call sheet.

## Alternative Flows

None.

## Failure Conditions

- The planner lacks permission to produce call sheets for the project: generation is refused and nothing is produced.
- The project has no planning in the chosen scope: no call sheet is produced and the planner is told why.
- The document cannot be produced: generation fails visibly and can be retried.

## Postconditions

A call sheet exists for the project.
