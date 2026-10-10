---
id: UC-PRJ-REVIEW-HISTORY
type: use-case
title: "Review the change history of a project or contact"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors: []
bounded-context: "BC-PROJECTS"
governed-by:
  - "BR-PRJ-HISTORY-STRUCTURE"
  - "BR-PRJ-HISTORY-FROM-ACTIVATION"
uses-terms:
  - "TERM-HISTORY-LOG"
provenance:
  source: ".private/research/help-center/articles/4405951357202.md, .private/research/help-center/articles/29017857097618.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The user knows what changed, by whom and when.

## Trigger

A planning difference must be explained.

## Preconditions

Change history is active.

## Main Flow

1. The user opens the history of a project, contact, equipment item or crew member.
2. The product lists saves grouped by hour and user, and per save the added, modified and deleted items by area, group, item and field with old and new values.

## Alternative Flows

None.

## Failure Conditions

Changes before activation are not available.

## Postconditions

None (read-only).
