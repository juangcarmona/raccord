---
id: CHG-PRJ-WEB-REQUESTS-TYPES-TEAMS-385
type: product-change
title: Accept project request, type and team workflows
status: proposed
base-revision: "5a92fa0cc2ad8841410e007f10391e469492915f"
operations:
  add: []
  modify:
    - UC-PRJ-RECEIVE-PROJECT-REQUEST
    - UC-PRJ-CONVERT-PROJECT-REQUEST
    - UC-PRJ-DECLINE-PROJECT-REQUEST
    - UC-PRJ-MANAGE-PROJECT-TYPES
    - UC-PRJ-MANAGE-TEAMS
    - FR-PRJ-015
    - FR-PRJ-017
    - FR-PRJ-024
    - BR-PRJ-REQUEST-NOTIFICATION
    - BR-PRJ-INITIAL-STATUS
    - BR-PRJ-REQUEST-DECLINE-NOTICE
    - BR-PRJ-PROJECT-TYPE-DEFAULTS
    - BR-PRJ-REMEMBER-LAST-TEMPLATE
    - BR-PRJ-TEAM-ACCESS-BY-MEMBERSHIP
    - BR-PRJ-TEAM-MEMBER-PREREQUISITE
    - BR-PRJ-TEAM-MEMBERSHIP-NOTICE
    - BR-PRJ-TASK-MULTIPLE-TEAMS
    - SB-PRJ-DECLINE-NOTIFIES-REQUESTER
    - SB-PRJ-SAVED-MATCH-SUGGESTED
    - SB-PRJ-NEW-TEAM-MEMBER-SEES-HISTORY
  remove: []
---

## Problem

The product model contains the project request, project type and team workflows needed by #385, but the related use cases, requirements, rules and scenarios are still drafts. Their semantics therefore are not yet an accepted basis for the user-facing workflows.

## Intended Product Outcome

The existing definitions for receiving, converting and declining project requests; configuring project types; and managing teams and team access become active without changing their modeled semantics. The complete definitions include request notifications and conversion status, project type defaults including document-template behavior, and team membership, visibility and access rules.

## Rationale

Issue #385 exposes these workflows through the web app. Accepting the existing product rules first gives implementation and E2E coverage a stable, canonical contract, while retaining the existing split between team administration in #385 and shared task-board presentation in #386.

## Affected Product Areas

Project request intake and conversion, project classification and defaults, team membership and task access, notifications, and project coordination.

## Open Questions

- Confirm that project type management in #385 includes the complete modeled default set: document templates, remembered last-used templates, letterhead, financial number series, invoice moment and additional conditions. Recommendation: accept the complete existing model; otherwise narrow the issue and Product Change before approval.
- Confirm that team membership and access rules become active with #385 while shared task-board UI delivery remains in #386. Recommendation: yes; #386 owns the browser workflow for boards.
- Confirm that the existing request flow's notification and saved-match behaviors are included in this accepted scope. Recommendation: yes; both are linked by the existing request requirement and scenarios.

## Product Acceptance

- The Product Owner confirms the proposed artifacts preserve the intended behavior for requests, project types, teams and access.
- The Product Owner confirms the scope boundaries in the Open Questions, especially full project type defaults and the #385/#386 team-board boundary.
- The accepted Product Definition provides the cited rules and scenarios used by the #385 implementation and its selected browser journey.

## Out of Scope

Changing domain behavior, adding new project request or classification semantics, project lifecycle and templates (#303–#304), shared task-board user interface (#386), schedules (#387), time reporting (#388), project context (#389), implementation, technical design, release or deployment.
