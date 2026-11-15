---
id: UC-CRW-RESOLVE-PLANNING-CONFLICTS
type: use-case
title: "Resolve planning conflicts"
status: draft
primary-actor: "ACT-CREW-PLANNER"
supporting-actors: []
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-NO-AUTOMATIC-SCHEDULE-NOTIFICATION"
  - "BR-CRW-AGENCY-MULTIPLE-BOOKING"
uses-terms:
  - "TERM-PLANNING-CONFLICT"
provenance:
  source: ".private/research/help-center/articles/14488851931026.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

No one is double booked or planned while unavailable.

## Trigger

Project times shifted or availability changed.

## Preconditions

Conflicting assignments exist.

## Main Flow

1. The planner identifies double-booked or unavailable crew.
2. The planner removes all crew from the project or only the conflicting assignments.
3. The planner plans others.
4. The planner communicates the revised schedule.

## Alternative Flows

None.

## Failure Conditions

- The planner lacks permission to change planning: the removal or new planning is refused and the conflicts remain.
- A crew member flagged as usable several times at once is not treated as double booked, so no conflict is raised for them (BR-CRW-AGENCY-MULTIPLE-BOOKING).
- Another user changed the same assignments in the meantime: the change is refused and the planner must reload and retry.
- Removing or replanning crew notifies nobody by itself; until the planner communicates the revised schedule, crew keep working from the old one (BR-CRW-NO-AUTOMATIC-SCHEDULE-NOTIFICATION).
- The schedule cannot be sent because the mail service is unreachable: the failure is shown and the communication can be retried.

## Postconditions

Conflicts are removed; crew are informed once the schedule is shared.
