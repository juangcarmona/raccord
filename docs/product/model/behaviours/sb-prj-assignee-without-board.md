---
id: SB-PRJ-ASSIGNEE-WITHOUT-BOARD
type: structured-behaviour
title: "Assignment does not give the board"
status: draft
illustrates:
  - "BR-PRJ-PROJECT-BOARD-VISIBILITY"
given:
  - "A user without the Admin or Office role is assigned one task of project P"
  - "The P board is not shared with them"
when: "The user looks at their boards"
then:
  - "The P board is not listed"
  - "The assigned task is visible to them"
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/38422661951506.md"
  confidence: high
  recovered-from: documentation
---

## Intent

Scope of assignment.

## Boundaries

None.
