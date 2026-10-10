---
id: UC-WHS-ORGANIZE-GROUPS
type: use-case
title: "Organise planned equipment into groups"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-EQUIPMENT"
governed-by:
  - "BR-WHS-LINE-TIMES-DEFAULT"
  - "BR-WHS-LINE-TIMES-LINKED"
uses-terms:
  - "TERM-EQUIPMENT-GROUP"
  - "TERM-PLANNING-PERIOD"
  - "TERM-USAGE-PERIOD"
provenance:
  source: ".private/research/help-center/articles/360017647460.md, .private/research/help-center/articles/38542583895186.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The plan is structured in named groups with their own default times and remarks.

## Trigger

The coordinator wants to separate equipment by discipline, area or time.

## Preconditions

A project exists.

## Main Flow

1. The coordinator creates a group with a name, planning and usage periods (default: the project's) or a duration, and an optional remark for the quotation.
2. The coordinator moves lines into groups, or places selected lines into each article's default group in one action.
3. Editing a group's times applies them to all its lines.

## Alternative Flows

- Template: groups come pre-created from a project template.
- Duration instead of dates for groups without fixed times.

## Failure Conditions

- A group time that is not a schedule entry cannot be entered; a new entry is created instead.

## Postconditions

Lines are grouped; quotations and packing slips can follow the grouping.
