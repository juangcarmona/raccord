---
id: CHG-PRJ-TEMPLATES-RESCHEDULE-304
type: product-change
title: Accept project templates and whole-project rescheduling
status: proposed
base-revision: "5a92fa0cc2ad8841410e007f10391e469492915f"
operations:
  add:
    - BR-PRJ-SHIFT-RECHECKS-AVAILABILITY
    - SB-PRJ-SHIFT-CONFLICT-REFUSED
  modify:
    - UC-PRJ-CREATE-PROJECT-TEMPLATE
    - UC-PRJ-CREATE-FROM-TEMPLATE
    - UC-PRJ-SHIFT-PROJECT-TIMES
    - UC-PRJ-PROGRESS-STATUS-AUTOMATICALLY
    - FR-PRJ-001
    - FR-PRJ-005
    - FR-PRJ-009
    - FR-PRJ-018
    - BR-PRJ-TEMPLATE-CONTENT
    - BR-PRJ-TEMPLATE-NOT-EDITABLE
    - BR-PRJ-TEMPLATE-INDEPENDENT
    - BR-PRJ-TEMPLATE-TASKS
    - BR-PRJ-TEMPLATE-START-ANCHOR
    - BR-PRJ-ARCHIVED-TEMPLATE-ITEM
    - BR-PRJ-SCHEDULE-REQUIRED
    - BR-PRJ-CALENDAR-QUICK-CREATE
    - BR-PRJ-INITIAL-STATUS
    - BR-PRJ-PROJECT-NUMBER-ASSIGNMENT
    - BR-PRJ-SHIFT-BY-OFFSET
    - BR-PRJ-SHIFT-TO-START
    - BR-PRJ-LINKED-PERIOD-PROPAGATION
    - BR-PRJ-QUOTE-SENT-PENDING
    - BR-PRJ-SIGNED-QUOTE-CONFIRMED
    - BR-PRJ-WAREHOUSE-STATUS-PROMPT
    - BR-INT-PROJECT-MANAGEMENT-LIFECYCLE
    - SB-PRJ-TEMPLATE-ANCHORED-SCHEDULE
    - SB-PRJ-TEMPLATE-SKIPS-ASSIGNMENTS
    - SB-PRJ-TEMPLATE-STATUS
    - SB-PRJ-SHIFT-NEGATIVE-OFFSET
    - SB-PRJ-SHIFT-TO-NEW-START
    - SB-PRJ-QUOTE-SENT-PENDING
    - SB-PRJ-SIGNED-QUOTE-CONFIRMS
    - SB-PRJ-ALL-ITEMS-MOVED-PROMPT
    - SB-PRJ-ITEM-LEFT-BEHIND-NO-PROMPT
    - TERM-PROJECT-TEMPLATE
  remove: []
---

## Problem

The Product Definition contains project template, project shift and automatic status workflows, but their artifacts remain drafts. The whole-project shift also lacks an explicit product rule proving that linked equipment availability is rechecked atomically and that a conflict leaves the schedule unchanged.

## Intended Product Outcome

Coordinators can create reusable project templates, create a project from a template anchored to a chosen date, and shift some or all project periods after a preview. Template content and starting status follow the modeled rules. Project status advances only on modeled quotation and warehouse events. Whole-project shifts recheck all affected equipment reservations atomically and are refused without partial changes when availability conflicts.

## Rationale

Issue #304 joins project templates with whole-project rescheduling, while #310 and #311 provide the equipment-line and availability mechanics. Explicitly modeling the atomic outcome prevents project times and equipment reservations from disagreeing after a conflict.

## Affected Product Areas

Project templates, template-based creation, schedule shifting, linked planning, project number and starting status, quotation and warehouse-driven project status, and equipment availability.

## Open Questions

- Confirm the complete template content from the existing model: client/location, schedule, notes, tasks (including subtasks and relative deadlines), equipment, functions, additional costs, financial details and starting status, with crew/vehicle assignments excluded. Recommendation: yes; templates should preserve the complete reusable project setup while leaving date-specific assignments empty.
- Confirm templates cannot be edited in place and are replaced by saving a new template from a project. Recommendation: yes; preserve the existing model.
- Confirm a whole-project shift rechecks all linked equipment availability through #311 atomically, and any conflict refuses the entire shift. Recommendation: yes; this is already an acceptance criterion in #304.
- Confirm that automatic status progression is limited to the existing quote and warehouse events, with no new triggers. Recommendation: yes; preserve the model and keep manual lifecycle/cancellation in #303.
- Confirm template-created projects receive a new project number and the template's starting status, while reusable client/default data boundaries remain governed by the respective contact and commercial stories (#298–#302). Recommendation: yes; use current modeled defaults without implementing contact maintenance here.
- Confirm template tasks can preserve subtasks and relative deadlines even though their shared-board UI remains #386. Recommendation: yes; #304 owns copied task content and #386 owns board interaction.

## Product Acceptance

- The Product Owner confirms the template content and assignment exclusions match intended reusable project setup.
- The Product Owner confirms whole-project shift conflicts cannot leave partially moved periods or reservations.
- The accepted model restricts automatic status changes to existing quotation and warehouse triggers and keeps manual lifecycle/cancellation separate in #303.
- Selected Projects browser evidence can cover template creation/use and whole-project rescheduling against the accepted rules.

## Out of Scope

Project cancellation and manual lifecycle UI (#303), equipment-line management and reservation implementation (#310–#311), schedule and personal-calendar workflows (#387), task-board interaction (#386), contact and commercial maintenance (#298–#302), project context (#389), implementation, technical design, release or deployment.
