---
id: CHG-PRJ-SCHEDULES-PERSONAL-387
type: product-change
title: Accept project and personal schedule workflows
status: proposed
base-revision: "5a92fa0cc2ad8841410e007f10391e469492915f"
operations:
  add:
    - BR-PRJ-PERIOD-EDIT-AVAILABILITY
    - SB-PRJ-PERIOD-EDIT-CONFLICT-REFUSED
    - SB-PRJ-SCHEDULE-AND-PERSONAL-SCHEDULE-JOURNEY
  modify:
    - UC-PRJ-MANAGE-TIME-SCHEDULE
    - UC-PRJ-VIEW-PERSONAL-SCHEDULE
    - FR-PRJ-003
    - FR-PRJ-105
    - BR-PRJ-DEFAULT-PERIODS
    - BR-PRJ-DEFAULT-PERIOD-DESIGNATION
    - BR-PRJ-USAGE-WITHIN-PLANNING
    - BR-PRJ-PERIOD-DOCUMENT-DISPLAY
    - BR-PRJ-FUNCTIONS-FROM-PERIODS
    - BR-PRJ-OVERVIEW-DAY-SCOPE
    - BR-CRW-FUNCTION-TIMING-PROPAGATION
  remove: []
---

## Problem

The product model describes project time schedules and personal schedules, but the related workflows and requirements remain drafts. It does not explicitly define how an individual period edit is refused when linked equipment becomes unavailable.

## Intended Product Outcome

Coordinators can maintain project planning and usage periods, and users can consult personal schedules and project days according to the existing use cases. Individual period edits update linked planning and crew functions under the modeled propagation rules. If an edit would make linked equipment unavailable, the edit is refused as one operation and the original schedule and linked records remain unchanged.

## Rationale

Issue #387 exposes project and personal schedule workflows through the web app. Accepting their product contract before implementation keeps period propagation, visibility and schedule safety consistent across Projects, Crew and Warehouse.

## Affected Product Areas

Project time schedules, planning and usage periods, linked equipment and crew planning, personal schedules, project-day visibility and schedule-based project creation.

## Open Questions

- Confirm that #387 should activate the complete existing personal-schedule behavior, including viewing colleagues when authorized, availability colors, usage-period day scope and starting a project from a day. Recommendation: yes; keep the issue's browser journey focused on the required self-schedule flow and include a separate assertion for permission boundaries.
- Confirm that an individual period edit rechecks affected equipment availability atomically and refuses the full edit on conflict, preserving linked equipment and crew records. Recommendation: yes; this is explicit in #387's acceptance criteria and complements the whole-project shift rule in #304.
- Confirm that generating crew and transport functions from selected periods is included in #387's schedule workflow. Recommendation: yes; it is an existing alternative flow of UC-PRJ-MANAGE-TIME-SCHEDULE and is needed to exercise linked schedule propagation.

## Product Acceptance

- The Product Owner confirms the complete project-schedule and personal-schedule behavior and resolves each open question.
- The accepted Product Definition states the period constraints, linked propagation, availability refusal, personal visibility and day-scope behavior that #387 will implement.
- Whole-project templates and rescheduling remain in #304, while line-level equipment and availability mechanics remain in #310–#311.

## Out of Scope

Whole-project templates, whole-project shifting and automatic project status behavior (#304); project lifecycle and cancellation (#303); changes to equipment or crew scheduling semantics outside linked period propagation; implementation, technical design, release or deployment.
