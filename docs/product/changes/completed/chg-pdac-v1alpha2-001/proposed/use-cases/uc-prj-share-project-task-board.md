---
id: UC-PRJ-SHARE-PROJECT-TASK-BOARD
type: use-case
title: "Share a project task board"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors:
  - "ACT-CREW-MEMBER"
bounded-context: "BC-PROJECTS"
governed-by:
  - "BR-PRJ-BOARD-SHARING-POWER-USER"
  - "BR-PRJ-BOARD-SHARING-VERSUS-ROLE"
  - "BR-PRJ-PROJECT-BOARD-VISIBILITY"
  - "BR-PRJ-EXTERNAL-BOARD-INVITE"
uses-terms:
  - "TERM-PROJECT-TASK-BOARD"
provenance:
  source: ".private/research/help-center/articles/38422661951506.md, .private/research/help-center/articles/38422735121170.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The people on a project follow the whole task plan.

## Trigger

A colleague or outside party should follow the project's work.

## Preconditions

The user holds the Admin or Office role.

## Main Flow

1. The user shares the project board with an existing crew member found by name.
2. Or the user enters an e-mail address, a name and a role to invite an outside person; the product invites them as a crew member.
3. The product notifies colleagues; invitees gain access after accepting.

## Alternative Flows

- Removing a person from the board withdraws their access.

## Failure Conditions

Invitation never accepted: nothing is shared.

## Postconditions

Shared people see all tasks of the project on their board list but not the project itself.
