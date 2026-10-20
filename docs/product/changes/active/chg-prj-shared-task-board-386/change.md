---
id: CHG-PRJ-SHARED-TASK-BOARD-386
type: product-change
title: Accept project task-board workflows
status: proposed
base-revision: "5a92fa0cc2ad8841410e007f10391e469492915f"
operations:
  add: []
  modify:
    - UC-PRJ-WORK-WITH-TASK-BOARD
    - UC-PRJ-SHARE-PROJECT-TASK-BOARD
    - UC-PRJ-ADD-TASK
    - UC-PRJ-PROGRESS-TASK
    - UC-PRJ-CONFIGURE-TASK-STATUSES
    - UC-PRJ-PROGRESS-STATUS-AUTOMATICALLY
    - FR-PRJ-023
    - BR-PRJ-BOARD-GROUPING-SCOPE
    - BR-PRJ-BOARD-HIDING-PERSONAL
    - BR-PRJ-BOARD-IS-VIEW
    - BR-PRJ-BOARD-MOVE-EDITS-TASK
    - BR-PRJ-BOARD-SHARING-POWER-USER
    - BR-PRJ-BOARD-SHARING-VERSUS-ROLE
    - BR-PRJ-EXTERNAL-BOARD-INVITE
    - BR-PRJ-PROJECT-BOARD-VISIBILITY
    - BR-PRJ-TASK-VISIBILITY-RULES
    - BR-PRJ-TASK-CREATION-BY-BOARD-ACCESS
    - BR-PRJ-TASK-STATUS-ANY-USER
    - BR-PRJ-TASK-STATUS-DELETION
    - BR-PRJ-TASK-STATUS-ENDPOINTS
    - BR-PRJ-TASK-STATUS-MIGRATION
    - BR-PRJ-TASK-STATUS-ORDER
    - SB-PRJ-ASSIGNEE-WITHOUT-BOARD
    - SB-PRJ-DRAG-REASSIGNS
    - SB-PRJ-EXTERNAL-SEES-BOARD-NOT-PROJECT
    - SB-PRJ-TASK-ON-TWO-BOARDS
    - TERM-TASK-BOARD
    - TERM-PROJECT-TASK-BOARD
    - TERM-SAVED-VIEW
    - TERM-TASK
    - TERM-TASK-STATUS
    - TERM-TASK-VISIBILITY
  remove: []
---

## Problem

The Product Definition contains the project task-board, task progression, status configuration and board access workflows needed by #386, but their artifacts remain drafts. They are not yet an accepted basis for the browser workflow.

## Intended Product Outcome

The existing project task-board definitions become active: users can work with shared project boards, create and progress tasks, configure task statuses, and view project progress according to configured completion statuses. Board and task access follows the modeled visibility and authorization rules.

## Rationale

Issue #386 requires a real web workflow and a selected browser journey. Accepting the canonical behavior first gives the implementation a stable contract and exposes scope that API-only workflows do not demonstrate.

## Affected Product Areas

Project task boards, task creation and progression, status configuration, project progress, board sharing, task visibility and saved board views.

## Open Questions

- Does #386 deliver the complete modeled board experience (board and list views, filters, grouping, saved views and hiding boards), or only the create, assign, order and status flow named in its E2E criterion? Recommendation: include the complete project-board use case; keep the selected E2E focused on all #386 acceptance criteria.
- Does #386 include sharing with existing crew and inviting outside collaborators, including accepted-invitation access boundaries, or is that a separate story? Recommendation: include board sharing because authorization and visible records are explicit in the issue, while keeping team administration in #385.
- FR-PRJ-023 currently combines project and team boards. Should its project-board portion be accepted here while the team-board browser workflow is tracked separately? Recommendation: yes; #386 owns project-board interaction, and #385 owns team administration and membership.
- Are task subtasks, relative deadlines, overdue behavior, calendar status mapping and automatic project lifecycle effects in #386? Recommendation: no; these remain outside the selected board slice unless their owning stories (#302/#303/#389 and related backlog) explicitly include them.

## Product Acceptance

- The Product Owner confirms the proposed active artifacts match the intended #386 scope and resolves each open question.
- The accepted definition states the project-board access, task visibility and status/progress behavior used by implementation and its browser journey.
- Team administration, project lifecycle, project context, subtasks and time/deadline behavior retain their separate backlog boundaries.

## Out of Scope

Changing the modeled behavior of task subtasks, relative deadlines, overdue tasks, task calendar mapping, automatic project lifecycle, team administration, project requests, schedules, time reporting or project context; implementation, technical design, release or deployment.
