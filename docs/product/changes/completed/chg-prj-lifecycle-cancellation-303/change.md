---
id: CHG-PRJ-LIFECYCLE-CANCELLATION-303
type: product-change
title: Accept project periods, lifecycle and cancellation behavior
status: applied
base-revision: "5a92fa0cc2ad8841410e007f10391e469492915f"
operations:
  add:
    - BR-PRJ-CANCEL-PRESERVES-INVOICED-WORK
    - SB-PRJ-CANCEL-PRESERVES-INVOICED-WORK
  modify:
    - UC-PRJ-EDIT-PROJECT-DETAILS
    - UC-PRJ-MANAGE-TIME-SCHEDULE
    - UC-PRJ-CHANGE-PROJECT-STATUS
    - UC-PRJ-CANCEL-PROJECT
    - FR-PRJ-011
    - BR-PRJ-DEFAULT-PERIODS
    - BR-PRJ-DEFAULT-PERIOD-DESIGNATION
    - BR-PRJ-USAGE-WITHIN-PLANNING
    - BR-PRJ-LINKED-PERIOD-PROPAGATION
    - BR-PRJ-PERIOD-DOCUMENT-DISPLAY
    - BR-PRJ-FUNCTIONS-FROM-PERIODS
    - BR-PRJ-MANUAL-STATUS-ANY-TIME
    - BR-PRJ-FIXED-STATUS-SET
    - BR-PRJ-STATUS-AREA-VISIBILITY
    - BR-WHS-STATUS-INDEPENDENCE
    - BR-PRJ-CANCEL-MANUAL-ONLY
    - BR-PRJ-CANCELLATION-ACTIONS-APPLICABLE
    - BR-PRJ-CANCEL-RELEASES-EQUIPMENT
    - BR-PRJ-CANCEL-OPEN-ITEMS-WARNING
    - BR-ISSUED-INVOICE-IMMUTABLE
    - TERM-PLANNING-PERIOD
    - TERM-USAGE-PERIOD
    - TERM-PROJECT-STATUS
    - TERM-PROJECT-STATUS-GROUP
    - TERM-PROJECT-STATUS-INQUIRY
    - TERM-PROJECT-STATUS-DRAFT
    - TERM-PROJECT-STATUS-PENDING
    - TERM-PROJECT-STATUS-CONFIRMED
    - TERM-PROJECT-STATUS-IN-PROGRESS
    - TERM-PROJECT-STATUS-EXECUTION-DONE
    - TERM-PROJECT-STATUS-COMPLETE
    - TERM-PROJECT-STATUS-CANCELLED
    - TERM-CANCELLATION-ACTION
    - SB-PRJ-CANCEL-FREES-STOCK
    - SB-PRJ-CANCEL-WITHDRAWS-INVITATIONS
    - SB-PRJ-CANCEL-WITHOUT-ACTIONS-WARNS
  remove: []
---

## Problem

The Product Definition contains the project detail, schedule, lifecycle and cancellation behavior needed by #303, but the relevant artifacts remain drafts. Their semantics are not yet an accepted basis for the Projects workflows. The issue also requires explicit protection for issued invoices and invoiced work during status changes and cancellation.

## Intended Product Outcome

Project coordinators can maintain project details and planning and usage periods, manually change project statuses according to the fixed lifecycle, and cancel projects with only applicable confirmed follow-up actions. Project status remains distinct from warehouse status. Cancellation preserves issued invoices and their invoiced work; the equipment-release rule remains part of the product outcome.

## Rationale

Issue #303 delivers lifecycle and cancellation behavior through the web app. Accepting the related definitions makes status effects, period propagation, cancellation choices, equipment availability and invoice preservation explicit before implementation. The cross-context implementation and browser evidence for equipment release remain assigned to #384.

## Affected Product Areas

Project details, project time schedules, planning and usage periods, fixed project statuses, cancellation actions, equipment availability after cancellation, and issued invoice history.

## Accepted Product Decisions

- #303 delivers the complete modeled project detail workflow, including existing contact selection, commercial overrides, notes, tasks and files. Contact/person maintenance, commercial defaults and project-context UI remain with #298–#302 and #389.
- #303 includes planning/usage period designation, propagation to linked equipment/functions, document visibility and crew/transport function generation as defined by UC-PRJ-MANAGE-TIME-SCHEDULE.
- Equipment reservation release on cancellation remains canonical here; #303 owns the project-side action and #384 verifies the cross-context implementation and journey.
- Modeled automatic status progression remains accepted, while #303 implements manual status changes and cancellation and #304 implements automatic progression.
- Issued invoice records and their invoiced project work remain immutable and linked in project history through status changes and cancellation.

## Open Questions

None. The prior questions are resolved by the decisions above and the Owner's authorization of the requested scope.

## Product Acceptance

The Product Owner authorized the requested unattended implementation of #303, #304 and the Projects workflow block, including the product-scope decisions above. This approval applies to the stated boundaries; implementation remains subject to the OpenSpec plan, tests, architecture decisions and CI evidence.

The accepted model proves that issued invoices and their invoiced project work survive status changes and cancellation unchanged.

## Out of Scope

Template management and whole-project rescheduling (#304), user-facing calendar workflows (#387), equipment reservation release implementation and cross-context E2E integration (#384), project requests and team workflows (#385), task-board workflows (#386), contact and commercial-default workflows (#298–#302), project context UI (#389), implementation, technical design, release or deployment.
