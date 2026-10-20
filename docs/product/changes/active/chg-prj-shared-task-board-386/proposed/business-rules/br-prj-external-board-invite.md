---
id: BR-PRJ-EXTERNAL-BOARD-INVITE
type: business-rule
title: "Inviting an outsider to a board creates a crew member without project access"
status: active
applies-to:
  - "UC-PRJ-SHARE-PROJECT-TASK-BOARD"
uses-terms:
  - "TERM-PROJECT-TASK-BOARD"
provenance:
  source: ".private/research/help-center/articles/38422735121170.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Inviting a person by e-mail to a project task board invites them as a crew member; access starts only on acceptance; they then see every task of that project, but not the project itself, its equipment, crew planning or financials, nor other boards; private tasks stay hidden; removing them from the board ends access.

## Rationale

Enables client collaboration while protecting the business.

## Examples

See SB-PRJ-EXTERNAL-SEES-BOARD-NOT-PROJECT.

## Exceptions

None.
