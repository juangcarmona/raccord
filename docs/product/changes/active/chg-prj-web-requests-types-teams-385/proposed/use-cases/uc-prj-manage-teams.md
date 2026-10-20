---
id: UC-PRJ-MANAGE-TEAMS
type: use-case
title: "Manage teams and team boards"
status: active
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors:
  - "ACT-OPERATIONS-COORDINATOR"
  - "ACT-CREW-MEMBER"
bounded-context: "BC-PROJECTS"
governed-by:
  - "BR-PRJ-TEAM-ACCESS-BY-MEMBERSHIP"
  - "BR-PRJ-TEAM-MEMBER-PREREQUISITE"
  - "BR-PRJ-TEAM-MEMBERSHIP-NOTICE"
uses-terms:
  - "TERM-TASK-TEAM"
  - "TERM-TEAM-TASK-BOARD"
provenance:
  source: ".private/research/help-center/articles/38422973173266.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Groups of colleagues see their shared work across projects.

## Trigger

Departments organise task work.

## Preconditions

Administrator rights for teams; Admin or Office role for team boards.

## Main Flow

1. The administrator creates a team with a name and members, or edits it.
2. A user with the Admin or Office role creates a team board for one or more teams.
3. The product shares the board with all team members and notifies members added or removed.

## Alternative Flows

None.

## Failure Conditions

A crew member without access to the task area cannot be added.

## Postconditions

Team members see team tasks and boards.
