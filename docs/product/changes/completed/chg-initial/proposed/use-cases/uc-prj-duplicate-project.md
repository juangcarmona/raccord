---
id: UC-PRJ-DUPLICATE-PROJECT
type: use-case
title: "Duplicate a project"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-PROJECTS"
governed-by:
  - "BR-PRJ-TEMPLATE-START-ANCHOR"
  - "BR-PRJ-PROJECT-NUMBER-ASSIGNMENT"
uses-terms:
  - "TERM-PROJECT"
  - "TERM-TIME-SCHEDULE"
provenance:
  source: ".private/research/help-center/articles/360013590720.md"
  confidence: medium
  recovered-from: documentation
---

## Goal

A new project is created as a copy of an existing one, moved to a new start date.

## Trigger

A new job will be largely the same as a previous one.

## Preconditions

The source project exists.

## Main Flow

1. The coordinator selects an existing project and asks to duplicate it.
2. The coordinator chooses the new start date.
3. The product creates a new project with the same content, all periods shifted by the same offset, and opens it for editing.

## Alternative Flows

None.

## Failure Conditions

No start date given: duplication does not proceed.

## Postconditions

An independent copy exists; changes to it do not affect the source.
