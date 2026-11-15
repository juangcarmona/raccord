---
id: UC-CRW-SHARE-SCHEDULE
type: use-case
title: "Share schedules with crew members"
status: draft
primary-actor: "ACT-CREW-PLANNER"
supporting-actors:
  - "ACT-CREW-MEMBER"
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-NO-AUTOMATIC-SCHEDULE-NOTIFICATION"
  - "BR-CRW-CHANGED-SINCE-LAST-SHARE"
  - "BR-CRW-SCHEDULE-PERSONAL-CONTENT"
  - "BR-CRW-NOTIFICATION-PREFERENCES-SELF-OWNED"
uses-terms:
  - "TERM-SCHEDULE-COMMUNICATION"
  - "TERM-NOTIFICATION-PREFERENCE"
provenance:
  source: ".private/research/help-center/articles/360014672919.md, .private/research/help-center/articles/360014610140.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Each crew member knows their planned work.

## Trigger

Planning was completed or changed.

## Preconditions

Final assignments exist.

## Main Flow

1. The planner selects a period and projects (or works within one project).
2. The planner selects crew members, seeing who changed since the last communication and which channels each has enabled.
3. Optionally the planner previews and adds a custom text.
4. The product sends each person their own schedule.

## Alternative Flows

None.

## Failure Conditions

Crew with all channels off will not be reached; the planner cannot change their channels.

## Postconditions

Crew have received their schedules.
